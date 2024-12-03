using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1; // Identificador do jogador (1, 2, 3 ou 4)
    public float moveSpeed = 5f; // Velocidade de movimento no chão
    public float airMoveSpeed = 2f; // Velocidade de movimento no ar
    public float jumpForce = 7f; // Força do pulo
    public float gravity = 35f; // Gravidade aplicada ao jogador
    public float deadzone = 0.2f; // Zona morta para evitar movimento indesejado
    public float rotationSpeed = 10f; // Velocidade da rotação no chão
    public float airRotationSpeed = 5f; // Velocidade da rotação no ar
    public LayerMask groundLayer; // Camada para detectar o chão
    public float kickForce = 10f; // Força do chute na bola

    private CharacterController characterController;
    private Vector3 movement; // Movimento horizontal
    private Vector3 velocity; // Movimento vertical
    private Animator animator; // Referência ao Animator
    private bool isGrounded = false; // Verifica se o jogador está no chão

    private string horizontalAxis; // Nome do eixo horizontal
    private string verticalAxis; // Nome do eixo vertical
    private string jumpButton; // Nome do botão de pulo

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Configura os inputs baseados no número do jogador
        horizontalAxis = $"P{playerNumber}_Horizontal";
        verticalAxis = $"P{playerNumber}_Vertical";
        jumpButton = $"P{playerNumber}_Jump";

        // Verifica imediatamente se o personagem está no chão
        CheckGrounded();

        // Inicializa a velocidade vertical para evitar lançamento para cima
        if (isGrounded)
        {
            velocity.y = -2f;
        }
    }

    private void Update()
    {
        // Detecta se o jogador está no chão
        CheckGrounded();

        // Captura os inputs específicos do jogador e inverte o eixo Y
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = -Input.GetAxis(verticalAxis); // Inversão do eixo Y

        // Aplica a deadzone
        if (Mathf.Abs(moveHorizontal) < deadzone) moveHorizontal = 0;
        if (Mathf.Abs(moveVertical) < deadzone) moveVertical = 0;

        // Define a direção de movimento
        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Define a velocidade de movimento com base no estado de grounded
        float currentMoveSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        // Define a velocidade de rotação com base no estado de grounded
        float currentRotationSpeed = isGrounded ? rotationSpeed : airRotationSpeed;

        // Rotaciona o jogador para a direção do movimento
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
        }

        // Controle de pulo: detecta o botão específico de pulo
        if (Input.GetButtonDown(jumpButton) && isGrounded)
        {
            Jump(); // Aplica força de pulo e inicia a animação
        }

        // Aplica gravidade apenas quando não está no chão
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        // Move o jogador com a velocidade ajustada
        characterController.Move((movement * currentMoveSpeed + velocity) * Time.deltaTime);

        // Atualiza as animações
        UpdateAnimations();
    }

    private void CheckGrounded()
    {
        // Verifica se o CharacterController está tocando o chão
        isGrounded = characterController.isGrounded;

        // Alternativa: Usar Raycast para detecção mais confiável
        if (!isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f, groundLayer))
            {
                isGrounded = true;
                velocity.y = -2f; // Evita pequenos saltos quando no chão
            }
        }

        // Zera a velocidade vertical quando está no chão
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void Jump()
    {
        velocity.y = jumpForce; // Aplica força de pulo
        isGrounded = false; // Define como não no chão

        // Ativa a animação de pulo ou hop
        if (animator != null)
        {
            if (movement.magnitude > 0.1f) // Se estiver se movendo
            {
                animator.SetTrigger("Hop");
            }
            else
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            // Atualiza o estado de corrida
            animator.SetBool("isRunning", movement.magnitude > 0.1f);

            // Atualiza o estado de pulo
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verifica se o objeto colidido é a bola
        if (hit.collider.gameObject.name == "Bola")
        {
            Rigidbody ballRigidbody = hit.collider.attachedRigidbody;

            // Aplica força apenas se for um Rigidbody válido e não kinematic
            if (ballRigidbody != null && !ballRigidbody.isKinematic)
            {
                // Direção do chute: do personagem para a bola
                Vector3 forceDirection = hit.point - transform.position;
                forceDirection.y = 0; // Mantém a força apenas no plano horizontal
                forceDirection.Normalize();

                // Aplica a força do chute
                ballRigidbody.AddForce(forceDirection * kickForce, ForceMode.Impulse);
            }
        }
    }
}