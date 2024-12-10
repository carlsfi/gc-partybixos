using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static List<int> playerPositions = new List<int>(); // Índices dos jogadores no tabuleiro
    public static List<int> playerScores; // Pontuação dos jogadores no tabuleiro
    public static List<int> miniGameWinners = new List<int>(); // Índices dos vencedores do último minigame
    public static int currentPlayerIndex = 0; // Índice do jogador atual no turno
    public static List<int> turnOrder = new List<int>(); // Ordem dos turnos (índices dos jogadores)
    public static bool isGameInitialized = false; // Verifica se os dados foram inicializados
    public static bool isHUDVisible = false; // Indica se o HUD deve estar visível
    public static List<Sprite> playerSprites = new List<Sprite>(); // Sprites dos jogadores
    public static int lastMinigameWinner = -1; // Índice do vencedor do último minigame
    public static List<int> lastQuizSkyWinnersIndexes = new(); // Índices dos vencedores do Quiz Sky

    // Método para inicializar os dados do jogo
    public static void InitializeGameData(int playerCount)
    {
        if (!isGameInitialized)
        {
            playerPositions = new List<int>(new int[playerCount]); // Inicializa posições com 0
            playerScores = new List<int>(new int[playerCount]); // Inicializa pontuações com 0
            turnOrder = new List<int>(); // Inicializa a ordem dos turnos
            currentPlayerIndex = 0;
            isGameInitialized = true;
        }

        // Garante que miniGameWinners está inicializado
        if (miniGameWinners == null)
        {
            miniGameWinners = new List<int>();
        }

        // Garante que playerSprites está sincronizado com o número de jogadores
        if (playerSprites.Count != playerCount)
        {
            playerSprites = new List<Sprite>(new Sprite[playerCount]);
        }

        // Garante que lastQuizSkyWinnersIndexes está inicializado
        if (lastQuizSkyWinnersIndexes == null)
        {
            lastQuizSkyWinnersIndexes = new List<int>();
        }
    }
}
