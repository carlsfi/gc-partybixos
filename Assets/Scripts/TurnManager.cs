using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> players; // Lista dos objetos dos jogadores
    public DiceRoll dice; // Referência ao objeto dado (com o script DiceRoll)
    private List<GameObject> turnOrder = new List<GameObject>(); // Ordem dos turnos
    private int currentPlayerIndex = 0; // Índice do jogador atual

    private void Start()
    {
        StartCoroutine(InitializeTurnOrder());
    }

    // Define a ordem de turnos baseada nas rolagens iniciais
    private IEnumerator InitializeTurnOrder()
    {
        Dictionary<GameObject, int> playerRolls = new Dictionary<GameObject, int>();

        // Rola o dado para cada jogador na sequência de definição inicial
        foreach (var player in players)
        {
            Debug.Log("É a vez do jogador " + player.name + " para definir a ordem de turnos. Clique no dado para rolar.");
            yield return StartCoroutine(WaitForPlayerRoll(player)); // Aguarda o jogador rolar o dado

            // Armazena o valor da rolagem inicial
            playerRolls.Add(player, dice.diceValue);
            Debug.Log("Jogador " + player.name + " tirou o valor: " + dice.diceValue);
        }

        // Ordena os jogadores pelo valor do dado (maior para menor)
        turnOrder = playerRolls.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

        // Lida com empates (50/50)
        for (int i = 0; i < turnOrder.Count - 1; i++)
        {
            if (playerRolls[turnOrder[i]] == playerRolls[turnOrder[i + 1]])
            {
                if (Random.Range(0, 2) == 0)
                {
                    // Troca a posição dos jogadores empatados aleatoriamente
                    var temp = turnOrder[i];
                    turnOrder[i] = turnOrder[i + 1];
                    turnOrder[i + 1] = temp;
                }
            }
        }

        Debug.Log("Ordem de turnos definida:");
        foreach (var player in turnOrder)
        {
            Debug.Log(player.name + " com valor de dado: " + playerRolls[player]);
        }

        // Inicia o ciclo de turnos principal
        StartCoroutine(HandleTurn());
    }

    // Ciclo principal de turnos
    private IEnumerator HandleTurn()
    {
        while (true)
        {
            GameObject currentPlayer = turnOrder[currentPlayerIndex];
            Debug.Log("Turno de: " + currentPlayer.name + ". Clique no dado para rolar.");

            // Aguarda o jogador rolar o dado
            yield return StartCoroutine(WaitForPlayerRoll(currentPlayer));

            // Ação do jogador pode ser adicionada aqui
            Debug.Log("Jogador " + currentPlayer.name + " rolou: " + dice.diceValue);

            // Passa para o próximo jogador
            currentPlayerIndex = (currentPlayerIndex + 1) % turnOrder.Count;
        }
    }

    // Aguarda o jogador clicar no dado para rolar
    private IEnumerator WaitForPlayerRoll(GameObject player)
    {
        bool hasRolled = false;

        // Inscreve-se no evento de rolagem do dado
        dice.OnRollComplete = () =>
        {
            hasRolled = true;
        };

        // Espera até que o jogador role o dado
        while (!hasRolled)
        {
            yield return null;
        }
    }
}
