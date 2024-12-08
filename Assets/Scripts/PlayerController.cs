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

        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = -Input.GetAxis(verticalAxis);

        if (Mathf.Abs(moveHorizontal) < deadzone) moveHorizontal = 0;
        if (Mathf.Abs(moveVertical) < deadzone) moveVertical = 0;

        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        float currentMoveSpeed = isGrounded ? moveSpeed : airMoveSpeed;
        float currentRotationSpeed = isGrounded ? rotationSpeed : airRotationSpeed;

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown(jumpButton) && isGrounded)
        {
            Jump();
        }

        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move((movement * currentMoveSpeed + velocity) * Time.deltaTime);

        HandleWastePickup();

        UpdateAnimations();
    }

    private void CheckGrounded()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f, groundLayer))
            {
                isGrounded = true;
                velocity.y = -2f;
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
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

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", movement.magnitude > 0.1f);
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.name == "Bola")
        {
            Rigidbody ballRigidbody = hit.collider.attachedRigidbody;

            if (ballRigidbody != null && !ballRigidbody.isKinematic)
            {
                Vector3 forceDirection = hit.point - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                ballRigidbody.AddForce(forceDirection * 10f, ForceMode.Impulse);
            }
        }
    }

    // --- Novos Métodos Adicionados ---
    public GameObject GetHeldWaste()
    {
        return heldWaste;
    }

    public bool HasWaste()
    {
        return heldWaste != null;
    }

    public void DropHeldWaste()
    {
        if (heldWaste != null)
        {
            Destroy(heldWaste);
            heldWaste = null;
        }
    }
}
