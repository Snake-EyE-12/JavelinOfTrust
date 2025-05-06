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
        if (context.performed) frameInput.jump.Press();
        if (context.canceled) frameInput.jump.Release();
    }

    public void OnAttackKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.attack.Press();
        if (context.canceled) frameInput.attack.Release();
    }

    public void OnDashKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.dash.Press();
        if (context.canceled) frameInput.dash.Release();
    }
    
    public void OnSprintKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.sprint.Press();
        if (context.canceled) frameInput.sprint.Release();
    }
    
    public void OnInteractKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.interact.Press();
        if (context.canceled) frameInput.interact.Release();
    }

    public void OnCrouchKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.crouch.Press();
        if (context.canceled) frameInput.crouch.Release();
    }
    
    public void OnGripKeyEvaluated(InputAction.CallbackContext context)
    {
        if (context.performed) frameInput.grip.Press();
        if (context.canceled) frameInput.grip.Release();
    }

    private void LateUpdate()
    {
        frameInput.jump.Reset();
        frameInput.dash.Reset();
        frameInput.attack.Reset();
        frameInput.grip.Reset();
        frameInput.interact.Reset();
        frameInput.sprint.Reset();
        frameInput.crouch.Reset();
    }

    public CharacterFrameInput GetFrameInput()
    {
        return frameInput;
    }
}

public class CharacterFrameInput
{
    public Vector2 direction;
    public BooleanInput jump = new BooleanInput();
    public BooleanInput dash = new BooleanInput();
    public BooleanInput attack = new BooleanInput();
    public BooleanInput grip = new BooleanInput();
    public BooleanInput interact = new BooleanInput();
    public BooleanInput sprint = new BooleanInput();
    public BooleanInput crouch = new BooleanInput();
}

[Flags]
public enum InputState
{
    Pressed = 1,
    Down = 2,
    Up = 4
}
public class BooleanInput
{
    private InputState state; // U D P 
    public bool Down => state.HasFlag(InputState.Down);
    public bool Up => state.HasFlag(InputState.Up);
    public bool Pressed => state.HasFlag(InputState.Pressed);
    public void Press() // Down Pressed
    {
        state = (InputState)3;
    }

    public void Release() // Up
    {
        state = (InputState)4;
    }

    public void Reset() 
    {
        state &= (InputState)1;
    }
}