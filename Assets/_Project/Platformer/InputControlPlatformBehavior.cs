using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController.Platformer
{
    public class InputControlPlatformBehavior : PlatformerPlayerBehavior
    {
        private CharacterFrameInput frameInput = new CharacterFrameInput();
        public void OnDirectionEvaluated(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            frameInput.InputDirection.Direction = direction;
        }
    
        public void OnJumpKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Jump.Press();
            if (context.canceled) frameInput.Jump.Release();
        }
    
        public void OnAttackKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Attack.Press();
            if (context.canceled) frameInput.Attack.Release();
        }
    
        public void OnDashKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Dash.Press();
            if (context.canceled) frameInput.Dash.Release();
        }
        
        public void OnSprintKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Sprint.Press();
            if (context.canceled) frameInput.Sprint.Release();
        }
        
        public void OnInteractKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Interact.Press();
            if (context.canceled) frameInput.Interact.Release();
        }
    
        public void OnCrouchKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Crouch.Press();
            if (context.canceled) frameInput.Crouch.Release();
        }
        
        public void OnGripKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) frameInput.Grip.Press();
            if (context.canceled) frameInput.Grip.Release();
        }
    
        private void LateUpdate()
        {
            frameInput.Jump.Reset();
            frameInput.Dash.Reset();
            frameInput.Attack.Reset();
            frameInput.Grip.Reset();
            frameInput.Interact.Reset();
            frameInput.Sprint.Reset();
            frameInput.Crouch.Reset();
        }
        protected override void OnLoad()
        {
            // if(blackboard == null) Debug.Log("B");
            // if(frameInput == null) Debug.Log("F");
            // if(frameInput.InputDirection == null) Debug.Log("I");
            // if(frameInput.InputDirection.Direction == null) Debug.Log("D");
            //Do(() => blackboard.SetVariable("DirectionInput", frameInput.InputDirection.Direction), 1);
        }
    }
    
    public class CharacterFrameInput
    {
        public InputDirection InputDirection { get; set; } = new InputDirection();
        public InputBoolean Jump { get; set; } = new InputBoolean();
        public InputBoolean Dash { get; set; } = new InputBoolean();
        public InputBoolean Attack { get; set; } = new InputBoolean();
        public InputBoolean Grip { get; set; } = new InputBoolean();
        public InputBoolean Interact { get; set; } = new InputBoolean();
        public InputBoolean Sprint { get; set; } = new InputBoolean();
        public InputBoolean Crouch { get; set; } = new InputBoolean();
    }


    public struct InputBoolean
    {
        public void Press()
        {
            started = true;
            pressed = true;
        }

        public void Release()
        {
            ended = true;
            pressed = false;
        }

        public void Reset()
        {
            started = false;
            ended = false;
        }

        public bool started;
        public bool ended;
        public bool pressed;
    }

    public class InputDirection
    {
        public Vector2 LastNonZeroDirection { get; set; }
        private Vector2 direction;
        public Vector2 Direction
        {
            get { return direction;} 
            set { if(value != Vector2.zero) LastNonZeroDirection = value; direction = value;} 
        }
    }
}