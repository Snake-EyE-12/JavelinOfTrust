using System;
using UnityEngine;

namespace CharacterController.Platformer
{
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
                    data.ShouldJump = false;
                    data.JumpPoint.position = data.Transform.position;
                    data.JumpPoint.time = Time.time;
                    data.Acceleration.additive.y = 0;
                    data.Velocity.additive.y = burstVelocity * data.JumpVelocityMultiplier;
                    data.PerformedJumps++;
                }
            }

            [Prioritized]
            private void LandingReset()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InJump && data.GroundContact.Contact)
                {
                    data.InJump = false;
                    data.PerformedJumps = 0;
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
                    data.Acceleration.multiplier.x *= horizontalAccelerationMultiplier;
                }
            }

            [Prioritized]
            private void AlterGravity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InApex)
                {
                    data.Acceleration.multiplier.y *= gravityMultiplier;
                }
            }
        }
        [Serializable]
        public class EarlyReleaseGimmick : Gimmick
        {
            [SerializeField] private float minimumHeight;
            private bool releasedJumpKeyInJump;

            [Prioritized]
            private void ListenToRelease()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InJump && data.Input.Jump.ended && data.Velocity.Y > 0 && !releasedJumpKeyInJump)
                {
                    releasedJumpKeyInJump = true;
                }
            }
            
            [Prioritized]
            private void ActivateEarlyRelease()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (releasedJumpKeyInJump && data.Transform.position.y - data.JumpPoint.position.y > minimumHeight)
                {
                    data.EarlyRelease = true;
                    data.Velocity.additive.y = 0;
                    releasedJumpKeyInJump = false;
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
                if(data.InJump && data.Velocity.Y < 0) data.Acceleration.multiplier.y *= multiplier;
            }
        }
        [Serializable]
        public class RoofDescendGimmick : Gimmick
        {
            [Prioritized]
            private void Descend()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if(data.InJump && data.RoofContact.EnteredContact && data.Velocity.Y > 0) data.Velocity.additive.y = 0;
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
                if (data.Input.Jump.pressed)
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
                if (!data.InJump && data.Input.Jump.pressed && Time.time - data.GroundContact.TimeOfContactExit < window)
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
                if (Time.time - data.GroundContact.TimeOfContactEnter < duration)
                {
                    data.Acceleration.multiplier.x *= accelerationMultiplier;
                    data.Velocity.multiplier.x *= velocityMultiplier;
                }
            }
        }
        [Serializable]
        public class AirControlGimmick : Gimmick
        {
            [SerializeField, Range(0, 1)] private float inputResponsiveness;

            [Prioritized]
            private void AlterInputMagnitude()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.Acceleration.@base *= inputResponsiveness;
            }
        }
        [Serializable]
        public class AirBreakGimmick : Gimmick
        {
            [SerializeField, Range(0, 1)] private float velocityDamping;

            [Prioritized]
            private void BreakInAir()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x);
                if (!data.GroundContact.Contact && movementDirection == 0)
                {
                    data.Velocity.multiplier.x *= velocityDamping;
                }
            }
        }
        [Serializable]
        public class AdjustInJumpTerminalFallSpeedGimmick : Gimmick
        {
            [SerializeField] private float maxFallSpeed;

            [Prioritized]
            private void CalculateInJumpParabola()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.InJumpArc = data.InJump && data.Transform.position.y > data.JumpPoint.position.y;
            }

            [Prioritized]
            private void AlterFallSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.InJumpArc) data.maxFallSpeed = maxFallSpeed;
            }
        }
        [Serializable]
        public class ClipRoofCornerGimmick : Gimmick
        {
            [SerializeField] private CharacterCorrectionRay leftHeadAvoidance;
            [SerializeField] private CharacterCorrectionRay rightHeadAvoidance;

            [Prioritized]
            private void ClipRoofCorner()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                int movementDirection = PlayerBehaviorUtilities.Sign(data.Input.InputDirection.Direction.x);
                if (data.InJump && data.EarlyRelease)
                {
                    
                    if (movementDirection != -1 && Physics2D.RaycastAll(data.Transform.position + (Vector3)leftHeadAvoidance.origin, leftHeadAvoidance.direction, leftHeadAvoidance.direction.magnitude).Length > 0 &&
                        Physics2D.RaycastAll(data.Transform.position + (Vector3)(leftHeadAvoidance.origin + leftHeadAvoidance.correction), leftHeadAvoidance.direction, leftHeadAvoidance.direction.magnitude).Length == 0)
                    {
                        data.Transform.position += (Vector3)leftHeadAvoidance.correction;
                    }
                    else if (movementDirection != 1 && Physics2D.RaycastAll(data.Transform.position + (Vector3)rightHeadAvoidance.origin, rightHeadAvoidance.direction, rightHeadAvoidance.direction.magnitude).Length > 0 && 
                             Physics2D.RaycastAll(data.Transform.position + (Vector3)(rightHeadAvoidance.origin + rightHeadAvoidance.correction), rightHeadAvoidance.direction, rightHeadAvoidance.direction.magnitude).Length == 0)
                    { 
                        data.Transform.position += (Vector3)rightHeadAvoidance.correction;
                    }
                }
            }
        }
        [Serializable]
        public class RunningJumpBoostGimmick : Gimmick
        {
            [SerializeField] private Curve boost;

            [Prioritized]
            private void BoostJumpHeight()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if(data.ShouldJump) data.JumpVelocityMultiplier = boost.Evaluate(data.Velocity.X);
            }
        }
        [Serializable]
        public class MultipleJumpCountGimmick : Gimmick
        {
            [SerializeField, Min(2)] private int count;

            [Prioritized]
            private void ExtraJump()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if(data.PerformedJumps < count && data.InJump) data.ShouldJump = true;
            }
        }
        
        [SerializeField] private JumpApplicationGimmick jumpApplication;
        [SerializeField] private ApexBoostGimmick apexBoost;
        [SerializeField] private EarlyReleaseGimmick earlyRelease;
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
        [SerializeField] private MultipleJumpCountGimmick multipleJumpCount;
        
        protected override void OnLoad()
        {
            Load(jumpApplication);
            Load(apexBoost);
            Load(earlyRelease);
            Load(descendingGravity);
            Load(roofDescend);
            Load(bufferJumpInput);
            Load(coyoteTimeBuffer);
            Load(stickyFeet);
            Load(airControl);
            Load(airBreak);
            Load(adjustInJumpTerminalFallSpeed);
            Load(clipRoofCorner);
            Load(runningJumpBoost);
            Load(multipleJumpCount);
        }
    }
}