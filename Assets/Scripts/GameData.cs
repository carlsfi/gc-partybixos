using System.Collections.Generic;
using UnityEngine;
public static class GameData
{
    public static List<int> playerPositions = new List<int>(); // Índices dos jogadores no tabuleiro
    public static List<int> playerScores = new List<int>(); // Pontuações dos jogadores
    public static int currentPlayerIndex = 0; // Índice do jogador atual no turno
    public static List<int> turnOrder = new List<int>(); // Ordem dos turnos (índices dos jogadores)
    public static bool isGameInitialized = false; // Verifica se os dados foram inicializados
    public static bool isHUDVisible = false; // Indica se o HUD deve estar visível
    public static List<Sprite> playerSprites = new List<Sprite>(); // Sprites dos jogadores
    public static int lastMinigameWinner = -1; // Índice do vencedor do último minigame




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
    }
}
