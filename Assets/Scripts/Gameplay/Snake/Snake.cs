using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEditor;


public class Snake : MonoBehaviour
{
    private class SnakeBodyPart
    {
        public Vector3Int Position { get; set; }
        public Vector3Int Direction { get; set; }
        public Vector3Int PrevDirection { get; set; }
        public GameObject Embodiment { get; private set; } 

        public SnakeBodyPart(GameObject embodiment, Vector3Int position, Vector3Int direction, Vector3Int prevDirection = default)
        {
            Position = position;
            Direction = direction;
            PrevDirection = prevDirection;
            Embodiment = embodiment;
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
        List<Vector3Int> fullSnakePositionList = new List<Vector3Int>() { SnakeGridPosition };
        
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
            Vector3Int previousDirection = snakeBodyParts[0].Direction;
            SnakeBodyPart tailEnd = snakeBodyParts[^1];
            snakeBodyParts.RemoveAt(snakeBodyParts.Count - 1);
            tailEnd.Direction = currentMoveDirection;
            tailEnd.PrevDirection = previousDirection;
            tailEnd.Position = SnakeGridPosition;
            snakeBodyParts.Insert(0, tailEnd);
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
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(currentMoveDirection));

        for (int i = 0; i < snakeBodyParts.Count; i++)
        {
            SnakeBodyPart bodyPart = snakeBodyParts[i];

            bodyPart.Embodiment.transform.position = groundTilemap.GetCellCenterWorld(bodyPart.Position);
            bodyPart.Embodiment.transform.eulerAngles = new(0, 0, GetAngleFromVector(bodyPart.Direction));  

            SpriteRenderer spriteRenderer = bodyPart.Embodiment.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = snakeTextures.SnakeBodyStraight;

            if (bodyPart.Direction != bodyPart.PrevDirection && bodyPart.PrevDirection != Vector3Int.zero)
            {
                spriteRenderer.sprite = snakeTextures.SnakeBodyCorner;
                float angleZ = CalculateCornerAngle(bodyPart.PrevDirection, bodyPart.Direction);
                bodyPart.Embodiment.transform.eulerAngles = new(0, 0, angleZ);
            }
        }
    }

    private float CalculateCornerAngle(Vector3Int prevDir, Vector3Int currentDir)
    {
        Vector3Int sum = prevDir + currentDir;

        if (prevDir == Vector3Int.left && currentDir == Vector3Int.up)
            return 180f;

        if (prevDir == Vector3Int.down && currentDir == Vector3Int.right)
            return 180f;

        if (prevDir == Vector3Int.down && currentDir == Vector3Int.left)
            return 270f;

        if (prevDir == Vector3Int.right && currentDir == Vector3Int.up)
            return 270f;

        if (prevDir == Vector3Int.right && currentDir == Vector3Int.down)
            return 0f;

        if (prevDir == Vector3Int.up && currentDir == Vector3Int.left)
            return 0f;

        if (prevDir == Vector3Int.up && currentDir == Vector3Int.right)
            return 90f;

        if (prevDir == Vector3Int.left && currentDir == Vector3Int.down)
            return 90f;

        return 0f;
    }

    private void CheckSnakeCollision(Vector3Int newSnakeGridPosition)
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyParts)
            if (newSnakeGridPosition == snakeBodyPart.Position)
                SetDead();
    }

    private float GetAngleFromVector(Vector3Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
