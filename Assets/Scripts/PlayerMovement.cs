using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array de waypoints (caminho a seguir)
    public int currentWaypointIndex = 0; // Índice do waypoint atual
    public float moveSpeed = 2f; // Velocidade do movimento
    public float rotationSpeed = 10f; // Velocidade da rotação
    public float jumpHeight = 1f; // Altura do pulo
    public float jumpDuration = 0.5f; // Duração do pulo

    public Animator animator; // Referência ao Animator

    private void Start()
    {
        // Garante que o personagem começa no estado Idle
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }

    public IEnumerator MovePlayer(int steps)
    {
        Debug.Log($"Iniciando movimentação com {steps} passos. Índice atual: {currentWaypointIndex}");

        while (steps > 0)
        {
            // Verifica se há mais waypoints no caminho
            if (currentWaypointIndex + 1 >= waypoints.Length)
            {
                Debug.Log("Fim do caminho!");
                yield break; // Finaliza o movimento se não houver mais waypoints
            }

            // Obtém o próximo waypoint
            Transform nextWaypoint = waypoints[currentWaypointIndex + 1];
            Debug.Log($"Movendo para o próximo waypoint: {nextWaypoint.name}");

            // Rotaciona o personagem para olhar na direção do próximo waypoint
            yield return StartCoroutine(RotateTowards(nextWaypoint.position));

            // Move o jogador até o próximo waypoint com um pulo
            yield return StartCoroutine(JumpTo(nextWaypoint.position));

            // Atualiza o índice do waypoint atual
            currentWaypointIndex++;
            steps--; // Reduz o número de passos restantes

            // Pequena pausa para visualização entre passos
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Jogador terminou de se mover.");
    }

    // Rotaciona o personagem para olhar na direção do destino
    private IEnumerator RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized; // Direção para o destino
        Quaternion targetRotation = Quaternion.LookRotation(direction); // Rotação desejada

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null; // Espera até a próxima frame
        }

        transform.rotation = targetRotation; // Ajusta a rotação final
    }

    // Faz o personagem "pular" até o destino
    private IEnumerator JumpTo(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        // Aciona a animação de pulo
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }

        while (elapsedTime < jumpDuration)
        {
            // Calcula o progresso do movimento
            float t = elapsedTime / jumpDuration;

            // Interpola entre a posição inicial e o destino
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);

            // Adiciona o efeito de pulo (parábola)
            position.y += jumpHeight * Mathf.Sin(t * Mathf.PI);

            // Atualiza a posição do jogador
            transform.position = position;

            elapsedTime += Time.deltaTime;
            yield return null; // Espera até a próxima frame
        }

        // Garante que o jogador chegue exatamente no destino
        transform.position = targetPosition;

        // Retorna ao estado Idle automaticamente
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }
}
