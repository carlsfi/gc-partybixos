using UnityEngine;
using System.Collections.Generic;

public class WasteManager : MonoBehaviour
{
    // Listas para diferentes tipos de lixo
    public List<GameObject> plasticWaste = new List<GameObject>();
    public List<GameObject> glassWaste = new List<GameObject>();
    public List<GameObject> metalWaste = new List<GameObject>();
    public List<GameObject> paperWaste = new List<GameObject>();

    // Lixeiras correspondentes
    public Transform plasticBin;
    public Transform glassBin;
    public Transform metalBin;
    public Transform paperBin;

    // Área para spawnar lixos
    public Vector3 spawnAreaMin; // Coordenadas mínimas da área
    public Vector3 spawnAreaMax; // Coordenadas máximas da área

    public GameObject[] wastePrefabs; // Prefabs dos tipos de lixo
    public int numberOfWastesToSpawn = 10; // Quantidade de lixos a spawnar

    void Start()
    {
        // Organiza os lixos existentes na cena
        OrganizeWaste();

        // Spawn inicial dos lixos
        SpawnWastes();
    }

    // Organiza os lixos em listas baseadas na tag
    void OrganizeWaste()
    {
        foreach (GameObject waste in GameObject.FindGameObjectsWithTag("Lixo"))
        {
            AddWasteToList(waste);
        }
    }

    // Adiciona lixo à lista correspondente
    private void AddWasteToList(GameObject waste)
    {
        string wasteType = GetWasteType(waste);

        switch (wasteType)
        {
            case "plastico":
                plasticWaste.Add(waste);
                break;
            case "vidro":
                glassWaste.Add(waste);
                break;
            case "metal":
                metalWaste.Add(waste);
                break;
            case "papel":
                paperWaste.Add(waste);
                break;
            default:
                Debug.LogWarning($"Tipo de lixo desconhecido: {waste.name}");
                break;
        }
    }

    // Retorna o tipo de lixo a partir do nome do objeto
    private string GetWasteType(GameObject waste)
    {
        return waste.name.Split('-')[1].Trim(); // Supõe que o nome segue o padrão: "Objeto-tipo"
    }

    // Spawna lixos em posições aleatórias dentro da área definida
    void SpawnWastes()
    {
        for (int i = 0; i < numberOfWastesToSpawn; i++)
        {
            // Escolhe um prefab de lixo aleatório
            GameObject wastePrefab = wastePrefabs[Random.Range(0, wastePrefabs.Length)];

            // Gera uma posição aleatória dentro da área definida
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // Instancia o lixo na posição gerada
            GameObject spawnedWaste = Instantiate(wastePrefab, randomPosition, Quaternion.identity);
            spawnedWaste.tag = "Lixo"; // Garante que o objeto tenha a tag "Lixo"

            // Adiciona o lixo à lista correspondente
            AddWasteToList(spawnedWaste);
        }
    }

    // Verifica se o lixo foi depositado na lixeira correta
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

    // Método para manipular o evento de lixo depositado
    public void DepositWaste(GameObject waste, Transform bin)
    {
        if (IsWasteInCorrectBin(waste, bin))
        {
            Debug.Log($"Lixo {waste.name} depositado na lixeira correta!");
        }
        else
        {
            Debug.Log($"Lixo {waste.name} depositado na lixeira errada!");
        }

        // Remove o lixo do mundo ao depositar
        Destroy(waste);
    }
}
