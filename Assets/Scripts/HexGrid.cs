using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab; // Prefab do hexágono
    public int width = 10;
    public int height = 10;
    private float hexWidth = 1.732f; // Largura de um hexágono
    private float hexHeight = 2.0f; // Altura de um hexágono

    void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calcula a posição de cada hexágono
                float xPos = x * hexWidth;
                if (y % 2 == 1)
                {
                    xPos += hexWidth / 2; // Offset para colunas ímpares
                }
                float zPos = y * (hexHeight * 0.75f);

                // Instancia o hexágono na posição calculada
                Vector3 position = new Vector3(xPos, 0, zPos);
                Instantiate(hexPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
