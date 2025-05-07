using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterGravityGimmick : ICharacterGimmick
    {
        [SerializeField] public float Gravity;
        [SerializeField] public bool IsUsingTerminalVelocity;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingTerminalVelocity))] public float TerminalVelocity;
        [SerializeField] public bool IsUsingHoverTime;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingHoverTime))] public float HoverDuration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingHoverTime))] public Curve GravityMultiplierCurve;
        [SerializeField] public bool IsUsingMass;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingMass))] public float Weight;
        /**/
        [HideInInspector] public float CalculatedMaxFallSpeed;
        [HideInInspector] public MovementVector2 CalculatedGravity;
    }
    
    public class FallHeightCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.GroundContact.EnteredContact)
            {
                data.FallHeight = data.PeakHeight - data.RigidBody.transform.position.y;
                data.PeakHeight = float.MinValue;
            }
        }
    }

    public class LocomotionAirDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingJumpUniqueAirControl) return;
            if (!data.GroundContact.Contact) data.ActiveLocomotion = data.AirLocomotion;
        }
    }

    public class TerminalFallSpeedCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingTerminalVelocity) return;
            data.CalculatedMaxFallSpeed = data.TerminalVelocity;
        }
    }
    public class FallSpeedClamper : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.Velocity.Value.y < -data.CalculatedMaxFallSpeed) data.Velocity.Value.y = -data.CalculatedMaxFallSpeed;
        }
    }

    
    public class GravityToAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingGravity) return;
            data.Acceleration.Additive.y += data.CalculatedGravity.y;
            data.CalculatedGravity.Reset();
        }
    }

    public class GravityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingGravity) return;
            data.CalculatedGravity.Additive.y -= data.Gravity;
        }
    }
}