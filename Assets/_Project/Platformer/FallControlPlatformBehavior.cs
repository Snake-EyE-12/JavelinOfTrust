using System;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class FallControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class FallGimmick : Gimmick
        {
            [SerializeField] private float gravity;

            [Prioritized]
            private void ApplyGravity()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                data.Acceleration.additive.y += gravity;
                if(data.Velocity.X > 100f) Debug.Log("Warning");
                
            }
        }
        [SerializeField] private FallGimmick gravity;
        protected override void OnLoad()
        {
            Load(gravity);
        }
    }
}