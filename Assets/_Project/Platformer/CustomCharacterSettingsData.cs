
using UnityEngine;

namespace CharacterController.Platformer
{
    public interface ICustomCharacterSettingsData
    {
        public CharacterFrameInput input { get; set; }
        public Vec2 Velocity { get; set; }
        public Vec2 Acceleration { get; set; }
        public float HorizontalAcceleration { get; set; }
        public float MaxSpeed { get; set; }
    }

    public class CustomCharacterSettingsData : ICustomCharacterSettingsData
    {
        public CharacterFrameInput input { get; set; }
        public Vec2 Velocity { get; set; }
        public Vec2 Acceleration { get; set; }
        public float HorizontalAcceleration { get; set; }
        public float MaxSpeed { get; set; }
    }

    public class Vec2
    {
        private float x;
        private float y;

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
    }
}