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
            if (!data.IsUsingSpeedClamp) return;
            if (Mathf.Abs(data.Velocity.Value.x) > data.ActiveLocomotion.MaxSpeed)
            {
                int movementDirection = MathUtils.Sign(data.Input.direction.x);
                data.Velocity.Value.x = data.ActiveLocomotion.MaxSpeed * movementDirection;
            }
        }
    }
    
    public class LocomotionSpeedLimiter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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
}