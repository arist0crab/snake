using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    private int score = 0;

    [SerializeField] private Snake snake;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private FoodManager foodManager;

    private GameObject currntFood;
    private bool isGameRun = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ApplicationInputManager.Instance.OnPausePressed += HandlePause;

        snake.SetupTilemap(groundTilemap);
        foodManager.SetupTilemap(groundTilemap);

        currntFood = foodManager.SpawnFood(snake.GetFullSnakePositionList());
    }

    void OnEnable()
    {
        snake.OnSnakeMoved += CheckSnakeCollision;
        snake.OnSnakeMoved += CheckGridBordersCollision;
        snake.OnSnakeMoved += CheckSnakeEatFood;
    }

    void OnDisable()
    {
        snake.OnSnakeMoved -= CheckSnakeCollision;
        snake.OnSnakeMoved -= CheckGridBordersCollision;
        snake.OnSnakeMoved -= CheckSnakeEatFood;
    }

    void Update()
    {
        if (!snake.IsAlive)
        {
            GameOverWindow.ShowStatic();
            return;
        }
    }

    private void HandlePause()
    {
        if (isGameRun) PauseGame();
        else ResumeGame();
    }

    public void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
        isGameRun = false;
    }

    public void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
        isGameRun = true;
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

    private void CheckSnakeCollision(Vector3Int snakeNewGridPosition)
    {
        int snakeHeadX = snakeNewGridPosition.x;
        int snakeHeadY = snakeNewGridPosition.y;

        List<Vector3Int> fullSnakePositionList = snake.GetFullSnakePositionList();

        for (int i = 1; i < fullSnakePositionList.Count; i++)
        {
            Vector3Int bodypartVector = fullSnakePositionList[i];
            if (snakeHeadX == bodypartVector.x && snakeHeadY == bodypartVector.y)
            {
                snake.SetDead();
                return;
            }
        }
    }  
}
