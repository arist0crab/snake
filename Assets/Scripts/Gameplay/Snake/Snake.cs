using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    private class SnakeBodyPart
    {
        public enum SnakeBodyPartType
        {
            Normal, 
            Corner,
            Tail
        }

        public Vector3Int Position { get; set; }
        public Vector3Int Direction { get; set; }
        public Vector3Int PrevDirection { get; set; }
        public GameObject Embodiment { get; private set; } 
        public SnakeBodyPartType Type => DetermineType();

        public SnakeBodyPart(GameObject embodiment, Vector3Int position, Vector3Int direction, Vector3Int prevDirection = default)
        {
            Position = position;
            Direction = direction;
            PrevDirection = prevDirection;
            Embodiment = embodiment;
        }

        public void UpdateVisual(Tilemap groundTilemap, SnakeTextures textures)
        {
            SpriteRenderer spriteRenderer = Embodiment.GetComponent<SpriteRenderer>();
            Embodiment.transform.position = groundTilemap.GetCellCenterWorld(Position);
            Embodiment.transform.eulerAngles = GetCurrentRotationVector();
            spriteRenderer.sprite = GetBodyPartSprite(textures);
        }

        private Sprite GetBodyPartSprite(SnakeTextures textures)
        {
            return Type switch
            {
                SnakeBodyPartType.Tail => textures.SnakeBodyTail,
                SnakeBodyPartType.Corner => textures.SnakeBodyCorner,
                _ => textures.SnakeBodyStraight,
            };
        }

        private Vector3 GetCurrentRotationVector()
        {
            if (Type == SnakeBodyPartType.Corner)
                return GetCornerRotationVector();

            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            return new(0, 0, (angle + 360f) % 360f);
        }

        private Vector3Int GetCornerRotationVector()
        {
            if (PrevDirection == Vector3Int.left && Direction == Vector3Int.up)
                return new(0, 0, 180);

            if (PrevDirection == Vector3Int.down && Direction == Vector3Int.right)
                return new(0, 0, 180);

            if (PrevDirection == Vector3Int.down && Direction == Vector3Int.left)
                return new(0, 0, 270);

            if (PrevDirection == Vector3Int.right && Direction == Vector3Int.up)
                return new(0, 0, 270);

            if (PrevDirection == Vector3Int.up && Direction == Vector3Int.right)
                return new(0, 0, 90);

            if (PrevDirection == Vector3Int.left && Direction == Vector3Int.down)
                return new(0, 0, 90);

            return new(0, 0, 0);
        }

        private SnakeBodyPartType DetermineType()
        {
            if (PrevDirection == Vector3Int.zero)
                return SnakeBodyPartType.Tail;

            if (PrevDirection != Direction)
                return SnakeBodyPartType.Corner;

            return SnakeBodyPartType.Normal;
        }
    }

    [SerializeField] private SnakeTextures snakeTextures;
    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private float gridMoveTimerMax;

    public event Action OnSnakeDead;
    public event Action<Vector3Int> OnSnakeMoved;

    public Vector3Int SnakeGridPosition { get; private set; }
    public Vector3Int SnakeGridMoveDirection => currentMoveDirection;
    public bool IsAlive { get; private set; } = true; 

    private Tilemap groundTilemap;

    private Vector3Int currentMoveDirection;
    private Vector3Int nextMoveDirection;
    private readonly List<SnakeBodyPart> snakeBodyParts = new List<SnakeBodyPart>();

    private bool shouldGrow = false;

    private float gridMoveTimer;

    private void Awake()
    {
        SnakeGridPosition = Vector3Int.zero;
        currentMoveDirection = Vector3Int.zero;

        gridMoveTimer = gridMoveTimerMax;
    }

    private void OnEnable()
    {
        OnSnakeMoved += CheckSnakeCollision;

        if (SnakeInputManager.Instance != null)
            SnakeInputManager.Instance.OnMoveInput += HandleMovement;
    }

    private void OnDisable()
    {
        OnSnakeMoved -= CheckSnakeCollision;

        if (SnakeInputManager.Instance != null)
            SnakeInputManager.Instance.OnMoveInput -= HandleMovement;
    }

    void Update()
    {
        if (!IsAlive) return;
        if (!GameHandler.Instance.IsGameRun) return;

        HandleGridMovement();       
    }

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public void Grow() => shouldGrow = true;
    public void SetDead()
    {
        IsAlive = false;
        OnSnakeDead?.Invoke();
    }

    public List<Vector3Int> GetFullSnakePositionList()
    {
        List<Vector3Int> fullSnakePositionList = new() { SnakeGridPosition };
        
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyParts)
            fullSnakePositionList.Add(snakeBodyPart.Position);

        return fullSnakePositionList;
    }

    private void HandleMovement(Vector3Int newMoveDirection)
    {
        if (currentMoveDirection != Vector3Int.zero && newMoveDirection == -currentMoveDirection)
            return;

        nextMoveDirection = newMoveDirection;
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer < gridMoveTimerMax)
            return;

        gridMoveTimer -= gridMoveTimerMax;
        currentMoveDirection = nextMoveDirection;

        if (shouldGrow)
        {

            GameObject newGameObject = Instantiate(snakeBodyPrefab);
            SnakeBodyPart newPart = new(newGameObject, SnakeGridPosition, currentMoveDirection);
            
            if (snakeBodyParts.Count > 0)
                newPart.PrevDirection = snakeBodyParts[0].Direction;
                
            snakeBodyParts.Insert(0, newPart);
            shouldGrow = false;
        }
        else if (snakeBodyParts.Count > 0)
        {
            // TODO refactor
            Vector3Int previousDirection = snakeBodyParts[0].Direction;
            SnakeBodyPart tailEnd = snakeBodyParts[^1];
            snakeBodyParts.RemoveAt(snakeBodyParts.Count - 1);
            tailEnd.Direction = currentMoveDirection;
            tailEnd.PrevDirection = previousDirection;
            tailEnd.Position = SnakeGridPosition;
            snakeBodyParts.Insert(0, tailEnd);
            snakeBodyParts[^1].PrevDirection = Vector3Int.zero;
        }

        SnakeGridPosition += currentMoveDirection;
        OnSnakeMoved?.Invoke(SnakeGridPosition);

        if (!IsAlive) return;

        UpdateBodyVisual();
    }

    private void UpdateBodyVisual()
    {
        if (groundTilemap == null) return;
        if (IsAlive == false) return;

        transform.position = groundTilemap.GetCellCenterWorld(SnakeGridPosition);
        transform.eulerAngles = GetNormalRotationVector(currentMoveDirection);

        for (int i = 0; i < snakeBodyParts.Count; i++)
            snakeBodyParts[i].UpdateVisual(groundTilemap, snakeTextures);
    }

    private void CheckSnakeCollision(Vector3Int newSnakeGridPosition)
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyParts)
            if (newSnakeGridPosition == snakeBodyPart.Position)
                SetDead();
    }

    private Vector3 GetNormalRotationVector(Vector3Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, (angle + 360f) % 360f);
    }
}
