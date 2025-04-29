
using UnityEngine;

namespace CharacterController.Platformer
{
    public interface ICustomCharacterSettingsData
    {
        public CharacterFrameInput Input { get; set; }
        public AxisAccumulation Velocity { get; set; }
        public AxisAccumulation Acceleration { get; set; }
        public float MaxSpeed { get; set; }
        public JumpPoint JumpPoint { get; set; }
        public Transform Transform { get; set; }
        public bool InJump { get; set; }
        public bool ShouldJump { get; set; }
        public float JumpVelocityMultiplier { get; set; }
        public bool InApex { get; set; }
        
        public CharacterContact GroundContact { get; set; }
        public CharacterContact RoofContact { get; set; }
        public CharacterContact LeftWallContact { get; set; }
        public CharacterContact RightWallContact { get; set; }
        public bool InJumpArc { get; set; }
        public float maxFallSpeed { get; set; }
        public bool EarlyRelease { get; set; }
        public int PerformedJumps { get; set; }

    }
    

    public class CustomCharacterSettingsData : ICustomCharacterSettingsData
    {
        public CharacterFrameInput Input { get; set; }
        public AxisAccumulation Velocity { get; set; } = new AxisAccumulation();
        public AxisAccumulation Acceleration { get; set; } = new AxisAccumulation();
        public float MaxSpeed { get; set; }
        public JumpPoint JumpPoint { get; set; } = new JumpPoint();
        public Transform Transform { get; set; }
        public bool InJump { get; set; }
        public bool ShouldJump { get; set; }
        public float JumpVelocityMultiplier { get; set; }
        public bool InApex { get; set; }
        public CharacterContact GroundContact { get; set; }
        public CharacterContact RoofContact { get; set; }
        public CharacterContact LeftWallContact { get; set; }
        public CharacterContact RightWallContact { get; set; }
        public bool InJumpArc { get; set; }
        public float maxFallSpeed { get; set; }
        public bool EarlyRelease { get; set; }
        public int PerformedJumps { get; set; }
    }

    public class Vec2
    {
        private float x;
        private float y;

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
    }

    public class AxisAccumulation
    {
        public Vector2 multiplier;
        public Vector2 additive;
        public Vector2 @base;
        public float X => (@base.x + additive.x) * multiplier.x;
        public float Y => (@base.y + additive.y) * multiplier.y;

        public Vector2 Value => new Vector2(X, Y);
        public void Reset() {
            @base = Vector2.zero; additive = Vector2.zero; multiplier = Vector2.one;
        }
    }

    public class JumpPoint
    {
        public Vector3 position;
        public float time;
    }
}