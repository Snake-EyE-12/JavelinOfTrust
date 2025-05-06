using System;
using UnityEngine;

namespace CharacterProcess
{
    public abstract class CharacterProcessor2 : OperationalProcessor<CharacterData2>
    {
    }

    public class TesterGuy : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            Debug.Log("Velocity: (" + data.Velocity.Value + ") | Acceleration: (" + data.Acceleration.Additive + " * " + data.Acceleration.Multiplicative + ")");
        }
    }

    public class InputSetter : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.Input = data.InputSystem.GetFrameInput();
        }
    }

    public class PhysicsVelocityApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingPhysicsEuler) return;
            data.RigidBody.linearVelocity = data.Velocity.Value;
        }
    }

    public class LocomotionWalkDetector : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.ActiveLocomotion = data.WalkLocomotion;
        }
    }
    public class LocomotionAirDetector : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpUniqueAirControl) return;
            if (!data.GroundContact.Contact) data.ActiveLocomotion = data.AirLocomotion;
        }
    }

    public class LocomotionCrouchDetector : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingCrouch) return;
            if (data.Input.crouch.Pressed) data.ActiveLocomotion = data.CrouchLocomotion;
        }
    }
    
    public class LocomotionSprintDetector : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingSprint) return;
            if (data.Input.crouch.Pressed) data.ActiveLocomotion = data.CrouchLocomotion;
        }
    }
    
    public class GravityToAcceleration : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingGravity) return;
            data.Acceleration.Additive.y += data.CalculatedGravity.y;
            data.CalculatedGravity.Reset();
        }
    }

    public class AccelerationApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.Velocity.Value += data.Acceleration * Time.deltaTime;
            data.Acceleration.Reset();
        }
    }
    
    public class ProcessLocomotionForwardAcceleration : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            int velDirection = MathUtils.Sign(data.Velocity.Value.x);

            if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
            {
                data.Acceleration.Additive.x += data.ActiveLocomotion.Acceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionReversalAcceleration : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            int velDirection = MathUtils.Sign(data.Velocity.Value.x);

            if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
            {
                data.Acceleration.Additive.x += data.ActiveLocomotion.ReversalAcceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionCoasting : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            int velDirection = MathUtils.Sign(data.Velocity.Value.x);

            if (movementDirection == 0 && velDirection != 0)
            {
                float decay = Mathf.Pow(1f - data.ActiveLocomotion.CoastDamping, Time.deltaTime);
                data.Velocity.Value.x *= decay;
            }
        }
    }

    public class ProcessVelocityCutOffImmediateStop : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (MathUtils.Sign(data.Input.direction.x) == 0 && 
                Mathf.Abs(data.Velocity.Value.x) < data.HorizontalCutOffVelocity)
            {
                data.Velocity.Value.x = 0;
                data.Acceleration.Additive.x = 0;
            }
        }
    }

    public class GravityApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingGravity) return;
            data.CalculatedGravity.Additive.y -= data.Gravity;
        }
    }

    public class GroundCollisionVelocityZeroer : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.GroundContact.Contact && data.Velocity.Value.y < 0) data.Velocity.Value.y = 0;
        }

    }

    public class WallCollisionVelocityZeroer : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.LeftWallContact.Contact && data.Velocity.Value.x < 0) data.Velocity.Value.x = 0;
            if (data.RightWallContact.Contact && data.Velocity.Value.x > 0) data.Velocity.Value.x = 0;
        }

    }
    
    public class RoofCollisionDetection : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.RoofContact.Update(data.RigidBody.transform);
        }
    }
    
    public class GroundCollisionDetection : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.GroundContact.Update(data.RigidBody.transform);
        }
    }

    public class WallCollisionDetection : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.LeftWallContact.Update(data.RigidBody.transform);
            data.RightWallContact.Update(data.RigidBody.transform);
        }
    }

    public class TerminalFallSpeedCalculator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingTerminalVelocity) return;
            data.CalculatedMaxFallSpeed = data.TerminalVelocity;
        }
    }
    public class FallSpeedClamper : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.Velocity.Value.y < -data.CalculatedMaxFallSpeed) data.Velocity.Value.y = -data.CalculatedMaxFallSpeed;
        }
    }

    public class LocomotionSpeedClamper : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingSpeedClamp) return;
            if (Mathf.Abs(data.Velocity.Value.x) > data.ActiveLocomotion.MaxSpeed)
            {
                int movementDirection = MathUtils.Sign(data.Input.direction.x);
                data.Velocity.Value.x = data.ActiveLocomotion.MaxSpeed * movementDirection;
            }
        }
    }
    
    public class LocomotionSpeedLimiter : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingSpeedLimiter) return;
            float xMag = Mathf.Abs(data.Velocity.Value.x);
            if (xMag > data.ActiveLocomotion.MaxSpeed)
            {
                float decay = Mathf.Pow(1f - data.Damping, Time.deltaTime);
                data.Velocity.Value.x *= decay;
            }
        }
    }

    public class JumpBufferListener : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpInputBuffer) return;
            if (data.Input.jump.Down) data.TimeOfJumpPressed = Time.time;
        }
    }

    public class JumpBufferApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpInputBuffer) return;
            if (data.GroundContact.EnteredContact && data.TimeOfJumpPressed + data.JumpBufferTime > Time.time)
            {
                if (data.JumpBuffersTaps || data.Input.jump.Pressed) data.ShouldJump = true;
            }
        }
    }

    public class GroundedJumpApplicator : CharacterProcessor2 //actually not needed with jump buffer, but who know if that is added
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.Input.jump.Down && data.GroundContact.Contact) data.ShouldJump = true;
        }
    }

    public class GroundContactJumpResetter : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.GroundContact.Contact) data.InJump = false;
        }
    }

    public class JumpCoyoteInformant : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.Input.jump.Down && data.GroundContact.TimeOfContactExit + data.JumpCoyoteTime > Time.time)
            {
                data.ShouldJump = true;
            }
        }
    
    }

    public class CalculateJumpForce : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.CalculatedJumpForce.Value.x = 0;
            data.CalculatedJumpForce.Value.y = data.JumpBurstForce;
        }
    }

    public class CalculateRunningJumpForce : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpRunningStart) return;
            data.CalculatedJumpForce.Value *= data.RunningStartBonusVelocity.Evaluate(Mathf.Abs(data.Velocity.Value.x));
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            if (movementDirection != 0)
            {
                data.CalculatedJumpForce.Value = Quaternion.Euler(0, 0, -data.RunningStartBoostAngle * movementDirection) * data.CalculatedJumpForce.Value;
            }
        }
    }

    public class AttemptJumpApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.ShouldJump)
            {
                data.InJump = true;
                data.JumpTakeoffPoint.point = data.RigidBody.transform.position;
                data.JumpTakeoffPoint.time = Time.time;
                data.Velocity.Value += data.CalculatedJumpForce.Value;
            }
        }
    }

    public class GoingToJumpMovementAdjuster : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpPreviousVelocityAlteration) return;
            if (data.ShouldJump)
            {
                data.Velocity.Value *= data.JumpingPreviousVelocityMultiplier;
                data.Acceleration.Multiplicative *= data.JumpingPreviousAccelerationMultiplier;
            }
        }
    }

    public class JumpingEvaluationResetter : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.ShouldJump = false;
        }
    }


    public class GravityJumpDescendAccelerator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpStrongerGravityDescend) return;
            if (data.InJump && data.Velocity.Value.y < 0) data.CalculatedGravity.Multiplicative.y *= data.DescendingGravityMultiplier;
        }
    }

    public class ApexCalculator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            data.InApex = data.InJump && Mathf.Abs(data.Velocity.Value.y) < data.ApexYVelocityThreshold;
        }
    }

    public class ApexXVelocityApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            if (data.InApex)
            {
                data.Acceleration.Multiplicative.x *= data.ApexHorizontalBonusAcceleration * MathUtils.Sign(data.Input.direction.x);
            }
        }
    }

    public class ApexAntiGravityMultiplierApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            if (data.InApex) data.CalculatedGravity.Multiplicative.y *= data.ApexGravityMultiplier;
        }
    }

    public class EarlyReleaseGravityMultiplierApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!(data.IsUsingJumpEarlyRelease && data.IsUsingJumpEarlyReleaseGravityMultiplier)) return;
            if (data.EarlyOutJump)
            {
                data.CalculatedGravity.Multiplicative.y *= data.EarlyReleaseGravityMultiplier;
            }
        }
    }

    public class EarlyReleaseCalculator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingJumpEarlyRelease) return;
            if (data.Input.jump.Up && data.InJump) data.JumpEarlyRelease = true;
            if (data.GroundContact.Contact) data.JumpEarlyRelease = false;
        }
    }

    public class RoofCollisionVelocityZeroer : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.RoofContact.Contact && data.Velocity.Value.y > 0) data.Velocity.Value.y = 0;
        }
    }


    public class GapPositionCorrector : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingAdjustmentLedgeLander) return;
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            if (data.Velocity.Value.y < 0 && !data.GroundContact.Contact)
            {
                if (movementDirection == -1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftLedgeDetector.origin, data.LeftLedgeDetector.direction, data.LeftLedgeDetector.direction.magnitude, data.LeftLedgeDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftLedgeDetector.origin + (Vector3)data.LeftLedgeDetector.correction, data.LeftLedgeDetector.direction, data.LeftLedgeDetector.direction.magnitude, data.LeftLedgeDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.LeftLedgeDetector.correction;
                }
                else if (movementDirection == 1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightLedgeDetector.origin, data.RightLedgeDetector.direction, data.RightLedgeDetector.direction.magnitude, data.RightLedgeDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightLedgeDetector.origin + (Vector3)data.RightLedgeDetector.correction, data.RightLedgeDetector.direction, data.RightLedgeDetector.direction.magnitude, data.RightLedgeDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightLedgeDetector.correction;
                }
            }
        }

    }

    public class HeadCollisionAvoidance : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingAdjustmentAscendingRoofCornerClip) return;
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            if (data.InJump && !data.JumpEarlyRelease)
            {
                if (movementDirection != -1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftRoofDetector.origin, data.LeftRoofDetector.direction, data.LeftRoofDetector.direction.magnitude, data.LeftRoofDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)(data.LeftRoofDetector.origin + data.LeftRoofDetector.correction), data.LeftRoofDetector.direction, data.LeftRoofDetector.direction.magnitude, data.LeftRoofDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightRoofDetector.correction;
                }
                else if (movementDirection != 1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightRoofDetector.origin, data.RightRoofDetector.direction, data.RightRoofDetector.direction.magnitude, data.RightRoofDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)(data.RightRoofDetector.origin + data.RightRoofDetector.correction), data.RightRoofDetector.direction, data.RightRoofDetector.direction.magnitude, data.RightRoofDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightRoofDetector.correction;
                }
            }
        }

    }

    public class LandingFrictionApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingLandingStickyFeet) return;
            float timeInStickyFeet = Time.time - data.GroundContact.TimeOfContactEnter;
            if (timeInStickyFeet < data.StickyFeetDuration)
            {
                data.Acceleration.Multiplicative *= data.StickyFeetAccelerationTimeToMultiplier.Evaluate(timeInStickyFeet);
                data.Velocity.Value *= data.StickyFeetVelocityTimeToMultiplier.Evaluate(timeInStickyFeet);
            }
        }

    }
    
    public class LandingDirectionLockApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingLandingDirectionLock) return;
            float timeInLock = Time.time - data.GroundContact.TimeOfContactEnter;
            if (timeInLock < data.LandingDirectionLockDuration)
            {
                int movementDirection = MathUtils.Sign(data.Input.direction.x);
                int velocityDirection = MathUtils.Sign(data.Velocity.Value.x);
                if (movementDirection != 0 && movementDirection != velocityDirection) //Question if vel is 0
                {
                    float decay = Mathf.Pow(1f - data.LockedOtherDirectionDamper, Time.deltaTime);
                    data.Velocity.Value.x *= decay;
                }
            }
        }
    }

    public class ApexHeightCalculator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if(data.PeakHeight < data.RigidBody.transform.position.y) data.PeakHeight = data.RigidBody.transform.position.y;
        }
    }

    public class FallHeightCalculator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (data.GroundContact.EnteredContact)
            {
                data.FallHeight = data.PeakHeight - data.RigidBody.transform.position.y;
                data.PeakHeight = float.MinValue;
            }
        }
    }

    public class LandingStunApplicator : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!data.IsUsingLandingStun) return;
            float timeInStun = Time.time - data.GroundContact.TimeOfContactEnter;
            if (timeInStun < data.LandingStunHeightToDuration.Evaluate(timeInStun))
            {
                data.Acceleration.Additive.x = 0;
            }
        }
    }

    public class ImmediateDescendOnReleaseJump : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            if (!(data.IsUsingJumpEarlyRelease && data.IsUsingJumpImmediateDescend)) return;
            if (data.EarlyOutJump && data.Velocity.Value.y > 0)
            {
                data.Velocity.Value.y = 0;
            }
        }
    }

    public class EnforceMinimumJumpHeight : CharacterProcessor2
    {
        public override void Operate(CharacterData2 data)
        {
            data.EarlyOutJump = data.JumpEarlyRelease;
            if (!data.IsUsingJumpMinimumHeight) return;
            data.EarlyOutJump = data.JumpEarlyRelease && data.RigidBody.transform.position.y - data.JumpTakeoffPoint.point.y > data.MinimumJumpHeight;
        }
    }
    
    
    
    
    
    
    
    

    public class CalculateWithinJumpArc : CharacterProcessor
    {
        public override void Process(CharacterData data)
        {
            data.inJumpArc = data.inJump && data.transform.position.y > data.startingJumpPoint.y;

            base.Process(data);
        }
    }
}