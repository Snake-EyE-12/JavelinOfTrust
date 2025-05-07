using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterLocomotionGimmick : ICharacterGimmick
    {
        [SerializeField] public Velocity Velocity;
        [SerializeField] public MovementVector2 Acceleration;
        [SerializeField] public CharacterLocomotionState WalkLocomotion;
        [SerializeField] public float HorizontalCutOffVelocity;
        /**/
        [HideInInspector] public CharacterLocomotionState ActiveLocomotion;
    }
    
    public class AccelerationApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.Velocity.Value += data.Acceleration * Time.deltaTime;
            data.Acceleration.Reset();
        }
    }
    
    public class ProcessLocomotionForwardAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            int velDirection = MathUtils.Sign(data.Velocity.Value.x);

            if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
            {
                data.Acceleration.Additive.x += data.ActiveLocomotion.Acceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionReversalAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            int velDirection = MathUtils.Sign(data.Velocity.Value.x);

            if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
            {
                data.Acceleration.Additive.x += data.ActiveLocomotion.ReversalAcceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionCoasting : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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

    public class ProcessVelocityCutOffImmediateStop : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (MathUtils.Sign(data.Input.direction.x) == 0 && 
                Mathf.Abs(data.Velocity.Value.x) < data.HorizontalCutOffVelocity)
            {
                data.Velocity.Value.x = 0;
                data.Acceleration.Additive.x = 0;
            }
        }
    }
    
    public class PhysicsVelocityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingPhysicsEuler) return;
            data.RigidBody.linearVelocity = data.Velocity.Value;
        }
    }

    public class LocomotionWalkDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.ActiveLocomotion = data.WalkLocomotion;
        }
    }
}