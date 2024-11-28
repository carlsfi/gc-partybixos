using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento
    public float jumpForce = 7f; // Força do pulo
    private Vector3 movement; // Direção do movimento
    private Rigidbody rb; // Referência ao Rigidbody
    private Animator animator; // Referência ao Animator

    public int playerNumber; // Número do jogador
    public float deadzone = 0.2f; // Zona morta para evitar movimento indesejado
    public float rotationSpeed = 10f; // Velocidade da rotação
    private bool isGrounded = true; // Verifica se o jogador está no chão

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Obtém o componente Animator
    }

    private void Update()
    {
        // Captura os inputs para o jogador específico
        float moveHorizontal = Input.GetAxis($"P{playerNumber}_Horizontal");
        float moveVertical = -Input.GetAxis($"P{playerNumber}_Vertical"); // Inverte o eixo Y

        // Aplica a deadzone para ignorar movimentos pequenos
        if (Mathf.Abs(moveHorizontal) < deadzone) moveHorizontal = 0;
        if (Mathf.Abs(moveVertical) < deadzone) moveVertical = 0;

        // Define a direção do movimento
        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Faz o personagem olhar na direção do movimento
        if (movement.magnitude > 0.1f) // Apenas rotaciona se houver movimento significativo
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Verifica entrada para pular (apenas o controle associado ao jogador)
        if (Input.GetButtonDown($"P{playerNumber}_Jump") && isGrounded)
        {
            Jump();
        }

        // Atualiza as animações
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // Move o jogador usando o Rigidbody
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            // Define o estado de correr ou parar
            bool isRunning = movement.magnitude > 0.1f; // Se há movimento significativo
            animator.SetBool("isRunning", isRunning);

            // Define o estado de pulo
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void Jump()
    {
        // Aplica a força de pulo no Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Indica que o jogador não está no chão

        // Ativa a animação de pulo
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o jogador tocou o chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Reseta a animação de pulo
            if (animator != null)
            {
                animator.ResetTrigger("Jump");
                animator.SetBool("isJumping", false);
            }
        }
    }
}