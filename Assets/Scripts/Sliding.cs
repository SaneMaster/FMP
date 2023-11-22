using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj;
    private CharacterController controller;
    private PlayerMovement pm;

    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideHeight; // Adjust this value based on your needs
    private float startHeight;

    public KeyCode slideKey = KeyCode.C;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        pm = GetComponent<PlayerMovement>();

        startHeight = controller.height;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey) && sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        sliding = true;
        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 forwardDirection = orientation.forward;
        Vector3 moveDirection = Vector3.ProjectOnPlane(forwardDirection, Vector3.up).normalized;

        controller.Move(moveDirection * slideForce * Time.deltaTime);

        slideTimer -= Time.deltaTime;

        // Smoothly change the character controller height during the slide
        float t = 1f - slideTimer / maxSlideTime;
        controller.height = Mathf.Lerp(startHeight, slideHeight, t);

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        sliding = false;

        // Restore the character controller height to its original value
        controller.height = startHeight;
    }
}



