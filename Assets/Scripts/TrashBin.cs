using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public WasteManager wasteManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lixo"))
        {
            GameObject waste = other.gameObject;

            // Verifica o jogador segurando o lixo
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                if (player.GetHeldWaste() == waste)
                {
                    wasteManager.DepositWaste(waste, transform, player.playerNumber);
                    player.DropHeldWaste();
                    return;
                }
            }

            Debug.LogWarning("Lixo detectado, mas não foi possível identificar o jogador.");
        }
    }
}
