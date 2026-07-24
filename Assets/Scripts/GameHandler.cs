using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private static int score;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Snake snake;
    [SerializeField] private FoodManager foodManager;

    private GameObject currntFood;

    private void Awake()
    {
        instance = this;
        InitializeStatic();
    }

    void Start()
    {
        snake.OnSnakeMoved += CheckSnakeCollision;
        snake.OnSnakeMoved += CheckGridBordersCollision;
        snake.OnSnakeMoved += CheckSnakeEatFood;

        snake.SetupTilemap(groundTilemap);
        foodManager.SetupTilemap(groundTilemap);

        currntFood = foodManager.SpawnFood(snake.GetFullSnakePositionList());
    }

    void Update()
    {
        if (!snake.isAlive)
        {
            GameOverWindow.ShowStatic();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            GameHandler.PauseGame();
    }

    public static void InitializeStatic()
    {
        score = 0;
    }

    public static void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }

    public static void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }

    public static int GetScore()
    {
        return score;
    }

    private void CheckSnakeEatFood(Vector3Int snakeNewGridPosition)
    {
        if (snakeNewGridPosition == foodManager.foodGridPosition)
        {
            Destroy(currntFood);
            snake.Grow();
            score += 1;
            currntFood = foodManager.SpawnFood(snake.GetFullSnakePositionList());
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
