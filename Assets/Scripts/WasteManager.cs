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

    // Referência ao jogador
    public Transform player;
    public Transform holdPoint; // Ponto onde o lixo será carregado

    private GameObject heldWaste = null; // Lixo que o jogador está segurando

    void Start()
    {
        OrganizeWaste();
    }

    // Organiza os lixos em listas baseadas na tag
    void OrganizeWaste()
    {
        foreach (GameObject waste in GameObject.FindGameObjectsWithTag("Lixo"))
        {
            string wasteType = waste.name.Split('-')[1].Trim(); // Obtém o tipo pelo nome do objeto
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
    }

    void Update()
    {
        HandleWastePickup();
        HandleWasteDrop();
    }

    // Permite pegar o lixo ao apertar o botão X do controle Xbox
    void HandleWastePickup()
    {
        if (Input.GetButtonDown("X") && heldWaste == null)
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(player.position, 2f); // Checa objetos próximos
            foreach (var obj in nearbyObjects)
            {
                if (obj.CompareTag("Lixo"))
                {
                    heldWaste = obj.gameObject;
                    heldWaste.transform.SetParent(holdPoint);
                    heldWaste.transform.localPosition = Vector3.zero; // Coloca o lixo na posição correta
                    heldWaste.GetComponent<Collider>().enabled = false; // Desativa o colisor para evitar problemas
                    Debug.Log($"Pegou: {heldWaste.name}");
                    break;
                }
            }
        }
    }

    // Permite soltar o lixo próximo à lixeira correta
    void HandleWasteDrop()
    {
        if (Input.GetButtonDown("X") && heldWaste != null)
        {
            float distanceToPlasticBin = Vector3.Distance(heldWaste.transform.position, plasticBin.position);
            float distanceToGlassBin = Vector3.Distance(heldWaste.transform.position, glassBin.position);
            float distanceToMetalBin = Vector3.Distance(heldWaste.transform.position, metalBin.position);
            float distanceToPaperBin = Vector3.Distance(heldWaste.transform.position, paperBin.position);

            string wasteType = heldWaste.name.Split('-')[1].Trim();

            if (distanceToPlasticBin < 2f && wasteType == "plastico")
            {
                DropWaste(plasticBin);
            }
            else if (distanceToGlassBin < 2f && wasteType == "vidro")
            {
                DropWaste(glassBin);
            }
            else if (distanceToMetalBin < 2f && wasteType == "metal")
            {
                DropWaste(metalBin);
            }
            else if (distanceToPaperBin < 2f && wasteType == "papel")
            {
                DropWaste(paperBin);
            }
            else
            {
                Debug.Log("O lixo não foi colocado na lixeira correta!");
            }
        }
    }

    // Solta o lixo na lixeira correta
    void DropWaste(Transform bin)
    {
        Debug.Log($"Lixo {heldWaste.name} depositado na lixeira correta!");
        heldWaste.transform.SetParent(null);
        heldWaste.GetComponent<Collider>().enabled = true; // Reativa o colisor
        heldWaste = null; // Reseta o objeto segurado
    }
}
