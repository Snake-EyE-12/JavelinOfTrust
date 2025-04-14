using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class CharacterData
{
    
    public CharacterFrameInput input;
    
    [Header("Trajectory")]
    [ReadOnly, SerializeField, AllowNesting] public Vector2 acceleration;
    [ReadOnly, SerializeField, AllowNesting] public Vector2 velocity;
    
    [Header("Fall")]
    public Vector2 gravity;
    [HideInInspector] public Vector2 gravityMultiplier;
    [Min(0)] public float maxFallSpeed;
    
    [Header("Walk")]
    [Min(0)] public float walkAcceleration;
    [Min(0)] public float walkDeceleration;
    [Range(0, 1)] public float stoppingDamping;
    [Min(0)] public float minimumStopXVelocity;
    [Min(0)] public float maxWalkSpeed;
    [HideInInspector] public float walkXAccelerationMultiplier;
    [HideInInspector] public int facingDirection = 1;
    [Min(0)] public float maxCrouchWalkSpeed;
    [Min(0)] public float maxSprintWalkSpeed;
    //[HideInInspector] public float maxWalkSpeed;

    [Header("Jump")]
    [Min(0)] public float apexYVelocityThreshold;//
    [Min(0)] public float apexBonusXAcceleration;//
    [Min(0)] public float apexAntiGravityMultiplier;//
    [Min(0)] public float earlyReleaseGravityMultiplier;//
    [HideInInspector] public bool inApex;
    [Min(0)] public float jumpForce;
    [HideInInspector] public bool inJump;
    [HideInInspector] public bool canJump;
    [HideInInspector] public float timeOfJumpPress;
    [Min(0)] public float jumpBufferTime;
    [HideInInspector] public float timeOfJumpStart;
    [Min(0)] public float jumpCoyoteTime;
    [Min(0)] public float jumpDescendAcceleration;//
    [HideInInspector] public bool earlyReleased;
    [Min(0)] public float airControlXAccelerationMultiplier;//
    public AnimationCurve runningStartBonus;//
    [Min(0)] public float landingAccelerationFrictionMultiplier;//
    [Min(0)] public float landingVelocityDampingMultiplier;//
    [Min(0)] public float maxInJumpFallSpeed;//
    [HideInInspector] public Vector3 startingJumpPoint;
    [HideInInspector] public bool inJumpArc;

    [Header("Attack")]
    public Inventory inventory;
    [HideInInspector] public float timeOfAttackStart;
    
    #region Comments
    //                                                                                                  Calculate in Apex
    //                                                                                                  Apply Bonus Apex Velocity
    //                                                                                                  Apply Less Gravity in Apex
    //                                                                                                  Apply More Gravity on Descend
    //                                                                                                  Apply More Gravity on Early Release
    //                                                                                                  Immediate Descend on Release
    //                                                                                                  Roof Hitter Velocity Stop
    //                                                                                                  Buffer Jump
    //                                                                                                  Increase Friction on Land Backwards Pressed (Sticky Feet)
    //                                                                                                  Air Speed Control
    //                                                                                                  Coyote Time
    //                                                                                                  Clamp Fall Speed
    //                                                                                                  Missed Jump Correction (Upwards Margin)
    //                                                                                                  Head Collision Avoidance
    //Upwards Feet Clip Ground Avoidance
    //                                                                                                  Horizontal Speed => Jump Height Boost
    //                                                                                                  Jump Velocity Addition
    //Jump Buffer Still Holding Jump
    //                                                                                                  Have specific jump fall speed only while in jump and above jump start y
    
    //Dash
    //Sprint
    //Crouch
    
    // slopes
    // momentum
    // wall jump
    // multiple jumps
    // fall damage
    // minimum jump height
    // 
    // drawing jump
    // 
    #endregion
    
    [Header("Corrections")]
    public CharacterCorrectionRay missedLeftJump;
    public CharacterCorrectionRay missedRightJump;
    public CharacterCorrectionRay leftHeadAvoidance;
    public CharacterCorrectionRay rightHeadAvoidance;
    
    [Header("Collisions")]
    public CharacterContact groundContact = new CharacterContact();
    public CharacterContact leftWallContact = new CharacterContact();
    public CharacterContact rightWallContact = new CharacterContact();
    public CharacterContact roofContact = new CharacterContact();
    
    [Header("References")]
    public Transform transform;
    public Rigidbody2D rigidBody;
    public CharacterInput inputSystem;
}

public interface ICharacterSettingsData
{
    public Transform Transform { get; set; }
    public Rigidbody2D RigidBody { get; set; }
    public CharacterInput InputSystem { get; set; }
    public Inventory Inventory { get; set; }
    
    public CharacterCorrectionRay MissedLeftJump { get; set; }
    public CharacterCorrectionRay MissedRightJump { get; set; }
    public CharacterCorrectionRay LeftHeadAvoidance { get; set; }
    public CharacterCorrectionRay RightHeadAvoidance { get; set; }

    public CharacterContact GroundContact { get; set; }
    public CharacterContact RoofContact { get; set; }
    public CharacterContact LeftWallContact { get; set; }
    public CharacterContact RightWallContact { get; set; }
    
    
    
    
    public Vector2 CalculatedGravity { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public float MaxFallSpeed { get; set; }
    public Vector3 LeavingGroundPoint { get; set; }
    public Vector3 LastApexPoint { get; set; }
    
    public bool  InApex { get; set; }
    public bool  InJump { get; set; }
    public bool  InJumpArc { get; set; }
    public bool  CanJump { get; set; }
    public float TimeOfJumpPress { get; set; }
    public float TimeOfJumpStart { get; set; }
    public bool  ReleasedEarly { get; set; }
    public Vector3 StartingJumpPoint { get; set; }
    
    
    
    public Vector2 FacingDirection { get; set; }
    public float VelocityInputLerp { get; set; }
    
    public Vector2 Gravity { get; set; }
    public float DefaultMaxFallSpeed { get; set; }
    public float WalkAcceleration { get; set; }
    public float WalkTurnAroundAcceleration { get; set; }
    public float WalkNoMovementDamping { get; set; }
    public float StopVelocityThreshold { get; set; }
    public float WalkMaxHorizontalSpeed { get; set; }
    
    public float SprintAcceleration { get; set; }
    public float SprintTurnAroundAcceleration { get; set; }
    public float SprintNoMovementDamping { get; set; }
    public float SprintMaxHorizontalSpeed { get; set; }
    
    public float CrouchAcceleration { get; set; }
    public float CrouchTurnAroundAcceleration { get; set; }
    public float CrouchNoMovementDamping { get; set; }
    public float CrouchMaxHorizontalSpeed { get; set; }
    
    public float ApexYVelocityThreshold { get; set; }
    public float ApexHorizontalBonusAcceleration { get; set; }
    public float ApexGravityMultiplier { get; set; }
    public float EarlyReleaseGravityMultiplier { get; set; }
    public float DescendingGravityMultiplier { get; set; }
    public float DescendingAccelerationAddition { get; set; }
    public float AirControlMultiplier { get; set; }
    public float LandingAccelerationMultiplier { get; set; }
    public float LandingVelocityMultiplier { get; set; }
    public float StickyFeetDuration { get; set; }
    public Curve HorizontalVelocityHeightBonus { get; set; }
    public float BurstForce { get; set; }
    public float MinimumHeight { get; set; }
    public float JumpMaxFallSpeed { get; set; }
    public float BufferTime { get; set; }
    public float CoyoteTime { get; set; }
    public int   JumpCount { get; set; }
}

public enum CharacterMoveState
{
    Normal,
    Crouching,
    Sprinting
}