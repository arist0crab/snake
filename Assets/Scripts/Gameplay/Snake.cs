using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class Snake : MonoBehaviour
{
    private class SnakeBodyPart
    {
        public Vector3Int Position { get; set; }
        public GameObject Embodiment { get; private set; } 

        public SnakeBodyPart(Vector3Int position, GameObject embodiment)
        {
            Position = position;
            Embodiment = embodiment;
        }
    }

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
        if (SnakeInputManager.Instance != null)
            SnakeInputManager.Instance.OnMoveInput += HandleMovement;
    }

    private void OnDisable()
    {
        if (SnakeInputManager.Instance != null)
            SnakeInputManager.Instance.OnMoveInput -= HandleMovement;
    }

    void Update()
    {
        if (!IsAlive) return;

        HandleGridMovement(); 
        UpdateBodyVisual();       
    }

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public void Grow() => shouldGrow = true;
    public void SetDead() => IsAlive = false;

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
            SnakeBodyPart newPart = new SnakeBodyPart(SnakeGridPosition, newGameObject);
            snakeBodyParts.Insert(0, newPart);
            shouldGrow = false;
        }
        else if (snakeBodyParts.Count > 0)
        {
            SnakeBodyPart tailEnd = snakeBodyParts[snakeBodyParts.Count - 1];
            snakeBodyParts.RemoveAt(snakeBodyParts.Count - 1);
            tailEnd.Position = SnakeGridPosition;
            snakeBodyParts.Insert(0, tailEnd);
        }

        SnakeGridPosition += currentMoveDirection;
        OnSnakeMoved?.Invoke(SnakeGridPosition);
    }

    private void UpdateBodyVisual()
    {
        if (groundTilemap == null) 
            return;

        transform.position = groundTilemap.GetCellCenterWorld(SnakeGridPosition);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(currentMoveDirection));

        foreach (var bodyPart in snakeBodyParts)
            bodyPart.Embodiment.transform.position = groundTilemap.GetCellCenterWorld(bodyPart.Position);
    }

    private float GetAngleFromVector(Vector3Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
