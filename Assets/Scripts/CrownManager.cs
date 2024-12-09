using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrownManager : MonoBehaviour
{
    private static TextMeshProUGUI crownValuePlayerOne;
    private static TextMeshProUGUI crownValuePlayerTwo;
    private static TextMeshProUGUI crownValuePlayerThree;
    private static TextMeshProUGUI crownValuePlayerFour;

    public static List<int> crownsList = new() { 0, 0, 0, 0 }; // Armazena os valores das coroas

    [SerializeField]
    private TextMeshProUGUI crownOne;

    [SerializeField]
    private TextMeshProUGUI crownTwo;

    [SerializeField]
    private TextMeshProUGUI crownThree;

    [SerializeField]
    private TextMeshProUGUI crownFour;

    void Awake()
    {
        // Inicializa os campos est�ticos com os objetos configurados no Inspector
        crownValuePlayerOne = crownOne;
        crownValuePlayerTwo = crownTwo;
        crownValuePlayerThree = crownThree;
        crownValuePlayerFour = crownFour;

        // Atualiza os textos com os valores iniciais
        UpdateAllCrownDisplays();
    }

    public static void IncrementCrown(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= crownsList.Count)
        {
            Debug.LogError($"Índice do jogador {playerIndex} está fora do intervalo.");
            return;
        }

        // Incrementa o valor no armazenamento
        crownsList[playerIndex]++;

        // Atualiza o texto correspondente
        UpdateCrownDisplay(playerIndex);
    }

    private static void UpdateCrownDisplay(int playerIndex)
    {
        TextMeshProUGUI targetCrown = GetCrownByIndex(playerIndex);

        if (targetCrown != null)
        {
            // Atualiza o texto do TextMeshProUGUI com o valor armazenado
            targetCrown.text = crownsList[playerIndex].ToString();
        }
        else
        {
            Debug.LogWarning($"Jogador com índice {playerIndex} não encontrado para atualização.");
        }
    }

    private static void UpdateAllCrownDisplays()
    {
        // Atualiza todos os textos com os valores armazenados
        for (int i = 0; i < crownsList.Count; i++)
        {
            UpdateCrownDisplay(i);
        }
    }

    private static TextMeshProUGUI GetCrownByIndex(int index)
    {
        // Retorna o TextMeshProUGUI correspondente ao �ndice
        return index switch
        {
            0 => crownValuePlayerOne,
            1 => crownValuePlayerTwo,
            2 => crownValuePlayerThree,
            3 => crownValuePlayerFour,
            _ => null
        };
    }
}
