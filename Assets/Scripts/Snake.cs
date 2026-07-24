using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject snakeBodyPrefab;

    private Tilemap groundTilemap;

    public event Action<Vector3Int> OnSnakeMoved;

    public Vector3Int snakeGridPosition { get; private set; }
    public Vector3Int snakeGridMoveDirection { get; private set; }
    private int snakeBodySize;
    private bool shouldGrow;
    private List<Vector3Int> snakeMovePositionList;
    private List<GameObject> snakeBodyPatrsList;
    public bool isAlive { get; private set; } = true; 

    private float gridMoveTimer;
    private float gridMoveTimerMax;

    public class SnakeMoveDirection
    {
        public static Vector3Int Stay = new Vector3Int(0, 0);
        public static Vector3Int Left = new Vector3Int(-1, 0);
        public static Vector3Int Right = new Vector3Int(1, 0);
        public static Vector3Int Top = new Vector3Int(0, 1);
        public static Vector3Int Down = new Vector3Int(0, -1);
    }

    private void Awake()
    {
        SnakeInputManager.Instance.OnMoveInput += HandleMovement;

        snakeGridPosition = new Vector3Int(0, 0);
        snakeGridMoveDirection = SnakeMoveDirection.Stay;

        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;

        snakeBodySize = 0;
        shouldGrow = false;
        snakeMovePositionList = new List<Vector3Int>();
        snakeBodyPatrsList = new List<GameObject>();
    }

    void Update()
    {
        HandleGridMovement(); 
        UpdateBodyVisual();       
    }

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public void Grow()
    {
        shouldGrow = true;
    }

    public void SetDead()
    {
        isAlive = false;
    }

    public List<Vector3Int> GetFullSnakePositionList()
    {
        List<Vector3Int> fullSnakePositionList = new List<Vector3Int>() { snakeGridPosition };
        fullSnakePositionList.AddRange(snakeMovePositionList);
        return fullSnakePositionList;
    }

    private void HandleMovement(Vector3Int newSnakeGridMoveDirection)
    {
        if (newSnakeGridMoveDirection == SnakeMoveDirection.Down && snakeGridMoveDirection == SnakeMoveDirection.Top)
            return;

        if (newSnakeGridMoveDirection == SnakeMoveDirection.Top && snakeGridMoveDirection == SnakeMoveDirection.Down)
            return;
        
        if (newSnakeGridMoveDirection == SnakeMoveDirection.Left && snakeGridMoveDirection == SnakeMoveDirection.Right)
            return;

        if (newSnakeGridMoveDirection == SnakeMoveDirection.Right && snakeGridMoveDirection == SnakeMoveDirection.Left)
            return;

        snakeGridMoveDirection = newSnakeGridMoveDirection;
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, snakeGridPosition);

            if (snakeMovePositionList.Count >= snakeBodySize && !shouldGrow)
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

            if (shouldGrow)
            {
                GameObject newBodypart = Instantiate(snakeBodyPrefab);
                snakeBodyPatrsList.Add(newBodypart);
                snakeBodySize++;
                shouldGrow = false;
            }

            snakeGridPosition += snakeGridMoveDirection;
            OnSnakeMoved?.Invoke(snakeGridPosition);
        }
    }

    private float GetAngleFromVector(Vector3Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void UpdateBodyVisual()
    {
        if (isAlive == false)
            return;

        transform.position = groundTilemap.GetCellCenterWorld(snakeGridPosition);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(snakeGridMoveDirection));

        for (int i = 0; i < snakeBodySize; i++)
        {
            GameObject currentBodypart = snakeBodyPatrsList[i];
            Vector3Int currentGridPosition = snakeMovePositionList[i];
            currentBodypart.transform.position = groundTilemap.GetCellCenterWorld(currentGridPosition);
        }
    }
}
