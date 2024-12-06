using UnityEngine;
using System.Collections.Generic;

public class WasteManager : MonoBehaviour
{
    public List<GameObject> plasticWaste = new List<GameObject>();
    public List<GameObject> glassWaste = new List<GameObject>();
    public List<GameObject> metalWaste = new List<GameObject>();
    public List<GameObject> paperWaste = new List<GameObject>();

    public Transform plasticBin;
    public Transform glassBin;
    public Transform metalBin;
    public Transform paperBin;

    public ScoreManager scoreManager;

    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    public int numberOfWastesToSpawn = 10;

    private List<GameObject> allWastePrefabs = new List<GameObject>();

    void Start()
    {
        ConsolidateWastePrefabs();
        SpawnWastes();
    }

    void ConsolidateWastePrefabs()
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

    void SpawnWastes()
    {
        if (allWastePrefabs.Count == 0)
        {
            Debug.LogError("Nenhum prefab disponível para spawnar.");
            return;
        }

        for (int i = 0; i < numberOfWastesToSpawn; i++)
        {
            GameObject wastePrefab = allWastePrefabs[Random.Range(0, allWastePrefabs.Count)];
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );
            GameObject spawnedWaste = Instantiate(wastePrefab, randomPosition, Quaternion.identity);
            spawnedWaste.tag = "Lixo";
        }
    }

    public bool IsWasteInCorrectBin(GameObject waste, Transform bin)
    {
        string wasteType = GetWasteType(waste);

        if ((wasteType == "plastico" && bin == plasticBin) ||
            (wasteType == "vidro" && bin == glassBin) ||
            (wasteType == "metal" && bin == metalBin) ||
            (wasteType == "papel" && bin == paperBin))
        {
            return true;
        }

        return false;
    }

    public void DepositWaste(GameObject waste, Transform bin, int playerNumber)
    {
        if (IsWasteInCorrectBin(waste, bin))
        {
            Debug.Log($"Jogador {playerNumber} depositou o lixo {waste.name} corretamente!");
            scoreManager.AddPoints(playerNumber, 1); // Adiciona 1 ponto ao jogador
            Destroy(waste); // Remove o lixo da cena
        }
        else
        {
            Debug.Log($"Jogador {playerNumber} depositou o lixo {waste.name} na lixeira errada!");
            Destroy(waste); // Remove o lixo, mas sem pontuar
        }
    }

    private string GetWasteType(GameObject waste)
    {
        if (waste.name.Contains("-"))
        {
            string[] nameParts = waste.name.Split('-');
            if (nameParts.Length > 1)
            {
                return nameParts[1].Trim().ToLower();
            }
        }

        Debug.LogWarning($"Nome do lixo '{waste.name}' não segue o formato esperado.");
        return "unknown";
    }
}
