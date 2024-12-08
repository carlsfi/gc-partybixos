using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[] playerScores = new int[4]; // Suporte para 4 jogadores

    public void AddPoints(int playerNumber, int points)
    {
        if (playerNumber > 0 && playerNumber <= playerScores.Length)
        {
            playerScores[playerNumber - 1] += points;
            Debug.Log($"Jogador {playerNumber} marcou {points} ponto(s)! Total: {playerScores[playerNumber - 1]} ponto(s).");
        }
        else
        {
            Debug.LogError("Número do jogador inválido.");
        }
    }

    public int GetPlayerScore(int playerNumber)
    {
        if (playerNumber > 0 && playerNumber <= playerScores.Length)
        {
            return playerScores[playerNumber - 1];
        }
        Debug.LogError("Número do jogador inválido.");
        return 0;
    }
}
