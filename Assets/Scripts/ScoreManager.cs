using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[] scores = new int[4];

    public void AddPoints(int playerNumber, int points)
    {
        scores[playerNumber - 1] += points;
        Debug.Log($"Jogador {playerNumber} agora tem {scores[playerNumber - 1]} ponto(s).");
    }

    public int GetPlayerScore(int playerNumber)
    {
        return scores[playerNumber - 1];
    }
}
