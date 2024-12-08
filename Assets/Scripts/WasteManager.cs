using UnityEngine;
using System.Collections.Generic;

public class WasteManager : MonoBehaviour
{
    public List<GameObject> plasticWaste, glassWaste, metalWaste, paperWaste;
    public Transform plasticBin, glassBin, metalBin, paperBin;
    public Vector3 spawnAreaMin, spawnAreaMax;
    public int numberOfWastesToSpawn = 10;

    private List<GameObject> allWastePrefabs = new List<GameObject>();
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        ConsolidateWastePrefabs();
        SpawnWastes();
    }

    // Consolida todos os prefabs de lixo em uma única lista
    private void ConsolidateWastePrefabs()
    {
        allWastePrefabs.Clear();
        allWastePrefabs.AddRange(plasticWaste);
        allWastePrefabs.AddRange(glassWaste);
        allWastePrefabs.AddRange(metalWaste);
        allWastePrefabs.AddRange(paperWaste);

        if (allWastePrefabs.Count == 0)
        {
            Debug.LogError("Nenhum prefab de lixo foi adicionado nas listas.");
        }
    }

    // Spawna lixos aleatoriamente dentro do mapa
    private void SpawnWastes()
    {
        if (allWastePrefabs.Count == 0)
        {
            Debug.LogError("Nenhum prefab disponível para spawnar.");
            return;
        }

        for (int i = 0; i < numberOfWastesToSpawn; i++)
        {
            GameObject wastePrefab = allWastePrefabs[Random.Range(0, allWastePrefabs.Count)];
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMin.y,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject spawnedWaste = Instantiate(wastePrefab, spawnPosition, Quaternion.identity);
            spawnedWaste.tag = "Lixo";
        }
    }

    // Deposita o lixo em uma lixeira
    public void DepositWaste(GameObject waste, Transform bin, int playerNumber)
    {
        string wasteType = GetWasteType(waste.name);

        // Verifica se o lixo foi depositado na lixeira correta
        bool correctBin = (bin == plasticBin && wasteType == "plastico") ||
                          (bin == glassBin && wasteType == "vidro") ||
                          (bin == metalBin && wasteType == "metal") ||
                          (bin == paperBin && wasteType == "papel");

        if (correctBin)
        {
            Debug.Log($"Jogador {playerNumber} acertou!");
            scoreManager.AddPoints(playerNumber, 1);
        }
        else
        {
            Debug.Log($"Jogador {playerNumber} errou!");
        }

        Destroy(waste); // Remove o lixo da cena
    }

    // Verifica se o lixo está na lixeira correta
    public bool IsWasteInCorrectBin(GameObject waste, Transform bin)
    {
        string wasteType = GetWasteType(waste);

        // Verifica se o tipo do lixo corresponde à lixeira
        if ((wasteType == "plastico" && bin == plasticBin) ||
            (wasteType == "vidro" && bin == glassBin) ||
            (wasteType == "metal" && bin == metalBin) ||
            (wasteType == "papel" && bin == paperBin))
        {
            return true;
        }

        return false;
    }

    // Método auxiliar para determinar o tipo do lixo
// Método auxiliar para determinar o tipo do lixo
public string GetWasteType(GameObject waste) // Alterado de 'private' para 'public'
{
    if (waste.name.Contains("-"))
    {
        string[] nameParts = waste.name.Split('-');
        if (nameParts.Length > 1)
        {
            return nameParts[1].Trim().ToLower(); // Retorna o tipo do lixo em letras minúsculas
        }
    }

    Debug.LogWarning($"Nome do lixo '{waste.name}' não segue o formato esperado.");
    return "unknown";
}



    // Sobrecarga para determinar o tipo do lixo baseado em seu nome
    private string GetWasteType(string wasteName)
    {
        if (wasteName.Contains("-"))
        {
            return wasteName.Split('-')[1].ToLower();
        }
        return "unknown";
    }
}
