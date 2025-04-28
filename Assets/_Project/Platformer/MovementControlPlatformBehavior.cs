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
                int movementDirection = PlayerBehaviorUtilities.Sign(data.input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
                {
                    data.HorizontalAcceleration = acceleration;
                }
            }

            [Prioritized]
            private void Decelerate()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
                {
                    data.HorizontalAcceleration = reversalAcceleration;
                }
            }
            
            [Prioritized]
            private void Coast()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.input.InputDirection.Direction.x);
                int velDirection = PlayerBehaviorUtilities.Sign(data.Velocity.X);
                if (movementDirection == 0 && velDirection != 0)
                {
                    data.Velocity.X *= coastalDamping;
                }
            }

            [Prioritized]
            private void Stop()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (PlayerBehaviorUtilities.Sign(data.input.InputDirection.Direction.x) == 0 && data.Velocity.X < stopThreshold && data.Velocity.X > -stopThreshold)
                {
                    data.Velocity.X = 0;
                    data.Acceleration.X = 0;
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

    public class SpeedControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class MaximumVelocityGimmick : Gimmick
        {
            [SerializeField] private bool applyInAir;
            [Prioritized]
            private void ClampSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.Velocity.X = Mathf.Clamp(data.Velocity.X, -data.MaxSpeed, data.MaxSpeed);
            }
        }

        [Serializable]
        public class LimitVelocityGimmick : Gimmick
        {
            [SerializeField] private bool applyInAir;
            [SerializeField] private float damping;

            [Prioritized]
            private void LimitSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (Mathf.Abs(data.Velocity.X) > data.MaxSpeed)
                {
                    data.Velocity.X *= (1 - damping * Time.deltaTime);
                }
            }

        }
        [SerializeField] private MaximumVelocityGimmick speedClamper;
        [SerializeField] private LimitVelocityGimmick speedLimiter;

        protected override void OnLoad()
        {
            Load(speedClamper);
            Load(speedLimiter);
        }
    }
    
    public class JumpControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class JumpApplicationGimmick : Gimmick
        {
            [SerializeField] private float burstVelocity;

            [Prioritized]
            private void Jump()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.ShouldJump)
                {
                    data.InJump = true;
                    data.JumpPoint.position = data.Transform.position;
                    data.JumpPoint.time = Time.time;
                    data.Acceleration.X = 0;
                    data.Velocity.Y = burstVelocity * data.JumpVelocityMultiplier;
                }
            }
        }
        [Serializable]
        public class ApexBoostGimmick : Gimmick
        {
            [SerializeField] private float yVelocityThreshold;
            [SerializeField] private float gravityMultiplier;
            [SerializeField] private float horizontalAccelerationMultiplier;

            [Prioritized]
            private void CalculateInApex()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.InApex = data.InJump && Mathf.Abs(data.Velocity.Y) < yVelocityThreshold;
            }

            [Prioritized]
            private void BoostAcceleration()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InApex)
                {
                    data.HorizontalAccelerationMultiplier *= horizontalAccelerationMultiplier;
                }
            }

            [Prioritized]
            private void AlterGravity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InApex)
                {
                    data.GravityMultiplier *= gravityMultiplier;
                }
            }
        }
        [Serializable]
        public class EarlyReleaseGimmick : Gimmick
        {
            [Prioritized]
            private void ZeroUpwardsVelocity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InJump && data.input.Jump.ended && data.Velocity.Y > 0)
                {
                    data.Velocity.Y = 0;
                }
            }
        }
        [Serializable]
        public class DescendingGravityGimmick : Gimmick
        {
            [SerializeField] private float multiplier;
            
            [Prioritized]
            private void AlterGravity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if(data.InJump && data.Velocity.Y < 0) data.GravityMultiplier *= multiplier;
            }
        }
        [Serializable]
        public class RoofDescendGimmick : Gimmick
        {
            [Prioritized]
            private void Descend()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if(data.InJump && data.RoofContact.EnteredContact && data.Velocity.Y > 0) data.Velocity.Y = 0;
            }
        }
        [Serializable]
        public class BufferJumpInputGimmick : Gimmick
        {
            [SerializeField] private float window;

            private float timeOfPress;
            
            [Prioritized]
            private void ListenToJumpInput()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.input.Jump.pressed)
                {
                    timeOfPress = Time.time;
                }
            }

            [Prioritized]
            private void UseBuffer()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (Time.time - timeOfPress < window && data.GroundContact.EnteredContact) data.ShouldJump = true;
            }
        }
        [Serializable]
        public class CoyoteTimeBufferGimmick : Gimmick
        {
            [SerializeField] private float window;

            [Prioritized]
            private void ListenToJumpInput()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (!data.InJump && data.input.Jump.pressed && Time.time - data.GroundContact.TimeOfContactExit < window)
                {
                    data.ShouldJump = true;
                }
            }
        }
        [Serializable]
        public class StickyFeetGimmick : Gimmick
        {
            [SerializeField] private float accelerationMultiplier;
            [SerializeField] private float velocityMultiplier;
            [SerializeField] private float duration;

            [Prioritized]
            private void StickToGround()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (Time.time - data.GroundContact.TimeOfGroundContact < duration)
                {
                    data.HorizontalAccelerationMultiplier *= accelerationMultiplier;
                    data.VelocityMultiplier *= velocityMultiplier;
                }
            }
        }
        [Serializable]
        public class AirControlGimmick : Gimmick
        {
            [SerializeField, Range(0, 1)] private float inputResponsiveness;
        }
        [Serializable]
        public class AirBreakGimmick : Gimmick
        {
            [SerializeField, Range(0, 1)] private float accelerationDamping;
        }
        [Serializable]
        public class AdjustInJumpTerminalFallSpeedGimmick : Gimmick
        {
            [SerializeField] private float maxFallSpeed;
        }
        [Serializable]
        public class ClipRoofCornerGimmick : Gimmick
        {
            [SerializeField] private CharacterCorrectionRay leftHeadAvoidance;
            [SerializeField] private CharacterCorrectionRay rightHeadAvoidance;
        }
        [Serializable]
        public class RunningJumpBoostGimmick : Gimmick
        {
            [SerializeField] private Curve boost;
        }
        [Serializable]
        public class MinimumJumpHeightBeforeReleaseGimmick : Gimmick
        {
            [SerializeField] private float height;
        }
        [Serializable]
        public class MultipleJumpCountGimmick : Gimmick
        {
            [SerializeField] private int count;
        }
        
        [SerializeField] private JumpApplicationGimmick jumpApplication;
        [SerializeField] private ApexBoostGimmick apexBoost;
        [SerializeField] private EarlyReleaseGimmick earlyRelease;
        [SerializeField] private ImmediateDescendGimmick immediateDescend;
        [SerializeField] private DescendingGravityGimmick descendingGravity;
        [SerializeField] private RoofDescendGimmick roofDescend;
        [SerializeField] private BufferJumpInputGimmick bufferJumpInput;
        [SerializeField] private CoyoteTimeBufferGimmick coyoteTimeBuffer;
        [SerializeField] private StickyFeetGimmick stickyFeet;
        [SerializeField] private AirControlGimmick airControl;
        [SerializeField] private AirBreakGimmick airBreak;
        [SerializeField] private AdjustInJumpTerminalFallSpeedGimmick adjustInJumpTerminalFallSpeed;
        [SerializeField] private ClipRoofCornerGimmick clipRoofCorner;
        [SerializeField] private RunningJumpBoostGimmick runningJumpBoost;
        [SerializeField] private MinimumJumpHeightBeforeReleaseGimmick minimumJumpHeightBeforeRelease;
        [SerializeField] private MultipleJumpCountGimmick multipleJumpCount;
        
        protected override void OnLoad()
        {
            
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
    
    public class FallControlPlatformBehavior : PlatformerPlayerBehavior
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