using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;

    private Tilemap groundTilemap;

    private GameObject food;
    public Vector3Int foodGridPosition { get; private set; }

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public GameObject SpawnFood()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        int randomX = Random.Range(bounds.xMin, bounds.xMax);
        int randomY = Random.Range(bounds.yMin, bounds.yMax);

        foodGridPosition = new Vector3Int(randomX, randomY, 0);
    
        food = Instantiate(foodPrefab);
        food.transform.position = groundTilemap.GetCellCenterWorld(foodGridPosition);

        return food;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
