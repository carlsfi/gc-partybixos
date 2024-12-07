using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class DeathZone : MonoBehaviour
    {
        public float respawnDelay = 0.5f;

        private Dictionary<string, Vector3> playerRespawnPositions = new Dictionary<string, Vector3>
        {
            { "CACHORRO", new Vector3(-2.3f, 10f, 0.8f) },
            { "PATO", new Vector3(5.9f, 10f, -10.8f) },
            { "POLAR", new Vector3(-3f, 10f, -10.8f) },
            { "URSO", new Vector3(5.7f, 10f, -1.1f) },
        };

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                RespawnPlayerWithDelay(collider.gameObject);
            }
        }

        private async void RespawnPlayerWithDelay(GameObject player)
        {
            Debug.Log($"Jogador {player.name} morreu. Respawn em {respawnDelay} segundos...");

            await Task.Delay((int)(respawnDelay * 1000));

            if (playerRespawnPositions.TryGetValue(player.name, out Vector3 respawnPosition))
            {
                Debug.Log($"Respawnando o jogador {player.name} na posição {respawnPosition}");
                player.transform.position = respawnPosition;
            }
            else
            {
                Debug.LogWarning(
                    $"Nenhuma posição de respawn encontrada para {player.name}. Usando posição padrão."
                );
                player.transform.position = new Vector3(0f, 10f, 0f);
            }
        }
    }
}
