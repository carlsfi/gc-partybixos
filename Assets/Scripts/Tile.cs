using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public List<Tile> nextTiles = new List<Tile>(); // Lista de próximos tiles

    public Tile GetNextTile(int choiceIndex)
    {
        if (choiceIndex >= 0 && choiceIndex < nextTiles.Count)
        {
            return nextTiles[choiceIndex];
        }

        return null; // Retorna null se a escolha for inválida
    }
}
