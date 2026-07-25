using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private float gridMoveTimerMax;

    public event Action<Vector3Int> OnSnakeMoved;

    public Vector3Int SnakeGridPosition { get; private set; }
    public Vector3Int SnakeGridMoveDirection => currentMoveDirection;
    public bool IsAlive { get; private set; } = true; 

    private Tilemap groundTilemap;

    private Vector3Int currentMoveDirection;
    private Vector3Int nextMoveDirection;

    private int snakeBodySize = 0;
    private bool shouldGrow = false;

    private readonly List<Vector3Int> snakeMovePositionList = new List<Vector3Int>();
    private readonly List<GameObject> snakeBodyPatrsList = new List<GameObject>();

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
        fullSnakePositionList.AddRange(snakeMovePositionList);
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

        snakeMovePositionList.Insert(0, SnakeGridPosition);

        if (snakeMovePositionList.Count >= snakeBodySize && !shouldGrow)
            snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

        if (shouldGrow)
        {
            GameObject newBodypart = Instantiate(snakeBodyPrefab);
            snakeBodyPatrsList.Add(newBodypart);
            snakeBodySize++;
            shouldGrow = false;
        }

        SnakeGridPosition += currentMoveDirection;
        OnSnakeMoved?.Invoke(SnakeGridPosition);
    }

    private void UpdateBodyVisual()
    {
        if (IsAlive == false)
            return;

        transform.position = groundTilemap.GetCellCenterWorld(SnakeGridPosition);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(currentMoveDirection));

        for (int i = 0; i < snakeBodySize; i++)
        {
            GameObject currentBodypart = snakeBodyPatrsList[i];
            Vector3Int currentGridPosition = snakeMovePositionList[i];
            currentBodypart.transform.position = groundTilemap.GetCellCenterWorld(currentGridPosition);
        }
    }

    private float GetAngleFromVector(Vector3Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
