using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance; // Singleton para facilitar acesso
    public float roundDuration = 30f; // Duração de cada rodada
    private float currentTimer;

    public QuestionDisplay questionDisplay; // Exibição de perguntas
    public Questions questions; // Banco de perguntas
    public TextMeshProUGUI timerText; // Exibição do temporizador

    public List<Hexagon> hexagons; // Lista de hexágonos na cena
    private Question currentQuestion;

    private Dictionary<GameObject, int> playerChoices = new Dictionary<GameObject, int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTimer = roundDuration;
        StartNewRound();
    }

    void Update()
    {
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndRound();
        }
    }

    void StartNewRound()
    {
        currentTimer = roundDuration;
        currentQuestion = questions.GetRandomUnansweredQuestion();

        if (currentQuestion != null)
        {
            questionDisplay.DisplayQuestion();
            DistributeAnswersToHexagons();
            playerChoices.Clear(); // Limpa as escolhas dos jogadores
        }
        else
        {
            Debug.Log("Sem mais perguntas! Jogo finalizado.");
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Tempo: " + Mathf.CeilToInt(currentTimer) + "s";
    }

    void EndRound()
    {
        Debug.Log("Rodada encerrada!");
        CheckPlayersOnCorrectHexagon();
        ResetHexagons();

        // Inicia nova rodada após um pequeno delay
        Invoke("StartNewRound", 2f);
    }

    void DistributeAnswersToHexagons()
{
    // Lista de índices das respostas (0, 1, 2, 3 para A, B, C, D)
    List<int> indexes = new List<int> { 0, 1, 2, 3 };

    // Gera uma lista embaralhada de índices, suficiente para cobrir todos os hexágonos
    List<int> randomizedIndexes = new List<int>();
    while (randomizedIndexes.Count < hexagons.Count)
    {
        // Embaralha os índices e adiciona na lista
        indexes.Sort((a, b) => Random.Range(-1, 2));
        randomizedIndexes.AddRange(indexes);
    }

    // Distribui os textos nos hexágonos
    for (int i = 0; i < hexagons.Count; i++)
    {
        int answerIndex = randomizedIndexes[i % randomizedIndexes.Count]; // Índice da resposta
        string letter = GetLetterForIndex(answerIndex); // Converte para A, B, C, D
        hexagons[i].Initialize(answerIndex, letter);
    }
}

// Função auxiliar para mapear índice (0, 1, 2, 3) para letras (A, B, C, D)
string GetLetterForIndex(int index)
{
    switch (index)
    {
        case 0: return "A";
        case 1: return "B";
        case 2: return "C";
        case 3: return "D";
        default: return "";
    }
}


    public void PlayerOnHexagon(GameObject player, int chosenAnswer)
    {
        if (!playerChoices.ContainsKey(player))
        {
            playerChoices[player] = chosenAnswer; // Salva a escolha do jogador
        }
    }

    void CheckPlayersOnCorrectHexagon()
    {
        foreach (var entry in playerChoices)
        {
            GameObject player = entry.Key;
            int chosenAnswer = entry.Value;

            if (chosenAnswer == currentQuestion.Answer)
            {
                Debug.Log($"Jogador {player.name} acertou a resposta!");
                // Aqui você pode adicionar lógica de pontuação
            }
            else
            {
                Debug.Log($"Jogador {player.name} errou a resposta!");
                // Lógica para eliminar jogador ou penalizar
            }
        }
    }

    void ResetHexagons()
    {
        foreach (var hexagon in hexagons)
        {
            hexagon.ResetHexagon();
        }
    }
}
