using UnityEngine;
using TMPro;

public class Hexagon : MonoBehaviour
{
    public int answerIndex; // Índice associado à resposta deste hexágono
    public TextMeshPro answerText; // Referência ao texto exibido no hexágono
    private bool isActive = false; // Define se o hexágono está ativo
    private Renderer hexagonRenderer; // Renderer do hexágono para controlar visibilidade
    private Collider hexagonCollider;

    void Awake()
    {
        hexagonRenderer = GetComponent<Renderer>();
        hexagonCollider = GetComponent<Collider>();
    }

    public void Initialize(int index, string text)
    {
        answerIndex = index;
        answerText.text = text; // Define o texto (A, B, C, D)
        isActive = true;
        ShowHexagon(); // Certifica-se de que o hexágono está visível
    }

    private void OnCollisionEnter(Collision collision)
    {
    if (collision.gameObject.CompareTag("Player"))
    {
        //Debug.Log($"Jogador {collision.gameObject.name} colidiu com o hexágono!");
        RoundManager.Instance.PlayerOnHexagon(collision.gameObject, answerIndex);
    }
    }


    public void ResetHexagon()
    {
        isActive = false;
        answerText.text = ""; // Remove o texto do hexágono
        ShowHexagon(); // Certifica-se de que o hexágono reaparece
    }

    public void HideHexagon()
    {
        isActive = false;
        hexagonRenderer.enabled = false; // Desativa visualmente o hexágono
        hexagonCollider.enabled = false;
        answerText.gameObject.SetActive(false); // Esconde o texto
    }

    public void ShowHexagon()
    {
        isActive = true;
        hexagonRenderer.enabled = true; // Reativa visualmente o hexágono
        hexagonCollider.enabled = true;
        answerText.gameObject.SetActive(true); // Mostra o texto
    }
}
