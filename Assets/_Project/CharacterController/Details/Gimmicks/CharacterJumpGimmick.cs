using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterJumpGimmick : ICharacterGimmick
    {
        [SerializeField] public float JumpBurstForce;
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
        [SerializeField] public bool IsUsingJumpChargeUp;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpChargeUp))] public Curve ChargePowerCurve;
        [SerializeField] public bool IsUsingJumpPreviousVelocityAlteration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousVelocityMultiplier;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousAccelerationMultiplier;
        /**/
        [HideInInspector] public bool InApex;
        [HideInInspector] public float TimeOfJumpPressed;
        [HideInInspector] public bool JumpEarlyRelease;
        [HideInInspector] public bool EarlyOutJump;
        [HideInInspector] public bool ShouldJump;
        [HideInInspector] public bool InJump;
        [HideInInspector] public JumpPoint JumpTakeoffPoint = new JumpPoint();
        [HideInInspector] public SVector2 CalculatedJumpForce = new SVector2();
    }
    
    public class JumpBufferListener : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpInputBuffer) return;
            if (data.Input.jump.Down) data.TimeOfJumpPressed = Time.time;
        }
    }

    public class JumpBufferApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpInputBuffer) return;
            if (data.GroundContact.EnteredContact && data.TimeOfJumpPressed + data.JumpBufferTime > Time.time)
            {
                if (data.JumpBuffersTaps || data.Input.jump.Pressed) data.ShouldJump = true;
            }
        }
    }

    public class GroundedJumpApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.Input.jump.Down && data.GroundContact.Contact) data.ShouldJump = true;
        }
    }

    public class GroundContactJumpResetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.GroundContact.Contact) data.InJump = false;
        }
    }

    public class JumpCoyoteInformant : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.Input.jump.Down && data.GroundContact.TimeOfContactExit + data.JumpCoyoteTime > Time.time)
            {
                data.ShouldJump = true;
            }
        }
    
    }

    public class CalculateJumpForce : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.CalculatedJumpForce.Value.x = 0;
            data.CalculatedJumpForce.Value.y = data.JumpBurstForce;
        }
    }

    public class CalculateRunningJumpForce : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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

    public class AttemptJumpApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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

    public class GoingToJumpMovementAdjuster : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpPreviousVelocityAlteration) return;
            if (data.ShouldJump)
            {
                data.Velocity.Value *= data.JumpingPreviousVelocityMultiplier;
                data.Acceleration.Multiplicative *= data.JumpingPreviousAccelerationMultiplier;
            }
        }
    }

    public class JumpingEvaluationResetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.ShouldJump = false;
        }
    }


    public class GravityJumpDescendAccelerator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpStrongerGravityDescend) return;
            if (data.InJump && data.Velocity.Value.y < 0) data.CalculatedGravity.Multiplicative.y *= data.DescendingGravityMultiplier;
        }
    }

    public class ApexCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            data.InApex = data.InJump && Mathf.Abs(data.Velocity.Value.y) < data.ApexYVelocityThreshold;
        }
    }

    public class ApexXVelocityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            if (data.InApex)
            {
                data.Acceleration.Multiplicative.x *= data.ApexHorizontalBonusAcceleration * MathUtils.Sign(data.Input.direction.x);
            }
        }
    }

    public class ApexAntiGravityMultiplierApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpApexBonus) return;
            if (data.InApex) data.CalculatedGravity.Multiplicative.y *= data.ApexGravityMultiplier;
        }
    }

    public class EarlyReleaseGravityMultiplierApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!(data.IsUsingJumpEarlyRelease && data.IsUsingJumpEarlyReleaseGravityMultiplier)) return;
            if (data.EarlyOutJump)
            {
                data.CalculatedGravity.Multiplicative.y *= data.EarlyReleaseGravityMultiplier;
            }
        }
    }

    public class EarlyReleaseCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpEarlyRelease) return;
            if (data.Input.jump.Up && data.InJump) data.JumpEarlyRelease = true;
            if (data.GroundContact.Contact) data.JumpEarlyRelease = false;
        }
    }

    public class RoofCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.RoofContact.Contact && data.Velocity.Value.y > 0) data.Velocity.Value.y = 0;
        }
    }



    public class ImmediateDescendOnReleaseJump : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!(data.IsUsingJumpEarlyRelease && data.IsUsingJumpImmediateDescend)) return;
            if (data.EarlyOutJump && data.Velocity.Value.y > 0)
            {
                data.Velocity.Value.y = 0;
            }
        }
    }

    public class EnforceMinimumJumpHeight : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.EarlyOutJump = data.JumpEarlyRelease;
            if (!data.IsUsingJumpMinimumHeight) return;
            data.EarlyOutJump = data.JumpEarlyRelease && data.RigidBody.transform.position.y - data.JumpTakeoffPoint.point.y > data.MinimumJumpHeight;
        }
    }
    
    
}