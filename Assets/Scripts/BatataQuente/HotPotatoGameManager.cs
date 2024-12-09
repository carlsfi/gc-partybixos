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
    public float defeatDuration = 3f; // Duração do estado de derrota

    public MessagePotato messagePotato; // Referência ao MessagePotato

    private GameObject currentHotPlayer; // Jogador com a batata quente
    private float roundTimer;
    private int currentRound = 1;

    private Dictionary<GameObject, int> playerScores = new Dictionary<GameObject, int>(); // Pontos de cada jogador
    private Dictionary<GameObject, bool> isPlayerActive = new Dictionary<GameObject, bool>(); // Status de atividade dos jogadores

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

        foreach (var player in players)
        {
            if (isPlayerActive[player] && Input.GetButtonDown($"P{GetPlayerNumber(player)}_X"))
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

        // Inicializa os pontos e status dos jogadores
        foreach (var player in players)
        {
            playerScores[player] = totalRounds; // Todos começam com a pontuação equivalente ao total de rounds
            isPlayerActive[player] = true; // Marca todos os jogadores como ativos
        }

        // Sincroniza com o GameData
        if (GameData.playerScores == null || GameData.playerScores.Count != players.Count)
        {
            GameData.playerScores = new List<int>(new int[players.Count]); // Inicializa com 0 para cada jogador
        }

        SyncGameDataScores(); // Sincroniza após inicializar
        ChooseRandomHotPlayer(); // Escolhe o jogador inicial
        StartCoroutine(DisplayRoundMessage()); // Mostra a mensagem do Round 1 com o jogador inicial
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

        // Penaliza o jogador que ficou com a batata quente
        if (playerScores.ContainsKey(currentHotPlayer))
        {
            playerScores[currentHotPlayer]--;
            Debug.Log($"Jogador {currentHotPlayer.name} perdeu 1 ponto! Nova pontuação: {playerScores[currentHotPlayer]}");
        }
        else
        {
            Debug.LogError($"Jogador {currentHotPlayer.name} não encontrado em playerScores!");
        }

        SyncGameDataScores(); // Sincroniza a pontuação

        Debug.Log("Pontuação atualizada no fim da rodada:");
        foreach (var player in players)
        {
            Debug.Log($"{player.name}: {playerScores[player]} pontos");
        }

        StartCoroutine(HandleDefeat(currentHotPlayer)); // Aplica animação de derrota no perdedor
        NextRound();
    }

    private void NextRound()
    {
        currentRound++;

        if (currentRound > totalRounds)
        {
            EndGame(); // Finaliza o minigame se o número de rounds for excedido
        }
        else
        {
            // Reduz o tempo por round, mas nunca abaixo de 10 segundos
            roundTimer = Mathf.Max(10f, initialRoundTime - (timeReductionPerRound * (currentRound - 1)));
            ShowRoundMessage(); // Mostra mensagem do próximo round
            ChooseRandomHotPlayer(); // Escolhe o próximo jogador com a batata
        }
    }

    private void EndGame()
    {
        Debug.Log("Fim do Minigame! Calculando os resultados...");
        SyncGameDataScores(); // Garante que a pontuação esteja sincronizada antes do cálculo

        GameObject winner = null;
        int maxScore = int.MinValue;
        List<GameObject> tiedPlayers = new List<GameObject>();

        foreach (var player in players)
        {
            Debug.Log($"{player.name}: {playerScores[player]} pontos");
            if (playerScores[player] > maxScore)
            {
                maxScore = playerScores[player];
                tiedPlayers.Clear();
                tiedPlayers.Add(player);
            }
            else if (playerScores[player] == maxScore)
            {
                tiedPlayers.Add(player);
            }
        }

        if (tiedPlayers.Count == 1)
        {
            winner = tiedPlayers[0];
            Debug.Log($"O vencedor foi {winner.name} com {maxScore} pontos!");
            StartCoroutine(ShowWinnerAndReturnToBoard(winner, null));
        }
        else
        {
            Debug.Log("Houve um empate!");
            StartCoroutine(ShowWinnerAndReturnToBoard(null, tiedPlayers));
        }
    }

    private IEnumerator ShowWinnerAndReturnToBoard(GameObject winner, List<GameObject> tiedPlayers)
    {
        if (messagePotato != null)
        {
            if (winner != null)
            {
                messagePotato.ShowMessage($"O vencedor foi: {winner.name}!");
                UpdateGameDataWinner(winner);
            }
            else if (tiedPlayers != null && tiedPlayers.Count > 0)
            {
                string tiedNames = string.Join(", ", tiedPlayers.ConvertAll(p => p.name));
                messagePotato.ShowMessage($"Empate entre: {tiedNames}!");
                UpdateGameDataTie(tiedPlayers);
            }
        }

        yield return new WaitForSeconds(5f); // Aguarda 5 segundos antes de voltar ao tabuleiro

        UnityEngine.SceneManagement.SceneManager.LoadScene("Tabuleiro");
    }

    private void SyncGameDataScores()
    {
        if (GameData.playerScores != null && GameData.playerScores.Count == players.Count)
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameData.playerScores[i] = playerScores[players[i]];
                Debug.Log($"Sincronizando pontuação: {players[i].name} -> {GameData.playerScores[i]}");
            }
        }
        else
        {
            Debug.LogError("GameData.playerScores está fora de sincronia com a lista de jogadores!");
        }
    }

    private void UpdateGameDataWinner(GameObject winner)
    {
        int index = players.IndexOf(winner);
        if (index >= 0 && index < GameData.playerScores.Count)
        {
            GameData.playerScores[index]++;
        }
    }

    private void UpdateGameDataTie(List<GameObject> tiedPlayers)
    {
        foreach (var player in tiedPlayers)
        {
            int index = players.IndexOf(player);
            if (index >= 0 && index < GameData.playerScores.Count)
            {
                GameData.playerScores[index]++;
            }
        }
    }

    private IEnumerator DisplayRoundMessage()
    {
        if (messagePotato != null)
        {
            // Mostra o início do Round 1
            messagePotato.ShowMessage($"Round {currentRound} Iniciando!");
            yield return new WaitForSeconds(1f); // Aguarda a exibição da primeira mensagem

            // Mostra o jogador inicial
            if (currentHotPlayer != null)
            {
                messagePotato.ShowMessage($"Jogador inicial: {currentHotPlayer.name}");
            }
        }
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

    private int GetPlayerNumber(GameObject player)
    {
        hotpotatoController controller = player.GetComponent<hotpotatoController>();
        return controller != null ? controller.playerNumber : 0;
    }

    private void PlayAttackAnimation(GameObject player)
    {
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void TryPassHotPotato()
    {
        Collider[] nearbyPlayers = Physics.OverlapSphere(currentHotPlayer.transform.position, proximityRadius, LayerMask.GetMask("Player"));
        foreach (var collider in nearbyPlayers)
        {
            GameObject nearbyPlayer = collider.gameObject;
            if (nearbyPlayer != currentHotPlayer && isPlayerActive[nearbyPlayer])
            {
                PassHotPotato(nearbyPlayer);
                break;
            }
        }
    }

    private void PassHotPotato(GameObject newHotPlayer)
    {
        RemoveOutline(currentHotPlayer);
        currentHotPlayer = newHotPlayer;
        ApplyOutline(newHotPlayer);
        Debug.Log($"{currentHotPlayer.name} recebeu a batata quente!");
    }

    private IEnumerator HandleDefeat(GameObject player)
    {
        isPlayerActive[player] = false;
        Animator animator = player.GetComponent<Animator>();
        hotpotatoController controller = player.GetComponent<hotpotatoController>();
        if (controller != null)
        {
            controller.DisableMovement();
        }

        if (animator != null)
        {
            animator.SetTrigger("Defeat");
        }

        yield return new WaitForSeconds(defeatDuration);

        if (controller != null)
        {
            controller.EnableMovement();
        }

        isPlayerActive[player] = true;
    }

    private void ShowRoundMessage()
{
    if (messagePotato != null)
    {
        messagePotato.ShowMessage($"Round {currentRound} Iniciando!");
    }
}

}
