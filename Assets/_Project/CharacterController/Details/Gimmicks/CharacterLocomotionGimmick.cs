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
        [SerializeField, Min(0)] public float HorizontalCutOffVelocity;
        /**/
        [NonSerialized] public CharacterLocomotionState ActiveLocomotion;
        /**/
        public Action OnZeroedVelocityEvent = delegate { };
    }
    
    public class AccelerationApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.locomotion.Velocity.Value += data.locomotion.Acceleration * Time.deltaTime;
            data.locomotion.Acceleration.Reset();
        }
    }
    
    public class ProcessLocomotionForwardAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            int velDirection = MathUtils.Sign(data.locomotion.Velocity.Value.x);

            if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
            {
                data.locomotion.Acceleration.Additive.x += data.locomotion.ActiveLocomotion.Acceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionReversalAcceleration : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            int velDirection = MathUtils.Sign(data.locomotion.Velocity.Value.x);

            if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
            {
                data.locomotion.Acceleration.Additive.x += data.locomotion.ActiveLocomotion.ReversalAcceleration * movementDirection;
            }
        }
    }

    public class ProcessLocomotionCoasting : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            int velDirection = MathUtils.Sign(data.locomotion.Velocity.Value.x);

            if (movementDirection == 0 && velDirection != 0)
            {
                float decay = Mathf.Pow(1f - data.locomotion.ActiveLocomotion.CoastDamping, Time.deltaTime);
                data.locomotion.Velocity.Value.x *= decay;
            }
        }
    }

    public class ProcessVelocityCutOffImmediateStop : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (MathUtils.Sign(data.input.Input.direction.x) == 0 && 
                Mathf.Abs(data.locomotion.Velocity.Value.x) < data.locomotion.HorizontalCutOffVelocity)
            {
                data.locomotion.Velocity.Value.x = 0;
                data.locomotion.Acceleration.Additive.x = 0;
                data.locomotion.OnZeroedVelocityEvent.Invoke();
            }
        }
    }
    
    public class LocomotionWalkDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.locomotion.ActiveLocomotion = data.locomotion.WalkLocomotion;
        }
    }
}