using System;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class MovementControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class AccelerationGimmick : Gimmick
        {
            [SerializeField, Min(0)] private float acceleration;
            [SerializeField, Min(0)] private float reversalAcceleration;
            [SerializeField, Min(0)] private float coastalDamping;
            [SerializeField, Min(0)] private float stopThreshold;
            [SerializeField] private float maxSpeed;

            [Prioritized]
            private void Accelerate()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
                {
                    data.Acceleration.@base.x = acceleration * Mathf.Sign(movementDirection);
                    if(data.Velocity.X > 100f) Debug.Log("Warning");
                }
            }

            [Prioritized]
            private void Decelerate()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
                {
                    data.Acceleration.@base.x = reversalAcceleration * Mathf.Sign(movementDirection);
                    if(data.Velocity.X > 100f) Debug.Log("Warning");
                }
            }
            
            [Prioritized]
            private void Coast()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection == 0 && velDirection != 0)
                {
                    data.Velocity.multiplier.x *= (1 - coastalDamping * Time.deltaTime);
                    if(data.Velocity.X > 100f) Debug.Log("Warning");
                }
            }

            [Prioritized]
            private void Stop()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x) == 0 && data.Velocity.X < stopThreshold && data.Velocity.X > -stopThreshold)
                {
                    data.Velocity.additive.x = 0;
                    data.Acceleration.additive.x = 0;
                    if(data.Velocity.X > 100f) Debug.Log("Warning");
                }
            }

            [Prioritized]
            private void OverrideMaxSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.MaxSpeed = maxSpeed;
            }
        }

        [Serializable]
        public class VelocityMovementGimmick : Gimmick
        {
            [SerializeField, Min(0)] private float speed;
            [SerializeField, Min(0)] private bool normalized;

            [Prioritized]
            private void Move()
            {

            }
        }
        

        

        [SerializeField] private VelocityMovementGimmick snapMovement;
        [SerializeField] private AccelerationGimmick walkAcceleration;
        [SerializeField] private AccelerationGimmick sprintingAcceleration;
        [SerializeField] private AccelerationGimmick crouchingAcceleration;

        protected override void OnLoad()
        {
            Load(snapMovement);
            Load(walkAcceleration);
            Load(sprintingAcceleration);
            Load(crouchingAcceleration);
        }
    }

    public class RollControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class DashControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class SlideControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class GlideControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }

    public class ClimbControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class SwingControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class SwimControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class WallControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
    
    public class LedgeControlPlatformBehavior : PlatformerPlayerBehavior
    {
        protected override void OnLoad()
        {
            throw new NotImplementedException();
        }
    }
}