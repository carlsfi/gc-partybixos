using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public List<Image> playerImages; // Lista de imagens no HUD
    public TurnManager turnManager; // Referência ao TurnManager
    public CanvasGroup hudCanvasGroup; // Controle de visibilidade do HUD (adicionado)

    [System.Serializable]
    public class PlayerHUD
    {
        public Image profileImage; // Imagem de perfil
        public TextMeshProUGUI scoreText; // Texto de pontuação
    }

    public List<PlayerHUD> leftPlayers; // Jogadores do lado esquerdo (QuizSky)
    public List<PlayerHUD> rightPlayers; // Jogadores do lado direito (QuizSky)

    private Dictionary<string, int> playerScores = new Dictionary<string, int>(); // Pontuação dos jogadores

    private void Start()
    {
        // Certifique-se de atualizar o HUD após a ordem ser definida
        if (turnManager != null)
        {
            turnManager.OnTurnOrderDefined += UpdateHUD;
        }

        // Esconde o HUD no início
        SetHUDVisibility(false);
    }

    private void OnDestroy()
    {
        // Remova o evento para evitar erros de referência
        if (turnManager != null)
        {
            turnManager.OnTurnOrderDefined -= UpdateHUD;
        }
    }

    public void UpdateHUD(List<GameObject> turnOrder)
    {
        for (int i = 0; i < playerImages.Count; i++)
        {
            if (i < turnOrder.Count)
            {
                Player playerData = turnOrder[i].GetComponent<Player>();
                playerImages[i].sprite = playerData.playerSprite; // Atualiza o sprite do jogador
                playerImages[i].gameObject.SetActive(true); // Certifique-se de que está ativo
            }
            else
            {
                playerImages[i].gameObject.SetActive(false); // Esconde imagens extras
            }
        }

        // Mostra o HUD após atualizar a ordem
        SetHUDVisibility(true);
    }

    private void SetHUDVisibility(bool isVisible)
    {
        if (hudCanvasGroup != null)
        {
            hudCanvasGroup.alpha = isVisible ? 1 : 0; // Define opacidade
            hudCanvasGroup.interactable = isVisible; // Ativa/Desativa interações
            hudCanvasGroup.blocksRaycasts = isVisible; // Controla cliques no HUD
        }
    }

    // Atualiza a pontuação de um jogador no QuizSky
    public void UpdateScore(string playerName, int score)
    {
        if (playerScores.ContainsKey(playerName))
        {
            playerScores[playerName] = score;
        }
        else
        {
            playerScores.Add(playerName, score);
        }

        UpdateQuizHUD(playerName);
    }

    // Atualiza o HUD de pontuação no QuizSky
    private void UpdateQuizHUD(string playerName)
    {
        foreach (var playerHUD in leftPlayers)
        {
            if (playerHUD.profileImage.name == playerName)
            {
                playerHUD.scoreText.text = playerScores[playerName].ToString();
                return;
            }
        }

        foreach (var playerHUD in rightPlayers)
        {
            if (playerHUD.profileImage.name == playerName)
            {
                playerHUD.scoreText.text = playerScores[playerName].ToString();
                return;
            }
        }
    }

    // Inicializa os jogadores com pontuação zero no QuizSky
    public void InitializeQuizPlayers(List<string> playerNames)
    {
        foreach (string playerName in playerNames)
        {
            playerScores[playerName] = 0;
            UpdateQuizHUD(playerName);
        }
    }

    public int GetPlayerScore(string playerName)
{
    if (playerScores.ContainsKey(playerName))
    {
        return playerScores[playerName];
    }
    return 0;
}

public List<string> GetPlayerNames()
{
    List<string> playerNames = new List<string>();

    // Adiciona nomes dos jogadores da esquerda
    foreach (var playerHUD in leftPlayers)
    {
        if (playerHUD.profileImage != null)
        {
            playerNames.Add(playerHUD.profileImage.name);
        }
    }

    // Adiciona nomes dos jogadores da direita
    foreach (var playerHUD in rightPlayers)
    {
        if (playerHUD.profileImage != null)
        {
            playerNames.Add(playerHUD.profileImage.name);
        }
    }

    return playerNames;
}


}
