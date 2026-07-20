using UnityEngine;
using UnityEngine.Tilemaps;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Snake snake;
    [SerializeField] private FoodManager foodManager;


    void Start()
    {
        snake.SetupTilemap(groundTilemap);
        snake.SetupFood(foodManager);

        foodManager.SetupTilemap(groundTilemap);
        foodManager.SetupSnake(snake);

        foodManager.SpawnFood();
    }

    void Update()
    {
        
    }
}
