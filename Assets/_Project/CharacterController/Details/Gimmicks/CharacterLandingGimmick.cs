using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterLandingGimmick : ICharacterGimmick
    {
        [SerializeField] public bool IsUsingLandingStickyFeet;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public float StickyFeetDuration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public Curve StickyFeetVelocityTimeToMultiplier;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public Curve StickyFeetAccelerationTimeToMultiplier;
        [SerializeField] public bool IsUsingLandingDirectionLock;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingDirectionLock))] public float LandingDirectionLockDuration;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingDirectionLock))] public float LockedOtherDirectionDamper;
        [SerializeField] public bool IsUsingLandingStun;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStun))] public Curve LandingStunHeightToDuration;
        [SerializeField] public bool IsUsingLandingVelocityBurst;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstAngle;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstForce;
        /**/
        [HideInInspector] public float PeakHeight;
        [HideInInspector] public float FallHeight;
    }
    
    public class LandingFrictionApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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
    
    public class LandingDirectionLockApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
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

    public class ApexHeightCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if(data.PeakHeight < data.RigidBody.transform.position.y) data.PeakHeight = data.RigidBody.transform.position.y;
        }
    }

    public class LandingStunApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingLandingStun) return;
            float timeInStun = Time.time - data.GroundContact.TimeOfContactEnter;
            if (timeInStun < data.LandingStunHeightToDuration.Evaluate(timeInStun))
            {
                data.Acceleration.Additive.x = 0;
            }
        }
    }

}