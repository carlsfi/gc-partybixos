using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotatoGameManager : MonoBehaviour
{
    public List<GameObject> players; // Lista de jogadores
    public int totalRounds = 3; // Número total de rounds
    public float initialRoundTime = 30f; // Duração do primeiro round em segundos
    public float timeReductionPerRound = 5f; // Redução do tempo por round
    public float proximityRadius = 2f; // Raio para detectar jogadores próximos

    private GameObject currentHotPlayer; // Jogador com a batata quente
    private float roundTimer;
    private int currentRound = 1;

    private Dictionary<GameObject, int> playerScores = new Dictionary<GameObject, int>(); // Pontos de cada jogador

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            UpdateOutlineIntensity(); // Atualiza o outline do jogador com a batata quente

            if (roundTimer <= 0)
            {
                EndRound();
            }
        }

        // Verifica entrada para ataque
        foreach (var player in players)
        {
            if (Input.GetButtonDown($"P{GetPlayerNumber(player)}_X"))
            {
                PlayAttackAnimation(player); // Sempre executa a animação de ataque
                if (player == currentHotPlayer)
                {
                    TryPassHotPotato(); // Tenta passar a batata se o jogador tiver ela
                }
            }
        }
    }

    private void InitializeGame()
    {
        roundTimer = initialRoundTime;

        foreach (var player in players)
        {
            playerScores[player] = 0; // Inicializa os pontos dos jogadores
        }

        ChooseRandomHotPlayer();
    }

    private void ChooseRandomHotPlayer()
    {
        if (currentHotPlayer != null)
        {
            RemoveOutline(currentHotPlayer); // Remove o outline do jogador anterior
        }

        currentHotPlayer = players[Random.Range(0, players.Count)];
        ApplyOutline(currentHotPlayer); // Aplica outline ao novo jogador
        Debug.Log($"Jogador {currentHotPlayer.name} começou com a batata quente!");
    }

    private void EndRound()
    {
        Debug.Log($"Fim do Round {currentRound}");
        playerScores[currentHotPlayer]--; // Penaliza o jogador que ficou com a batata no final do tempo
        NextRound();
    }

    private void NextRound()
    {
        currentRound++;

        if (currentRound > totalRounds)
        {
            EndGame();
        }
        else
        {
            // Reduz o tempo por round, mas nunca abaixo de 10 segundos
            roundTimer = Mathf.Max(10f, initialRoundTime - (timeReductionPerRound * (currentRound - 1)));
            ChooseRandomHotPlayer();
        }
    }

    private void EndGame()
    {
        Debug.Log("Fim do Minigame! Calculando os resultados...");
        GameObject winner = null;
        int maxScore = int.MinValue;

        foreach (var player in players)
        {
            Debug.Log($"{player.name}: {playerScores[player]} pontos");
            if (playerScores[player] > maxScore)
            {
                maxScore = playerScores[player];
                winner = player;
            }
        }

        Debug.Log($"O vencedor foi {winner.name} com {maxScore} pontos!");
        // Aqui você pode adicionar a lógica para transitar de volta ao tabuleiro ou outro minigame
    }

    private void ApplyOutline(GameObject player)
    {
        Outline outline = player.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true; // Ativa o outline
            outline.OutlineColor = Color.yellow; // Cor inicial
            outline.OutlineWidth = 2f; // Espessura inicial
        }
    }

    private void RemoveOutline(GameObject player)
    {
        Outline outline = player.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Desativa o outline
        }
    }

    private void UpdateOutlineIntensity()
    {
        if (currentHotPlayer != null)
        {
            Outline outline = currentHotPlayer.GetComponent<Outline>();
            if (outline != null)
            {
                // Calcula a intensidade do vermelho baseado no tempo restante
                float intensity = Mathf.Lerp(0f, 1f, 1f - (roundTimer / initialRoundTime));
                outline.OutlineColor = Color.Lerp(Color.yellow, Color.red, intensity); // Transição de amarelo para vermelho
            }
        }
    }

    public void PassHotPotato(GameObject newHotPlayer)
    {
        RemoveOutline(currentHotPlayer); // Remove o outline do jogador atual
        currentHotPlayer = newHotPlayer;
        ApplyOutline(currentHotPlayer); // Adiciona outline ao novo jogador
        Debug.Log($"{currentHotPlayer.name} recebeu a batata quente!");
    }

    public void AddScore(GameObject player, int score)
    {
        if (playerScores.ContainsKey(player))
        {
            playerScores[player] += score;
        }
    }

    private void TryPassHotPotato()
    {
        Collider[] nearbyPlayers = Physics.OverlapSphere(currentHotPlayer.transform.position, proximityRadius, LayerMask.GetMask("Player"));
        foreach (var collider in nearbyPlayers)
        {
            GameObject nearbyPlayer = collider.gameObject;
            if (nearbyPlayer != currentHotPlayer) // Certifica-se de que não está tentando passar para si mesmo
            {
                PassHotPotato(nearbyPlayer); // Passa a batata quente
                AddScore(currentHotPlayer, 1); // Dá um ponto para quem passou
                break;
            }
        }
    }

    private void PlayAttackAnimation(GameObject player)
    {
        // Aciona a animação de ataque para o jogador especificado
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    private int GetPlayerNumber(GameObject player)
    {
        hotpotatoController controller = player.GetComponent<hotpotatoController>();
        return controller != null ? controller.playerNumber : 0;
    }
}
