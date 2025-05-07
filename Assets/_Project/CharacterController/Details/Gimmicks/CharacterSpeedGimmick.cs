using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterSpeedGimmick : ICharacterGimmick
    {
        [SerializeField, AllowNesting, HideIf(nameof(IsUsingSpeedLimiter))] public bool IsUsingSpeedClamp;
        [SerializeField, AllowNesting, HideIf(nameof(IsUsingSpeedClamp))] public bool IsUsingSpeedLimiter;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSpeedLimiter))] public float Damping;
    }
    
    
    public class LocomotionSpeedClamper : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.speed.IsUsingSpeedClamp) return;
            if (Mathf.Abs(data.locomotion.Velocity.Value.x) > data.locomotion.ActiveLocomotion.MaxSpeed)
            {
                int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
                data.locomotion.Velocity.Value.x = data.locomotion.ActiveLocomotion.MaxSpeed * movementDirection;
            }
        }
    }
    
    public class LocomotionSpeedLimiter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.speed.IsUsingSpeedLimiter) return;
            float xMag = Mathf.Abs(data.locomotion.Velocity.Value.x);
            if (xMag > data.locomotion.ActiveLocomotion.MaxSpeed)
            {
                float decay = Mathf.Pow(1f - data.speed.Damping, Time.deltaTime);
                data.locomotion.Velocity.Value.x *= decay;
            }
        }
    }
}