using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float gravityMultiplier = 5f;
    [SerializeField] Transform cameraTransform;

    private CharacterController characterController;
    private Animator animator;
    private float ySpeed;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;
    public float jumpButtonGracePeriod = 0.2f;
    public float jumpHorizontalSpeed = 5f;
    private bool isJumping;
    private bool isGrounded;
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;
        HandleInput();
        ApplyGravity();
        HandleJump();
        MovePlayer();
        RotatePlayer();
        HandleEmotes();
    }

    void HandleEmotes()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayEmote("Wave");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayEmote("Salsa");
        }
    }

    void PlayEmote(string emoteName)
    {
        animator.SetTrigger(emoteName);
    }

    // Handles input for movement and jumping
    void HandleInput()
    {
        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }
    }

    // Handles gravity logic
    void ApplyGravity()
    {
        float gravity = Physics.gravity.y * gravityMultiplier;

        if (isJumping && ySpeed > 0 && !Input.GetButton("Jump"))
        {
            gravity *= 2; // Apply stronger gravity when jump button is released early
        }

        ySpeed += gravity * Time.deltaTime;
    }

    // Handles jumping logic
    void HandleJump()
    {
        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            ySpeed = -0.5f;
            SetAnimationState(true, false, false); 

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y * gravityMultiplier);
                isJumping = true;
                SetAnimationState(false, true, false); 
                jumpButtonPressedTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);

            if ((isJumping && ySpeed < 2.0f) || ySpeed < -5.0f)
            {
                animator.SetBool("isFalling", true);
            }
        }
    }

    // Moves the player based on camera direction
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = GetCameraRelativeDirection(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        bool isMoving = inputMagnitude > 0;

        // Running logic
        bool isRunning = isMoving && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        float speed = isRunning ? runSpeed : walkSpeed;

        animator.SetFloat("Move", isRunning ? 0.5f : inputMagnitude, 0.05f, Time.deltaTime);

        // Apply movement
        Vector3 velocity = moveDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    // Rotates the player smoothly
    void RotatePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = GetCameraRelativeDirection(horizontalInput, verticalInput);

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Returns the movement direction relative to the camera
    Vector3 GetCameraRelativeDirection(float horizontalInput, float verticalInput)
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        return (camForward * verticalInput + camRight * horizontalInput).normalized;
    }

    // Sets animation states
    void SetAnimationState(bool grounded, bool jumping, bool falling)
    {
        animator.SetBool("isGrounded", grounded);
        animator.SetBool("isJumping", jumping);
        animator.SetBool("isFalling", falling);
        isGrounded = grounded;
        isJumping = jumping;
    }

    public bool IsRunning()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return (horizontalInput != 0 || verticalInput != 0) && Input.GetKey(KeyCode.LeftShift);
    }

    public bool IsWalking()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        return movementInput.sqrMagnitude > 0.01f; 
    }
}
