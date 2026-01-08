using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]

    public float walkSpeed;
    public float runSpeed;

    public float jumpForce;

    public float gravity;


    [Header("References")]

    public Transform headCheckPosition;

    private CharacterController characterController;
    private Transform cameraTransform;


    [Header("Other")]

    public float groundCheckRadius;
    public float headCheckRadius;
    public LayerMask groundMask;

    //Private variables

    private float xMove;
    private float zMove;
    private float ySpeed;

    private float movementSmoothing = 15f;

    private Vector3 targetMoveDir;
    private Vector3 movementVector;

    private Vector3 inputDirection;

    private float moveSpeed;
    private bool isGrounded;


    private bool hitHead;

    public bool movementLocked = false;


    private bool isSliding;
    private Vector3 slideVector;
    private float slideAcceleration = 0f;
    private RaycastHit slopeHit;

    private Vector3 hitNormal;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        HandleGrounding();

        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");

        SetPlayerMoveSpeed();

        hitHead = Physics.CheckSphere(headCheckPosition.position, headCheckRadius, groundMask);

        inputDirection = (transform.right * xMove + transform.forward * zMove).normalized;

        targetMoveDir = inputDirection * moveSpeed;

        bool isAgainstSteepSlope = Vector3.Angle(Vector3.up, hitNormal) > characterController.slopeLimit;

        if (isGrounded)
        {
            PlayerHitsGround();

            if (isSliding && ySpeed <= 0)
            {
                SteepSlopeMovement();
            }
            else
            {
                slideAcceleration = 0f;
                slideVector = Vector3.zero;
            }
        }

        ySpeed += gravity * Time.deltaTime;

        if (hitHead)
        {
            Debug.Log("Hit head");

            if (ySpeed > 0)
            {
                // push the player down if they hit their head
                ySpeed = -2f;
            }
        }



        if (Input.GetButtonDown("Jump") && isGrounded && !isSliding)
        {
            ySpeed += jumpForce;
        }


        //clamp maximum vertical speed from jumping
        if (ySpeed > jumpForce)
        {
            ySpeed = jumpForce;
        }

        movementVector = Vector3.MoveTowards(movementVector, targetMoveDir, movementSmoothing * Time.deltaTime);

        Vector3 totalMovement = movementVector + slideVector;


        if (!movementLocked)
        {
            characterController.Move((totalMovement + new Vector3(0, ySpeed, 0)) * Time.deltaTime);
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Store the normal of the surface we are hitting
        hitNormal = hit.normal;
    }


    void HandleGrounding()
    {
        // Reset states
        isGrounded = false;
        isSliding = false;

        Vector3 playerPos = transform.position;
        const float RAY_DISTANCE = 1.15f;
        const int RAY_COUNT = 16;
        const float ANGLE_STEP = 360f / RAY_COUNT;
        const float RAYCAST_RADIUS = 0.4f;

        RaycastHit shallowestHit = new RaycastHit();
        float shallowestAngle = 90f;

        // Raycase downward in a ring around the player
        for (int i = 0; i < RAY_COUNT; i++)
        {
            Vector3 rayOrigin = playerPos +
                               Quaternion.Euler(0, i * ANGLE_STEP, 0) *
                               Vector3.forward * RAYCAST_RADIUS;

            if (Physics.Raycast(rayOrigin, Vector3.down, out var hitInRing, RAY_DISTANCE, groundMask))
            {
                float hitAngle = Vector3.Angle(Vector3.up, hitInRing.normal);

                // If any of the rays hit flat ground, we aren't sliding and exit the loop
                if (hitAngle < characterController.slopeLimit)
                {
                    isGrounded = true;
                    isSliding = false;
                    break;
                }
                    
                // Check if we hit a steep slope and haven't hit a flat area yet, we use this slope and start sliding
                if (hitAngle > characterController.slopeLimit && hitAngle < 89)
                {
                    if (hitAngle < shallowestAngle)
                    {
                        shallowestAngle = hitAngle;
                        shallowestHit = hitInRing;
                    }

                    isGrounded = true;
                    isSliding = true;
                }
            }

            slopeHit = shallowestHit;
        }
    }

    private void HandleGroundingSpherecast()
    {
        isGrounded = false;
        isSliding = false;

        float sphereRadius = characterController.radius;

        Vector3 sphereOrigin = transform.position;

        float groundOffset = 0.2f;
        float castDistance = (characterController.height / 2f) - sphereRadius + groundOffset;

        if (Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out var hit, castDistance, groundMask))
        {

            isGrounded = true;

            float hitAngle = Vector3.Angle(Vector3.up, hit.normal);


            if (hitAngle > characterController.slopeLimit && hitAngle < 89f)
                {
                    isSliding = true;
                    slopeHit = hit;
                }
        }
    }


    private void SteepSlopeMovement()
    {
        // 1. Calculate the downhill direction based on the slope hit
        Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, slopeHit.normal).normalized;

        // 2. Apply gravity-based sliding
        slideAcceleration += -gravity * Time.deltaTime;
        slideVector = slopeDir * slideAcceleration;

        // 3. DEFLECTION LOGIC: Instead of setting movementVector to zero, 
        // we "clean" the movementVector so it doesn't push into the wall.

        // Get the direction the player is trying to move
        Vector3 currentInput = (transform.right * xMove + transform.forward * zMove).normalized * moveSpeed;

        // If we are moving TOWARD the wall (Dot product < 0), project the movement onto the wall plane
        if (Vector3.Dot(currentInput, hitNormal) < 0)
        {
            movementVector = Vector3.ProjectOnPlane(currentInput, hitNormal);
        }
        else
        {
            // If we are moving AWAY from the wall, allow full movement
            movementVector = currentInput;
        }
    }


    void PlayerHitsGround()
    {
        if (ySpeed < 0f) //slowly push player down so they keep in contact with the ground
        {
            ySpeed = -2f;
        }
    }


    void SetPlayerMoveSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && zMove == 1)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }




}
