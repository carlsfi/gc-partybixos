using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public WasteManager wasteManager; // Referência ao WasteManager para verificar o lixo
    public string correctWasteType; // Tipo correto de lixo para esta lixeira (ex.: "vidro", "plastico")

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o jogador colidiu
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log($"Jogador {player.playerNumber} colidiu com a lixeira: {gameObject.name}");

            // Verifica se o jogador está segurando lixo
            GameObject heldWaste = player.GetHeldWaste();
            if (heldWaste != null)
            {
                // Verifica se o lixo é do tipo correto
                string wasteType = wasteManager.GetWasteType(heldWaste);
                if (wasteType == correctWasteType)
                {
                    Debug.Log($"Lixo {heldWaste.name} depositado CORRETAMENTE na lixeira {gameObject.name}.");
                    wasteManager.DepositWaste(heldWaste, transform, player.playerNumber);
                    player.DropHeldWaste();
                }
                else
                {
                    Debug.Log($"Lixo {heldWaste.name} foi colocado na lixeira ERRADA ({gameObject.name}).");
                    player.DropHeldWaste(); // Opcional: solta o lixo mesmo sendo incorreto
                }
            }
            else
            {
                Debug.Log($"Jogador {player.playerNumber} não está segurando nenhum lixo.");
            }
        }
    }
}
