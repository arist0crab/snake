using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Snake snake;
    [SerializeField] private FoodManager foodManager;

    private GameObject currntFood;


    void Start()
    {
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

    private void CheckSnakeEatFood()
    {
        if (snake.snakeGridPosition == foodManager.foodGridPosition)
        {
            Object.Destroy(currntFood);
            snake.Grow();
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

        foreach (Vector3Int bodypartVector in fullSnakePositionList)
        {
            if (snakeHeadX == bodypartVector.x && snakeHeadY == bodypartVector.y)
                Debug.Log("произошел столкновение со своим же хвостом");
        }
    }  
}
