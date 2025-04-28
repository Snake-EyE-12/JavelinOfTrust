using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController.Platformer
{
    public class InputControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class InputGimmick : Gimmick
        {
            public CharacterFrameInput frameInput { get; set; }
            public InputGimmick(CharacterFrameInput frameInput)
            {
                this.frameInput = frameInput;
            }
            [Prioritized]
            private void GatherInput()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.input = frameInput;
            }

            [Prioritized]
            private void ResetInput()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.input.Reset();
            }
        }
        public void OnDirectionEvaluated(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            input.frameInput.InputDirection.Direction = direction;
        }
    
        public void OnJumpKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Jump.Press();
            if (context.canceled) input.frameInput.Jump.Release();
        }
    
        public void OnAttackKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Attack.Press();
            if (context.canceled) input.frameInput.Attack.Release();
        }
    
        public void OnDashKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Dash.Press();
            if (context.canceled) input.frameInput.Dash.Release();
        }
        
        public void OnSprintKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Sprint.Press();
            if (context.canceled) input.frameInput.Sprint.Release();
        }
        
        public void OnInteractKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Interact.Press();
            if (context.canceled) input.frameInput.Interact.Release();
        }
    
        public void OnCrouchKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Crouch.Press();
            if (context.canceled) input.frameInput.Crouch.Release();
        }
        
        public void OnGripKeyEvaluated(InputAction.CallbackContext context)
        {
            if (context.performed) input.frameInput.Grip.Press();
            if (context.canceled) input.frameInput.Grip.Release();
        }

        [SerializeField] private InputGimmick input;
        protected override void OnLoad()
        {
            Load(input);
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

        public void Reset()
        {
            Jump.Reset();
            Dash.Reset();
            Attack.Reset();
            Grip.Reset();
            Interact.Reset();
            Sprint.Reset();
            Crouch.Reset();
        }
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