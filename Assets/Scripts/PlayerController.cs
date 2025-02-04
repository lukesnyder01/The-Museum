using System.Collections;
using System.Collections.Generic;
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

    private Vector3 moveDirection;

    private float moveSpeed;
    private bool isGrounded;

    private bool hitHead;
    private Vector3 velocity;





    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        

        xMove = Input.GetAxis("Horizontal");
        zMove = Input.GetAxis("Vertical");


        SetPlayerMoveSpeed();


        isGrounded = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, groundMask);

        hitHead = Physics.CheckSphere(headCheckPosition.position, headCheckRadius, groundMask);


        if (isGrounded)
        {

            PlayerHitsGround();

            characterController.stepOffset = 0.1f;          //allows player to climb stairs
        }
        else
        {

        }



        moveDirection = transform.right * xMove * moveSpeed * lateralSprintSpeedPenalty + transform.forward * zMove * moveSpeed;




        if (hitHead && velocity.y > 0)
        {
            velocity.y = -0.1f;
        }

        if (!isGrounded)
        {
            characterController.stepOffset = 0.0001f;       //prevents player from catching on edges when jumping up near them
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += jumpForce;
        }


        //clamp maximum vertical speed from jumping
        if (velocity.y > jumpForce)
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime + velocity * Time.deltaTime);
    }



    void PlayerHitsGround()
    {

        if (velocity.y < 0f) //slowly push player down so they keep in contact with the ground
        {
            velocity.y = -2f;
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
