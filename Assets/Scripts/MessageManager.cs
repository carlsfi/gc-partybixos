using System.Collections;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Texto da mensagem
    public GameObject messagePanel; // Painel da mensagem
    public float messageDuration = 3f; // Duração de exibição da mensagem
    public Vector3 bounceEffect = new Vector3(1.2f, 1.2f, 1f); // Efeito de "pulo" do texto

    private Coroutine currentMessageRoutine;

    private void Start()
    {
        messagePanel.SetActive(false); // Esconde o painel inicialmente
    }

    // Exibe a mensagem com estilo
    public void ShowMessage(string message)
    {
        if (currentMessageRoutine != null)
        {
            StopCoroutine(currentMessageRoutine); // Para mensagens anteriores
        }

        currentMessageRoutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        messagePanel.SetActive(true); // Mostra o painel
        messageText.text = message; // Define o texto

        // Animação de "entrada" com efeito de pulo
        messagePanel.transform.localScale = Vector3.zero;
        LeanTween.scale(messagePanel, bounceEffect, 0.3f).setEaseOutBounce();

        yield return new WaitForSeconds(messageDuration); // Aguarda a exibição

        // Animação de "saída" (desaparecendo)
        LeanTween.scale(messagePanel, Vector3.zero, 0.3f).setEaseInBack();
        yield return new WaitForSeconds(0.3f);

        messagePanel.SetActive(false); // Esconde o painel
        currentMessageRoutine = null;
    }
}