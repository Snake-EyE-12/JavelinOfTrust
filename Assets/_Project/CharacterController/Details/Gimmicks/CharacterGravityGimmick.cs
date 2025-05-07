using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterGravityGimmick : ICharacterGimmick
    {
        [SerializeField, Min(0)] public float Gravity;
        [SerializeField] public bool IsUsingTerminalVelocity;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingTerminalVelocity))] public float TerminalVelocity;
        //[SerializeField] public bool IsUsingHoverTime;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingHoverTime))] public float HoverDuration;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingHoverTime))] public Curve GravityMultiplierCurve;
        //[SerializeField] public bool IsUsingMass;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingMass))] public float Weight;
        /**/
        [HideInInspector] public float CalculatedMaxFallSpeed;
        [HideInInspector] public MovementVector2 CalculatedGravity;
    }
    
    public class FallHeightCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.collision.GroundContact.EnteredContact)
            {
                data.landing.FallHeight = data.landing.PeakHeight - data.physics.RigidBody.transform.position.y;
                data.landing.PeakHeight = float.MinValue;
            }
        }
    }

    public class LocomotionAirDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.jump.IsUsingJumpUniqueAirControl) return;
            if (!data.collision.GroundContact.Contact) data.locomotion.ActiveLocomotion = data.jump.AirLocomotion;
        }
    }

    public class TerminalFallSpeedCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.gravity.IsUsingTerminalVelocity) return;
            data.gravity.CalculatedMaxFallSpeed = data.gravity.TerminalVelocity;
        }
    }
    public class FallSpeedClamper : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.locomotion.Velocity.Value.y < -data.gravity.CalculatedMaxFallSpeed) data.locomotion.Velocity.Value.y = -data.gravity.CalculatedMaxFallSpeed;
        }
    }

    
    public class GravityToAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.locomotion.Acceleration.Additive.y += data.gravity.CalculatedGravity.y;
            data.gravity.CalculatedGravity.Reset();
        }
    }

    public class GravityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.gravity.CalculatedGravity.Additive.y -= data.gravity.Gravity;
        }
    }
}