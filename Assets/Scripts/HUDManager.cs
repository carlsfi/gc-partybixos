using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public List<Image> playerImages; // Lista de imagens no HUD
    public TurnManager turnManager; // Referência ao TurnManager
    public CanvasGroup hudCanvasGroup; // Controle de visibilidade do HUD (adicionado)

    private void Start()
    {
        // Certifique-se de atualizar o HUD após a ordem ser definida
        turnManager.OnTurnOrderDefined += UpdateHUD;

        // Esconde o HUD no início
        SetHUDVisibility(false);
    }

    private void OnDestroy()
    {
        // Remova o evento para evitar erros de referência
        turnManager.OnTurnOrderDefined -= UpdateHUD;
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
}
