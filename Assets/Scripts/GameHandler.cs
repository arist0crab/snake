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

        currntFood = foodManager.SpawnFood();
    }

    void Update()
    {
        CheckSnakeEatFood();
    }

    private void CheckSnakeEatFood()
    {
        if (snake.snakeGridPosition == foodManager.foodGridPosition)
        {
            Object.Destroy(currntFood);
            currntFood = foodManager.SpawnFood();

            // TODO
        }
    }
}
