using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    private CharacterFrameInput frameInput = new CharacterFrameInput();
    public void OnDirectionEvaluated(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        frameInput.direction = direction;
    }

    public void OnJumpKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.jumpStarted = true;
        if (context.canceled) frameInput.jumpEnded = true;
    }

    public void OnAttackKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.attackStarted = true;
        if (context.canceled) frameInput.attackEnded = true;
    }

    public void OnDashKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.dashStarted = true;
        if (context.canceled) frameInput.dashEnded = true;
    }
    
    public void OnSprintKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.sprintStarted = true;
        if (context.canceled) frameInput.sprintEnded = true;
    }
    
    public void OnInteractKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.interactStarted = true;
        if (context.canceled) frameInput.interactEnded = true;
    }

    public void OnCrouchKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.sprintStarted = true;
        if (context.canceled) frameInput.sprintEnded = true;
    }
    
    public void OnGripKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.gripStarted = true;
        if (context.canceled) frameInput.gripEnded = true;
    }

    private void LateUpdate()
    {
        frameInput.jumpStarted = false;
        frameInput.jumpEnded = false;
        frameInput.attackStarted = false;
        frameInput.attackEnded = false;
        frameInput.dashStarted = false;
        frameInput.dashEnded = false;
        frameInput.sprintStarted = false;
        frameInput.sprintEnded = false;
        frameInput.interactStarted = false;
        frameInput.interactEnded = false;
        frameInput.crouchStarted = false;
        frameInput.crouchEnded = false;
    }

    public CharacterFrameInput GetFrameInput()
    {
        return frameInput;
    }
}

public struct CharacterFrameInput
{
    public Vector2 direction;
    public bool jumpStarted;
    public bool jumpEnded;
    public bool dashStarted;
    public bool dashEnded;
    public bool attackStarted;
    public bool attackEnded;
    public bool gripStarted;
    public bool gripEnded;
    public bool interactStarted;
    public bool interactEnded;
    public bool sprintStarted;
    public bool sprintEnded;
    public bool crouchStarted;
    public bool crouchEnded;
}