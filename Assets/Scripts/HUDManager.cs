using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (turnManager != null)
        {
            turnManager.OnTurnOrderDefined += UpdateHUD;
        }

        // Configura o HUD com base no estado persistente
        SetHUDVisibility(GameData.isHUDVisible);
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

                if (playerData.playerSprite != null)
                {
                    Debug.Log($"Setting sprite for player {playerData.name}");
                    playerImages[i].sprite = playerData.playerSprite;
                    playerImages[i].gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Player {playerData.name} has no sprite set!");
                }
            }
            else
            {
                playerImages[i].gameObject.SetActive(false);
            }
        }

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
