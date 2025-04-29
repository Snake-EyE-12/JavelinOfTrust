using System;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class PhysicsControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [SerializeField] private Rigidbody2D rigidBody;

        [Serializable]
        public class EulerPhysicsGimmick : Gimmick
        {
            [SerializeField] private Rigidbody2D body;

            [Prioritized]
            private void ApplyAcceleration()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.Velocity.additive += data.Acceleration.Value;
                data.Acceleration.Reset();
            }
            [Prioritized]
            private void ApplyVelocity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                body.linearVelocity = data.Velocity.Value;
                data.Velocity.Reset();
            }
        }
        
        [SerializeField] private EulerPhysicsGimmick eulerPhysics;
        protected override void OnLoad()
        {
            Load(eulerPhysics);
        }
    }
}