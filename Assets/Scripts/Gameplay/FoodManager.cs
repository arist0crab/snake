using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;

    public Vector3Int FoodGridPosition { get; private set; }

    private Tilemap groundTilemap;
    private GameObject currentFood;

    public void SetupTilemap(Tilemap groundTilemap)
    {
        this.groundTilemap = groundTilemap;
    }

    public bool TryEat(Vector3Int snakePosition)
    {
        Vector3 snakeWorldPosition = groundTilemap.GetCellCenterWorld(snakePosition);
        return snakeWorldPosition == currentFood.transform.position;
    }

    public GameObject SpawnFood(List<Vector3Int> fullSnakePositionList)
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        do
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);

            FoodGridPosition = new Vector3Int(randomX, randomY, 0);
        }
        while (fullSnakePositionList.IndexOf(FoodGridPosition) != -1);

        if (currentFood) Destroy(currentFood);
    
        currentFood = Instantiate(foodPrefab);
        currentFood.transform.position = groundTilemap.GetCellCenterWorld(FoodGridPosition);

        return currentFood;
    }
}
