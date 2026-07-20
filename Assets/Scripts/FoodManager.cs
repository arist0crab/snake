using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;

    private Tilemap groundTilemap;
    private Snake snake;

    private Vector3Int gridPosition;

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public void SetupSnake(Snake snake)
    {
        this.snake = snake;
    }

    public void SpawnFood()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        int randomX = Random.Range(bounds.xMin, bounds.xMax);
        int randomY = Random.Range(bounds.yMin, bounds.yMax);

        gridPosition = new Vector3Int(randomX, randomY, 0);

        GameObject food = Instantiate(foodPrefab);
        food.transform.position = groundTilemap.GetCellCenterWorld(gridPosition);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
