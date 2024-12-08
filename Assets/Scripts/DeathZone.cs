using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Dictionary<string, Vector3> playerRespawnPositions = new Dictionary<string, Vector3>
    {
        { "CACHORRO", new Vector3(-2.3f, 10f, 0.8f) },
        { "PATO", new Vector3(5.9f, 10f, -10.8f) },
        { "POLAR", new Vector3(-3f, 10f, -10.8f) },
        { "URSO", new Vector3(5.7f, 10f, -1.1f) },
    };

    private HashSet<GameObject> playersInDeathZone = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Jogador {collider.gameObject.name} entrou na zona de morte.");

            if (!playersInDeathZone.Contains(collider.gameObject))
            {
                playersInDeathZone.Add(collider.gameObject);
                RoundManager.Instance.PlayerLeftHexagon(collider.gameObject);
            }
        }
    }

    public Vector3 GetRespawnPosition(string playerName)
    {
        if (playerRespawnPositions.TryGetValue(playerName, out Vector3 respawnPosition))
        {
            return respawnPosition;
        }
        else
        {
            Debug.LogWarning($"Nenhuma posição de respawn encontrada para {playerName}. Usando posição padrão.");
            return new Vector3(0f, 10f, 0f);
        }
    }

    public void RespawnAllPlayers()
    {
        foreach (var player in playersInDeathZone)
        {
            if (player != null)
            {
                string playerName = player.name;
                Vector3 respawnPosition = GetRespawnPosition(playerName);

                // Verifica e desativa o CharacterController antes de modificar a posição
                CharacterController characterController = player.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    characterController.enabled = false;
                }

                // Atualiza a posição do jogador
                player.transform.position = respawnPosition;

                // Reativa o CharacterController
                if (characterController != null)
                {
                    characterController.enabled = true;
                }

                Debug.Log($"Respawnando jogador {playerName} na posição {respawnPosition}.");
            }
        }
        playersInDeathZone.Clear();
    }

}
