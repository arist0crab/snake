using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Snake : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;

    private Vector3Int gridPosition;
    private Vector3Int gridMoveDirection;

    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private class SnakeMoveDirection
    {
        public static Vector3Int Stay = new Vector3Int(0, 0);
        public static Vector3Int Left = new Vector3Int(-1, 0);
        public static Vector3Int Right = new Vector3Int(1, 0);
        public static Vector3Int Top = new Vector3Int(0, 1);
        public static Vector3Int Down = new Vector3Int(0, -1);
    }

    private void Awake()
    {
        gridPosition = new Vector3Int(0, 0);
        gridMoveDirection = SnakeMoveDirection.Stay;

        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
    }


    void Start()
    {
        
    }

    void Update()
    {
        HandleInput();
        HandleGridMovement();        
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            if (gridMoveDirection != SnakeMoveDirection.Down)
                gridMoveDirection = SnakeMoveDirection.Top;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            if (gridMoveDirection != SnakeMoveDirection.Top)
                gridMoveDirection = SnakeMoveDirection.Down;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            if (gridMoveDirection != SnakeMoveDirection.Right)
                gridMoveDirection = SnakeMoveDirection.Left;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            if (gridMoveDirection != SnakeMoveDirection.Left)
                gridMoveDirection = SnakeMoveDirection.Right;
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;

            transform.position = groundTilemap.GetCellCenterWorld(gridPosition);
        }
    }
}
