using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [Header("Prefab del bloque")]
    public GameObject blockPrefab;

    [Header("Dimensiones de la pared")]
    public int rows = 5;
    public int columns = 6;

    [Header("Espaciado entre bloques")]
    public float spacing = 1.1f;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        if (blockPrefab == null) return;

        // Centramos la pared
        float offsetX = (columns - 1) * spacing / 2f;
        float offsetY = (rows - 1) * spacing / 2f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 pos = transform.position + new Vector3(x * spacing - offsetX, y * spacing - offsetY, 0f);
                Instantiate(blockPrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
