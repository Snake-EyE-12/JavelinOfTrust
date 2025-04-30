using System;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class SpeedControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class MaximumVelocityGimmick : Gimmick
        {
            [SerializeField] private bool applyInAir;
            [Prioritized]
            private void ClampSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (applyInAir && !data.GroundContact.Contact) return;
                if (Mathf.Abs(data.Velocity.X) > data.MaxSpeed)
                {
                    //data.Velocity.multiplier.x *= data.MaxSpeed / Mathf.Abs(data.Velocity.X);
                    data.Velocity.additive.x = Mathf.Sign(data.Velocity.X) * data.MaxSpeed;
                    if(data.Velocity.X > 100f) Debug.Log("Warning");
                }
            }
        }

        [Serializable]
        public class LimitVelocityGimmick : Gimmick
        {
            [SerializeField] private bool applyInAir;
            [SerializeField] private float damping;

            [Prioritized]
            private void LimitSpeed()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (applyInAir && !data.GroundContact.Contact) return;
                if (Mathf.Abs(data.Velocity.X) > data.MaxSpeed)
                {
                    data.Velocity.multiplier *= (1 - damping * Time.deltaTime);
                }
            }

        }
        [SerializeField] private MaximumVelocityGimmick speedClamper;
        [SerializeField] private LimitVelocityGimmick speedLimiter;

        protected override void OnLoad()
        {
            Load(speedClamper);
            Load(speedLimiter);
        }
    }
}