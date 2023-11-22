using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    
    private PlayerMove move;
    private PlayerLook look;

    

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        move = GetComponent<PlayerMove>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => move.Jump();  // call back context used to call jump function
        onFoot.Crouch.performed += ctx => move.Crouch();
        onFoot.Sprint.performed += ctx => move.Sprint();
    }
    
    

    
    void FixedUpdate()
    {
        move.ProcessMove(onFoot.Movement.ReadValue<Vector2>());   // tells the player to move using value from input movement action
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
