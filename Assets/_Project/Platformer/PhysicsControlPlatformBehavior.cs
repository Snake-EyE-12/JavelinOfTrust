using System;
using NaughtyAttributes;
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
            [ReadOnly] [SerializeField] private Vector2 velocity;
            [ReadOnly] [SerializeField] private Vector2 acceleration;

            [Prioritized]
            private void ApplyAcceleration()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                acceleration = data.Acceleration.Value;
                data.Velocity.additive += data.Acceleration.Value;
                data.Acceleration.Reset();
                if(data.Velocity.X > 100f) Debug.Log("Warning");
            }
            [Prioritized]
            private void ApplyVelocity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                velocity = data.Velocity.Value;
                body.linearVelocity = data.Velocity.Value * Time.deltaTime;
                data.Velocity.multiplier = Vector2.one;
                if(data.Velocity.X > 100f) Debug.Log("Warning");
            }
        }
        
        [SerializeField] private EulerPhysicsGimmick eulerPhysics;
        protected override void OnLoad()
        {
            Load(eulerPhysics);
        }
    }
}