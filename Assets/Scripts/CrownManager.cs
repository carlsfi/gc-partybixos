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

    [SerializeField]
    private TMP_Text winnerText; // Campo para exibir o vencedor (configurar no Inspector)

    void Awake()
    {
        // Inicializa os campos estáticos com os objetos configurados no Inspector
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

        // Verifica se o jogador atingiu o limite de coroas
        CheckForWinner(playerIndex);
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
        // Retorna o TextMeshProUGUI correspondente ao índice
        return index switch
        {
            0 => crownValuePlayerOne,
            1 => crownValuePlayerTwo,
            2 => crownValuePlayerThree,
            3 => crownValuePlayerFour,
            _ => null
        };
    }

    private static void CheckForWinner(int playerIndex)
    {
        if (crownsList[playerIndex] >= 5)
        {
            Debug.Log($"Jogador {playerIndex + 1} venceu o jogo com 5 coroas!");

            // Exibe o vencedor (opcional, configurar `winnerText` no Inspector)
            CrownManager instance = FindObjectOfType<CrownManager>();
            if (instance != null && instance.winnerText != null)
            {
                instance.winnerText.text = $"Jogador {playerIndex + 1} venceu o jogo!";
            }

            // Finaliza o jogo e carrega a cena de fim
            EndGame();
        }
    }

    private static void EndGame()
    {
        Debug.Log("Finalizando o jogo...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
