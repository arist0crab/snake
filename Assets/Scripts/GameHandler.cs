using System.Collections.Generic;
using UnityEngine;
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
    }

    void Start()
    {
        score = 0;

        snake.SetupTilemap(groundTilemap);
        foodManager.SetupTilemap(groundTilemap);

        currntFood = foodManager.SpawnFood(snake.GetFullSnakePositionList());
    }

    void Update()
    {
        CheckGridBordersCollision();
        CheckSnakeCollision();
        CheckSnakeEatFood();
    }

    public static int GetScore()
    {
        return score;
    }

    private void CheckSnakeEatFood()
    {
        if (snake.snakeGridPosition == foodManager.foodGridPosition)
        {
            Object.Destroy(currntFood);
            snake.Grow();
            score += 1;
            currntFood = foodManager.SpawnFood(snake.GetFullSnakePositionList());
        }
    }

    private void CheckGridBordersCollision()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        int snakeHeadX = snake.snakeGridPosition.x;
        int snakeHeadY = snake.snakeGridPosition.y;

        if (snakeHeadX < bounds.xMin || snakeHeadX > bounds.xMax)
            Debug.Log("произошело столкновение в стену по X");
        
        if (snakeHeadY < bounds.yMin || snakeHeadY > bounds.yMax)
            Debug.Log("произошло столкновение в стену по Y");
    }

    private void CheckSnakeCollision()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        int snakeHeadX = snake.snakeGridPosition.x;
        int snakeHeadY = snake.snakeGridPosition.y;

        List<Vector3Int> fullSnakePositionList = snake.GetFullSnakePositionList();

        for (int i = 1; i < fullSnakePositionList.Count; i++)
        {
            Vector3Int bodypartVector = fullSnakePositionList[i];
            if (snakeHeadX == bodypartVector.x && snakeHeadY == bodypartVector.y)
                Debug.Log("произошел столкновение со своим же хвостом");
        }
    }  
}
