using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Snake snake;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private FoodManager foodManager;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        snake.SetupTilemap(groundTilemap);
        foodManager.SetupTilemap(groundTilemap);
        foodManager.SpawnFood(snake.GetFullSnakePositionList());
    }

    void OnEnable()
    {
        snake.OnSnakeMoved += CheckGridBordersCollision;
        snake.OnSnakeMoved += CheckSnakeEatFood;
    }

    void OnDisable()
    {
        snake.OnSnakeMoved -= CheckGridBordersCollision;
        snake.OnSnakeMoved -= CheckSnakeEatFood;
    }

    public int GetScore()
    {
        return score;
    }

    private void CheckSnakeEatFood(Vector3Int snakeNewGridPosition)
    {
        if (foodManager.TryEat(snakeNewGridPosition))
        {
            score++;
            snake.Grow();
            foodManager.SpawnFood(snake.GetFullSnakePositionList());
        }
    }

    private void CheckGridBordersCollision(Vector3Int snakeNewGridPosition)
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        int snakeHeadX = snakeNewGridPosition.x;
        int snakeHeadY = snakeNewGridPosition.y;

        if (snakeHeadX < bounds.xMin || snakeHeadX > bounds.xMax - 1)
            snake.SetDead();
        
        if (snakeHeadY < bounds.yMin || snakeHeadY > bounds.yMax - 1)
            snake.SetDead();
    }
}
