using UnityEngine;
using TMPro;

public class Hexagon : MonoBehaviour
{
    public int answerIndex; // Índice associado à resposta deste hexágono
    public TextMeshPro answerText; // Referência ao texto 3D no hexágono
    private bool isActive = false; // Define se o hexágono está ativo

    // Inicializa o hexágono com a resposta (letra)
    public void Initialize(int index, string text)
    {
        answerIndex = index;
        answerText.text = text; // Exibe apenas a letra (A, B, C, D)
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"Jogador {other.name} entrou no hexágono com resposta: {answerText.text}");
            RoundManager.Instance.PlayerOnHexagon(other.gameObject, answerIndex);
        }
    }

    public void ResetHexagon()
    {
        isActive = false;
        answerText.text = ""; // Remove o texto do hexágono
    }
}
