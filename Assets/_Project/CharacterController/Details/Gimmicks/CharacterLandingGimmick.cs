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
        //[SerializeField] public bool IsUsingLandingVelocityBurst;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstAngle;
        //[SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstForce;
        /**/
        [NonSerialized] public float PeakHeight;
        [NonSerialized] public float FallHeight;
    }
    
    public class LandingFrictionApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.landing.IsUsingLandingStickyFeet) return;
            float timeInStickyFeet = Time.time - data.collision.GroundContact.TimeOfContactEnter;
            if (timeInStickyFeet < data.landing.StickyFeetDuration)
            {
                data.locomotion.Acceleration.Multiplicative *= data.landing.StickyFeetAccelerationTimeToMultiplier.Evaluate(timeInStickyFeet);
                data.locomotion.Velocity.Value *= data.landing.StickyFeetVelocityTimeToMultiplier.Evaluate(timeInStickyFeet);
            }
        }

    }
    
    public class LandingDirectionLockApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.landing.IsUsingLandingDirectionLock) return;
            float timeInLock = Time.time - data.collision.GroundContact.TimeOfContactEnter;
            if (timeInLock < data.landing.LandingDirectionLockDuration)
            {
                int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
                int velocityDirection = MathUtils.Sign(data.locomotion.Velocity.Value.x);
                if (movementDirection != 0 && movementDirection != velocityDirection) //Question if vel is 0
                {
                    float decay = Mathf.Pow(1f - data.landing.LockedOtherDirectionDamper, Time.deltaTime);
                    data.locomotion.Velocity.Value.x *= decay;
                }
            }
        }
    }

    public class ApexHeightCalculator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if(data.landing.PeakHeight < data.physics.RigidBody.transform.position.y) data.landing.PeakHeight = data.physics.RigidBody.transform.position.y;
        }
    }

    public class LandingStunApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.landing.IsUsingLandingStun) return;
            float timeInStun = Time.time - data.collision.GroundContact.TimeOfContactEnter;
            if (timeInStun < data.landing.LandingStunHeightToDuration.Evaluate(timeInStun))
            {
                data.locomotion.Acceleration.Additive.x = 0;
            }
        }
    }

}