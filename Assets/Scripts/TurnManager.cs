using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public event Action<List<GameObject>> OnTurnOrderDefined; // Evento para notificar o HUD
    public List<GameObject> players; // Lista dos jogadores
    public DiceRoll dice; // Referência ao script do dado
    public MessageManager messageManager; // Referência ao MessageManager
    private List<GameObject> turnOrder = new List<GameObject>(); // Ordem de turnos

    // Lista de todos os minigames
    private string[] allMinigames = { "Quiz Sky", "Cata Lixo", "Leva Caixa", "Corrida Animal", "Batata Quente" };
    // Lista de minigames disponíveis com cenas
    private string[] availableMinigames = {"QuizSky(MG)", "BatataQuente(MG)"};

    private void Start()
    {
        GameData.InitializeGameData(players.Count); // Inicializa dados persistentes
        RestoreGameState(); // Restaura o estado do jogo

        if (GameData.turnOrder.Count == 0)
        {
            StartCoroutine(InitializeTurnOrder());
        }
        else
        {
            // Restaura a ordem dos turnos já definida
            GameData.lastQuizSkyWinnersIndexes.ForEach(winnerIndex =>
            {
                CrownManager.IncrementCrown(winnerIndex);
            });
            turnOrder = GameData.turnOrder.Select(index => players[index]).ToList();
            StartCoroutine(HandleTurn());
        }
    }

    private IEnumerator InitializeTurnOrder()
    {
        Dictionary<GameObject, int> playerRolls = new Dictionary<GameObject, int>();

        foreach (var player in players)
        {
            messageManager.ShowMessage($"É a vez do jogador {player.name}!\nRole o dado!");
            yield return StartCoroutine(WaitForPlayerRoll(player));
            playerRolls.Add(player, dice.diceValue);
            messageManager.ShowMessage($"Jogador {player.name} tirou: {dice.diceValue}");
        }

        // Define a ordem dos turnos
        turnOrder = playerRolls.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        GameData.turnOrder = turnOrder.Select(player => players.IndexOf(player)).ToList(); // Salva no GameData

        yield return StartCoroutine(ShowTurnOrder());
        StartCoroutine(HandleTurn());
        OnTurnOrderDefined?.Invoke(turnOrder);
    }

    private IEnumerator ShowTurnOrder()
    {
        string orderMessage = "Ordem dos turnos:\n";
        for (int i = 0; i < turnOrder.Count; i++)
        {
            orderMessage += $"{i + 1}º: {turnOrder[i].name}\n";
            messageManager.ShowMessage(orderMessage);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator HandleTurn()
    {
        while (true)
        {
            GameObject currentPlayer = turnOrder[GameData.currentPlayerIndex];
            messageManager.ShowMessage($"Turno do jogador {currentPlayer.name}!\nRole o dado!");

            yield return StartCoroutine(WaitForPlayerRoll(currentPlayer));

            int steps = dice.diceValue;
            messageManager.ShowMessage($"{currentPlayer.name} tirou {steps}!");

            PlayerMovement movement = currentPlayer.GetComponent<PlayerMovement>();
            yield return StartCoroutine(movement.MovePlayer(steps));

            // Atualiza a posição do jogador
            GameData.playerPositions[GameData.currentPlayerIndex] = movement.currentWaypointIndex;

            // Próximo jogador
            GameData.currentPlayerIndex = (GameData.currentPlayerIndex + 1) % players.Count;

            if (GameData.currentPlayerIndex == 0)
            {
                yield return StartCoroutine(EndRound());
            }
        }
    }

    private IEnumerator WaitForPlayerRoll(GameObject player)
    {
        bool hasRolled = false;

        dice.OnRollComplete = () =>
        {
            hasRolled = true;
        };

        while (!hasRolled)
        {
            yield return null;
        }
    }

    private IEnumerator EndRound()
    {
        messageManager.ShowMessage("Fim da rodada!");
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(ShowMinigameRoulette());
    }

    private IEnumerator ShowMinigameRoulette()
    {
        float totalDuration = 4f; // Duração total da animação
        float currentInterval = 0.05f; // Começa com intervalos rápidos
        float slowDownRate = 1.5f; // Fator de desaceleração
        string selectedMinigame = null;

        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            // Gira aleatoriamente entre os nomes dos minigames
            string currentMinigame = allMinigames[UnityEngine.Random.Range(0, allMinigames.Length)];
            messageManager.ShowMessage($"Minigame: {currentMinigame}");

            yield return new WaitForSeconds(currentInterval);
            elapsedTime += currentInterval;

            // Aumenta o intervalo para desacelerar a roleta
            currentInterval *= slowDownRate;
        }

        // Seleciona um minigame disponível
        selectedMinigame = availableMinigames[
            UnityEngine.Random.Range(0, availableMinigames.Length)
        ];
        messageManager.ShowMessage($"Minigame Selecionado: {selectedMinigame}");

        yield return new WaitForSeconds(2);

        SaveGameState();
        SceneManager.LoadScene(selectedMinigame);
    }

    private void SaveGameState()
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerMovement movement = players[i].GetComponent<PlayerMovement>();
            GameData.playerPositions[i] = movement.currentWaypointIndex;

            // Salva o sprite do jogador
            Player playerData = players[i].GetComponent<Player>();
            if (GameData.playerSprites.Count <= i)
            {
                GameData.playerSprites.Add(playerData.playerSprite);
            }
            else
            {
                GameData.playerSprites[i] = playerData.playerSprite;
            }
        }

        GameData.isHUDVisible = true;
    }

private void RestoreGameState()
{
    for (int i = 0; i < players.Count; i++)
    {
        PlayerMovement movement = players[i].GetComponent<PlayerMovement>();
        movement.currentWaypointIndex = GameData.playerPositions[i];
        movement.transform.position = movement
            .waypoints[movement.currentWaypointIndex]
            .position;

        // Restaura o sprite do jogador
        Player playerData = players[i].GetComponent<Player>();
        if (i < GameData.playerSprites.Count)
        {
            playerData.playerSprite = GameData.playerSprites[i];
        }
    }

    // Atualiza as pontuações no HUD
    var hudManager = FindObjectOfType<HUDManager>();
    for (int i = 0; i < GameData.playerScores.Count; i++)
    {
        string playerName = players[i].name;
        int playerScore = GameData.playerScores[i];
        hudManager.UpdateScore(playerName, playerScore); // Atualiza o ScoreText do HUD
    }

    // Verifica os vencedores do último minigame e adiciona pontos
    if (GameData.miniGameWinners != null && GameData.miniGameWinners.Count > 0)
    {
        foreach (int winnerIndex in GameData.miniGameWinners)
        {
            // Incrementa a pontuação do vencedor
            GameData.playerScores[winnerIndex]++;
            hudManager.UpdateScore(players[winnerIndex].name, GameData.playerScores[winnerIndex]); // Atualiza no HUD
            
            // Incrementa a coroa do vencedor
            CrownManager.IncrementCrown(winnerIndex);
        }

        // Reseta a lista de vencedores após contabilizar os pontos
        GameData.miniGameWinners.Clear();
    }

    if (GameData.isHUDVisible && turnOrder.Count > 0)
    {
        hudManager.UpdateHUD(turnOrder);
    }
}

}
