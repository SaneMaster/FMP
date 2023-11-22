using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching = false;

    bool lerpCrouch = false;
    bool sprinting = false;


    [Header("Crouching")]
    public float crouchTimer = 1;
    [Header("Movement")]
    public float speed = 7f;
    public float sprintSpeed = 15f;
    [Header("Sprinting")]
    public float sprintDuration = 5f; 
    private float sprintTimer = 0f;
    private bool isSprinting = false;
    [Header("Gravity")]
    public float gravity = -9.8f;
    [Header("Jump")]
    public float jumpHeight = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
        if (isSprinting)
        {
            sprintTimer += Time.deltaTime;

            if (sprintTimer >= sprintDuration)
            {
                isSprinting = false;
                speed = 7f; // Reset speed when sprint duration is reached
                sprintTimer = 0f; // Reset the timer
            }
        }

    }

    //public void ProcessMove(Vector2 input)
    //{
    //    Vector3 moveDirection = Vector3.zero;
    //    moveDirection.x = input.x;
    //    moveDirection.z = input.y;
    //    controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
    //    playerVelocity.y += gravity * Time.deltaTime;
    //    if (isGrounded && playerVelocity.y < 0)
    //        playerVelocity.y = -2f;
    //    controller.Move(playerVelocity * Time.deltaTime);
    //}

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = transform.TransformDirection(new Vector3(input.x, 0.0f, input.y)) * speed * Time.deltaTime;

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        // Combine movement and gravity adjustments into one controller.Move call
        controller.Move(moveDirection + playerVelocity * Time.deltaTime);

        // Reset downward velocity when grounded
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
    }



    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            speed = sprintSpeed; // Set sprint speed
        }
    }
}

