using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]

    public float walkSpeed;
    public float runSpeed;
    public bool isRunning;


    public float jumpForce;

    public float gravity;
    public float lateralSprintSpeedPenalty = 0.75f;


    [Header("References")]

    public Transform headCheckPosition;
    public Transform groundCheckPosition;

    private CharacterController characterController;
    private Transform cameraTransform;


    [Header("Other")]

    public float groundCheckRadius;
    public float headCheckRadius;
    public LayerMask groundMask;

    //Private variables

    private float xMove;
    private float zMove;

    private Vector3 movementVector;

    private Vector3 inputDirection;

    private float moveSpeed;
    private bool isGrounded;
    private bool isSliding;

    private bool hitHead;

    public bool movementLocked = false;

    private Vector3 slideVector;
    private float slideAcceleration = 0f;
    private float slopeSlideSpeed = 3f;
    private RaycastHit slopeHit;




    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");

        SetPlayerMoveSpeed();

        GetGroundedState();

        //isGrounded = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, groundMask);

        hitHead = Physics.CheckSphere(headCheckPosition.position, headCheckRadius, groundMask);

        if (isGrounded)
        {
            PlayerHitsGround();
        }

        inputDirection = (transform.right * xMove + transform.forward * zMove).normalized;

        movementVector.x = inputDirection.x * moveSpeed;
        movementVector.z = inputDirection.z * moveSpeed;

        if (isSliding && movementVector.y <= 0)
        {
            SteepSlopeMovement();
        }
        else 
        {
            slideAcceleration = 0f;
            const float SLIDE_DECAY = 5f;
            slideVector = Vector3.Lerp(slideVector, Vector3.zero, SLIDE_DECAY * Time.deltaTime);
        }


        if (hitHead && movementVector.y > 0)
        {
            movementVector.y = -0.1f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isSliding)
        {
            movementVector.y += jumpForce;
        }


        //clamp maximum vertical speed from jumping
        if (movementVector.y > jumpForce)
        {
            movementVector.y = jumpForce;
        }

        movementVector.y += gravity * Time.deltaTime;


        Vector3 totalMovement = movementVector + slideVector;


        if (!movementLocked)
        {
            characterController.Move(totalMovement * Time.deltaTime);
        }

    }

    void GetGroundedState()
    {
        // Reset states
        isGrounded = false;
        isSliding = false;

        Vector3 playerPos = transform.position;
        const float RAY_DISTANCE = 1.2f;
        const int RAY_COUNT = 16;
        const float ANGLE_STEP = 360f / RAY_COUNT;
        const float RAYCAST_RADIUS = 0.52f;

        // Raycase downward in a ring around the player
        for (int i = 0; i < RAY_COUNT; i++)
        {
            Vector3 rayOrigin = playerPos +
                               Quaternion.Euler(0, i * ANGLE_STEP, 0) *
                               Vector3.forward * RAYCAST_RADIUS;

            if (Physics.Raycast(rayOrigin, Vector3.down, out var hitInRing, RAY_DISTANCE, groundMask))
            {
                // If we hit anything, we're grounded
                isGrounded = true;

                float hitAngle = Vector3.Angle(hitInRing.normal, Vector3.up);

                // If any of the rays hit flat ground, we aren't sliding and exit the loop
                if (hitAngle < characterController.slopeLimit)
                {
                    isSliding = false;
                    break;
                }
                    
                // Check if we hit a steep slope and haven't hit a flat area yet, we use this slope and start sliding
                if (hitAngle > characterController.slopeLimit && hitAngle < 90f)
                {
                    isSliding = true;
                    slopeHit = hitInRing;
                }
            }
        }
    }

    private void SteepSlopeMovement()
    {
        if (slopeHit.collider == null) return;

        // Calculate the downhill direction
        Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, slopeHit.normal).normalized;

        // Calculate slope angle
        float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
        float steepness = Mathf.Clamp01((slopeAngle - characterController.slopeLimit) / 90f);

        // Calculate control reduction - clamp to avoid negative values
        float controlFactor = Mathf.Max(0f, 0.5f - steepness);
        movementVector *= controlFactor;
        movementVector = Vector3.zero;

        // Apply gravity-based sliding
        slideAcceleration += -gravity * Time.deltaTime;
        slideVector = slopeDir * slideAcceleration;
    }


    void PlayerHitsGround()
    {

        if (movementVector.y < 0f) //slowly push player down so they keep in contact with the ground
        {
            movementVector.y = -2f;
        }
    }


    void SetPlayerMoveSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && zMove == 1)
        {
            moveSpeed = runSpeed;
            isRunning = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            isRunning = false;
        }
    }




}
