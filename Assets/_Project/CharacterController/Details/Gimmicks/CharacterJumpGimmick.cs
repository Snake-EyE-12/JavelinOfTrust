using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterJumpGimmick : ICharacterGimmick //Bunny Hop
    {
        [SerializeField, Min(0)] public float JumpBurstForce;
        [SerializeField] public bool IsUsingJumpApexBonus;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexYVelocityThreshold;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexHorizontalBonusAcceleration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexGravityMultiplier;
        [SerializeField] public bool IsUsingJumpEarlyRelease;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpEarlyReleaseGravityMultiplier;
        [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingJumpEarlyRelease), nameof(IsUsingJumpEarlyReleaseGravityMultiplier))] public float EarlyReleaseGravityMultiplier;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpImmediateDescend;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpMinimumHeight;
        [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingJumpEarlyRelease), nameof(IsUsingJumpMinimumHeight))] public float MinimumJumpHeight;
        [SerializeField] public bool IsUsingJumpStrongerGravityDescend;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpStrongerGravityDescend))] public float DescendingGravityMultiplier;
        [SerializeField] public bool IsUsingJumpInputBuffer;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpInputBuffer))] public float JumpBufferTime;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpInputBuffer))] public bool JumpBuffersTaps;
        [SerializeField] public bool IsUsingJumpCoyoteTime;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpCoyoteTime))] public float JumpCoyoteTime;
        [SerializeField] public bool IsUsingJumpUniqueAirControl;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpUniqueAirControl))] public CharacterLocomotionState AirLocomotion;
        [SerializeField] public bool IsUsingJumpArcTerminalFall;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpArcTerminalFall))] public float JumpArcTerminalVelocity;
        [SerializeField] public bool IsUsingJumpRunningStart;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpRunningStart))] public float RunningStartBoostAngle;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpRunningStart))] public Curve RunningStartBonusVelocity;
        [SerializeField] public bool IsUsingMultipleJumps;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingMultipleJumps))] public int MultipleJumpCount;
        //[SerializeField] public bool IsUsingJumpChargeUp;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpChargeUp))] public Curve ChargePowerCurve;
        [SerializeField] public bool IsUsingJumpPreviousVelocityAlteration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousVelocityMultiplier;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousAccelerationMultiplier;
        /**/
        [NonSerialized] public bool InApex;
        [NonSerialized] public float TimeOfJumpPressed;
        [NonSerialized] public bool JumpEarlyRelease;
        [NonSerialized] public bool EarlyOutJump;
        [NonSerialized] public bool ShouldJump;
        [NonSerialized] public bool InJump;
        [NonSerialized] public JumpPoint JumpTakeoffPoint = new JumpPoint();
        [NonSerialized] public SVector2 CalculatedJumpForce = new SVector2();
        [NonSerialized] public bool InJumpArc;
        [NonSerialized] public int JumpCount;
    }

    public class JumpCountUser : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingMultipleJumps) return;
            if (data.input.Input.jump.Down && data.jump.JumpCount < data.jump.MultipleJumpCount)
            {
                data.jump.ShouldJump = true;
                data.jump.InApex = false;
                data.jump.EarlyOutJump = false;
                data.jump.JumpEarlyRelease = false;
            }
        }
    }
    public class JumpCounterResetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if(data.collision.GroundContact.EnteredContact) data.jump.JumpCount = 0;
        }
    }

    public class JumpArcDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.jump.InJumpArc = data.physics.RigidBody.transform.position.y > data.jump.JumpTakeoffPoint.point.y && data.jump.InJump;
        }
    }
    public class JumpArcTerminalFall : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpArcTerminalFall) return;
            if (data.jump.InJumpArc)
            {
                data.gravity.CalculatedMaxFallSpeed = data.jump.JumpArcTerminalVelocity;
            }
        }
    }
    
    public class JumpBufferListener : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpInputBuffer) return;
            if (data.input.Input.jump.Down) data.jump.TimeOfJumpPressed = Time.time;
        }
    }

    public class JumpBufferApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpInputBuffer) return;
            if (data.collision.GroundContact.EnteredContact && data.jump.TimeOfJumpPressed + data.jump.JumpBufferTime > Time.time)
            {
                if (data.jump.JumpBuffersTaps || data.input.Input.jump.Pressed) data.jump.ShouldJump = true;
            }
        }
    }

    public class GroundedJumpApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.input.Input.jump.Down && data.collision.GroundContact.Contact) data.jump.ShouldJump = true;
        }
    }

    public class GroundContactJumpResetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.collision.GroundContact.Contact) data.jump.InJump = false;
        }
    }

    public class JumpCoyoteInformant : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.input.Input.jump.Down && data.collision.GroundContact.TimeOfContactExit + data.jump.JumpCoyoteTime > Time.time)
            {
                data.jump.ShouldJump = true;
            }
        }
    
    }

    public class CalculateJumpForce : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.jump.CalculatedJumpForce.Value.x = 0;
            data.jump.CalculatedJumpForce.Value.y = data.jump.JumpBurstForce;
        }
    }

    public class CalculateRunningJumpForce : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpRunningStart) return;
            data.jump.CalculatedJumpForce.Value *= data.jump.RunningStartBonusVelocity.Evaluate(Mathf.Abs(data.locomotion.Velocity.Value.x));
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            if (movementDirection != 0)
            {
                data.jump.CalculatedJumpForce.Value = Quaternion.Euler(0, 0, -data.jump.RunningStartBoostAngle * movementDirection) * data.jump.CalculatedJumpForce.Value;
            }
        }
    }

    public class AttemptJumpApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.jump.ShouldJump)
            {
                data.jump.JumpCount++;
                data.jump.InJump = true;
                data.jump.JumpTakeoffPoint.point = data.physics.RigidBody.transform.position;
                data.jump.JumpTakeoffPoint.time = Time.time;
                data.locomotion.Velocity.Value += data.jump.CalculatedJumpForce.Value;
            }
        }
    }

    public class GoingToJumpMovementAdjuster : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpPreviousVelocityAlteration) return;
            if (data.jump.ShouldJump)
            {
                data.locomotion.Velocity.Value *= data.jump.JumpingPreviousVelocityMultiplier;
                data.locomotion.Acceleration.Multiplicative *= data.jump.JumpingPreviousAccelerationMultiplier;
            }
        }
    }

    public class JumpingEvaluationResetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.jump.ShouldJump = false;
        }
    }


    public class GravityJumpDescendAccelerator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpStrongerGravityDescend) return;
            if (data.jump.InJump && data.locomotion.Velocity.Value.y < 0) data.gravity.CalculatedGravity.Multiplicative.y *= data.jump.DescendingGravityMultiplier;
        }
    }

    public class ApexCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpApexBonus) return;
            data.jump.InApex = data.jump.InJump && Mathf.Abs(data.locomotion.Velocity.Value.y) < data.jump.ApexYVelocityThreshold;
        }
    }

    public class ApexXVelocityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpApexBonus) return;
            if (data.jump.InApex)
            {
                data.locomotion.Acceleration.Multiplicative.x *= data.jump.ApexHorizontalBonusAcceleration * MathUtils.Sign(data.input.Input.direction.x);
            }
        }
    }

    public class ApexAntiGravityMultiplierApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpApexBonus) return;
            if (data.jump.InApex) data.gravity.CalculatedGravity.Multiplicative.y *= data.jump.ApexGravityMultiplier;
        }
    }

    public class EarlyReleaseGravityMultiplierApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!(data.jump.IsUsingJumpEarlyRelease && data.jump.IsUsingJumpEarlyReleaseGravityMultiplier)) return;
            if (data.jump.EarlyOutJump)
            {
                data.gravity.CalculatedGravity.Multiplicative.y *= data.jump.EarlyReleaseGravityMultiplier;
            }
        }
    }

    public class EarlyReleaseCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpEarlyRelease) return;
            if (data.input.Input.jump.Up && data.jump.InJump) data.jump.JumpEarlyRelease = true;
            if (data.collision.GroundContact.Contact) data.jump.JumpEarlyRelease = false;
        }
    }

    public class RoofCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.collision.RoofContact.Contact && data.locomotion.Velocity.Value.y > 0) data.locomotion.Velocity.Value.y = 0;
        }
    }



    public class ImmediateDescendOnReleaseJump : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!(data.jump.IsUsingJumpEarlyRelease && data.jump.IsUsingJumpImmediateDescend)) return;
            if (data.jump.EarlyOutJump && data.locomotion.Velocity.Value.y > 0)
            {
                data.locomotion.Velocity.Value.y = 0;
            }
        }
    }

    public class EnforceMinimumJumpHeight : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.jump.EarlyOutJump = data.jump.JumpEarlyRelease;
            if (!data.jump.IsUsingJumpMinimumHeight) return;
            data.jump.EarlyOutJump = data.jump.JumpEarlyRelease && data.physics.RigidBody.transform.position.y - data.jump.JumpTakeoffPoint.point.y > data.jump.MinimumJumpHeight;
        }
    }
    
    
}