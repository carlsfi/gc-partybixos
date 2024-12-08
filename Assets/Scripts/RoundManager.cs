using System.Collections.Generic;
using System.Collections;
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

    private Dictionary<GameObject, bool> playerStatus = new Dictionary<GameObject, bool>(); // Controle de status de jogadores
    private List<GameObject> fallenPlayers = new List<GameObject>(); // Lista de jogadores que caíram

    public float resetDelay = 5f; // Tempo entre o final da rodada e o início da próxima
    private DeathZone deathZone; // Referência para o DeathZone

    private int currentRound = 1; // Contador de rodadas
    private bool isProcessingRound = false; // Controle para evitar finalizar antes de processar tudo
    [SerializeField] 
    private HUDManager hudManager; // Referência do HUDManager no Inspector
    [SerializeField] private int winningScore = 2; // Pontuação para vencer o minigame
    [SerializeField] private string boardSceneName = "Tabuleiro"; // Nome da cena do tabuleiro



    void Awake()
    {
        Instance = this;
        if (hudManager == null)
    {
        Debug.LogError("HUDManager não foi configurado no Inspector!");
    }

    }

    void Start()
    {
        currentTimer = roundDuration;
        deathZone = FindObjectOfType<DeathZone>(); // Localiza o DeathZone na cena
        StartNewRound();
    }

    void Update()
    {
        if (!isProcessingRound)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                isProcessingRound = true; // Bloqueia novas ações até o processamento terminar
                StartCoroutine(ProcessHexagonDisappearance());
            }
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Tempo: " + Mathf.CeilToInt(currentTimer) + " segundos";
    }

    

    void StartNewRound()
    {
        isProcessingRound = false; // Reseta o estado de processamento
        currentTimer = roundDuration;
        currentQuestion = questions.GetRandomUnansweredQuestion();

        Debug.Log($"Iniciando rodada {currentRound}!");

        // Reativa todos os hexágonos antes de começar uma nova rodada
        foreach (var hexagon in hexagons)
        {
            hexagon.ShowHexagon(); // Mostra todos os hexágonos novamente
        }

        if (currentQuestion != null)
        {
            // Passa a pergunta para o QuestionDisplay exibir
            questionDisplay.DisplayQuestion(currentQuestion);
            DistributeAnswersToHexagons();
            playerStatus.Clear(); // Reseta o status dos jogadores

            // Reposiciona jogadores caídos
            if (deathZone != null)
            {
                deathZone.RespawnAllPlayers();
            }
        }
        else
        {
            Debug.Log("Sem mais perguntas! Jogo finalizado.");
        }
    }

    IEnumerator ProcessHexagonDisappearance()
    {
        Debug.Log("Escondendo hexágonos errados...");

        // Esconde os hexágonos errados
        foreach (var hexagon in hexagons)
        {
            if (hexagon.answerIndex != currentQuestion.Answer)
            {
                hexagon.HideHexagon();
            }
        }

        // Aguarda para permitir que os jogadores caiam
        yield return new WaitForSeconds(4f);

        ProcessFallenPlayers(); // Processa os jogadores após a queda
    }

    void ProcessFallenPlayers()
    {
        Debug.Log("Processando jogadores que caíram na zona de morte...");

        foreach (var player in fallenPlayers)
        {
            if (playerStatus.ContainsKey(player))
            {
                playerStatus[player] = false; // Atualiza o status como caído
            }
        }

        fallenPlayers.Clear(); // Limpa a lista após o processamento
        EndRound();
    }

    void EndRound()
{
    Debug.Log($"Rodada {currentRound} encerrada!");

    if (currentQuestion != null)
    {
        Debug.Log($"A resposta correta é: {currentQuestion.Answer}");
    }
    else
    {
        Debug.LogError("currentQuestion é null. Não foi possível determinar a resposta correta.");
        return; // Evita erros se currentQuestion estiver null
    }

    foreach (var entry in playerStatus)
    {
        GameObject player = entry.Key;
        bool isSafe = entry.Value;

        if (isSafe)
        {
            Debug.Log($"Rodada {currentRound}: Jogador {player.name} ganhou um ponto por permanecer no hexágono!");
            if (hudManager != null)
            {
                hudManager.UpdateScore(player.name, hudManager.GetPlayerScore(player.name) + 1);
            }
            else
            {
                Debug.LogError("HUDManager não está configurado!");
            }
        }
        else
        {
            Debug.Log($"Rodada {currentRound}: Jogador {player.name} caiu e não ganhou pontos!");
        }
    }

    CheckForWinner(); // Verifica se há um vencedor

    currentRound++; // Incrementa o contador de rodadas

    StartCoroutine(ResetRoundWithDelay());
}


    IEnumerator<WaitForSeconds> ResetRoundWithDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        StartNewRound();
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
        if (!playerStatus.ContainsKey(player))
        {
            playerStatus[player] = true; // Marca como seguro
        }
    }

    public void PlayerEnteredHexagon(GameObject player)
    {
        if (!playerStatus.ContainsKey(player))
        {
            playerStatus[player] = true; // Marca como seguro
        }
    }

    public void PlayerLeftHexagon(GameObject player)
    {
        if (playerStatus.ContainsKey(player))
        {
            playerStatus[player] = false; // Marca como caído
            if (!fallenPlayers.Contains(player))
            {
                fallenPlayers.Add(player); // Adiciona à lista de caídos
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

    void CheckForWinner()
{
    foreach (var playerName in hudManager.GetPlayerNames())
    {
        int playerScore = hudManager.GetPlayerScore(playerName);

        if (playerScore >= winningScore)
        {
            Debug.Log($"Jogador {playerName} venceu o minigame com {playerScore} pontos!");
            EndMinigame();
            return;
        }
    }
}

    void EndMinigame()
    {
        Debug.Log("Encerrando o minigame e voltando para o tabuleiro...");
        UnityEngine.SceneManagement.SceneManager.LoadScene(boardSceneName);
    }

}
