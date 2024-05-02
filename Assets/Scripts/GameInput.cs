using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//To handle the movement and interactions with Unity, remember installing the InputSystem from PackageManager

public class GameInput : MonoBehaviour {

    //public event EventHandler OnInteractAction;
    public event Action<InputAction.CallbackContext> OnInteractAction;
    public event Action<InputAction.CallbackContext> OnJumpAction;
    public event Action<InputAction.CallbackContext> OnRunAction;

    // Here we instantiate a playerInputActions to handle it with the Player, and just Enable() this on the Awake method.
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Run.performed += Run_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;

    }

    private void Jump_performed(InputAction.CallbackContext context) {
        OnJumpAction?.Invoke(context);
    }

    private void Run_performed(InputAction.CallbackContext context) {

        OnRunAction?.Invoke(context);

    }

    private void Interact_performed(InputAction.CallbackContext context) {

        OnInteractAction?.Invoke(context);

    }


    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool IsRunning() {

        bool returnValue;

        //return playerInputActions.Player.Run.ReadValue<>() != null;

        if (playerInputActions.Player.Run.ReadValue<float>() == 0) {
            returnValue = false;
        } else {
            returnValue = true;
        }



        return returnValue;
    }

}
