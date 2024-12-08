using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1; // Identificador do jogador (1, 2, 3 ou 4)
    public float moveSpeed = 5f;
    public float airMoveSpeed = 2f;
    public float jumpForce = 7f;
    public float gravity = 35f;
    public float deadzone = 0.2f;
    public float rotationSpeed = 10f;
    public float airRotationSpeed = 5f;
    public LayerMask groundLayer;

    public Transform holdPoint; // Ponto onde o lixo será carregado
    private GameObject heldWaste = null; // Lixo que o jogador está segurando
    private WasteManager wasteManager; // Referência ao WasteManager

    private CharacterController characterController;
    private Vector3 movement;
    private Vector3 velocity;
    private Animator animator;
    private bool isGrounded = false;

    private string horizontalAxis;
    private string verticalAxis;
    private string jumpButton;
    private string actionButton;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        wasteManager = FindObjectOfType<WasteManager>();

        // Configura os inputs baseados no número do jogador
        horizontalAxis = $"P{playerNumber}_Horizontal";
        verticalAxis = $"P{playerNumber}_Vertical";
        jumpButton = $"P{playerNumber}_Jump";
        actionButton = $"P{playerNumber}_X"; // Botão "X" do controle Xbox

        CheckGrounded();

        if (isGrounded)
        {
            velocity.y = -2f;
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleWastePickup();
        UpdateAnimations();
    }

    private void CheckGrounded()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = -Input.GetAxis(verticalAxis);

        if (Mathf.Abs(moveHorizontal) < deadzone) moveHorizontal = 0;
        if (Mathf.Abs(moveVertical) < deadzone) moveVertical = 0;

        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        float currentMoveSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown(jumpButton) && isGrounded)
        {
            Jump();
        }

        characterController.Move((movement * currentMoveSpeed + velocity) * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = jumpForce;
        isGrounded = false;

        if (animator != null)
        {
            if (movement.magnitude > 0.1f)
            {
                animator.SetTrigger("Hop");
            }
            else
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void HandleWastePickup()
    {
        if (heldWaste == null && Input.GetButtonDown(actionButton)) // Botão "X" do controle
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 2f);

            foreach (var obj in nearbyObjects)
            {
                if (obj.CompareTag("Lixo"))
                {
                    heldWaste = obj.gameObject;
                    heldWaste.transform.SetParent(holdPoint);
                    heldWaste.transform.localPosition = Vector3.zero;
                    heldWaste.GetComponent<Collider>().enabled = false;

                    if (animator != null)
                    {
                        animator.SetTrigger("Pickup");
                    }

                    Debug.Log($"Jogador {playerNumber} pegou: {heldWaste.name}");
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lixeira"))
        {
            Debug.Log($"Jogador {playerNumber} colidiu com a lixeira: {collision.gameObject.name}");

            if (heldWaste != null)
            {
                Debug.Log($"Jogador {playerNumber} entregou o lixo: {heldWaste.name}");

                if (wasteManager != null)
                {
                    wasteManager.DepositWaste(heldWaste, collision.transform, playerNumber);
                    DropHeldWaste(); // Solta o lixo
                }
                else
                {
                    Debug.LogError("WasteManager não está configurado no PlayerController.");
                }
            }
            else
            {
                Debug.Log($"Jogador {playerNumber} não está segurando nenhum lixo.");
            }
        }
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", movement.magnitude > 0.1f);
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    public GameObject GetHeldWaste()
    {
        return heldWaste;
    }

    public void DropHeldWaste()
    {
        if (heldWaste != null)
        {
            heldWaste.transform.SetParent(null);
            heldWaste.GetComponent<Collider>().enabled = true;
            heldWaste = null;

            Debug.Log($"Jogador {playerNumber} soltou o lixo.");
        }
    }
}
