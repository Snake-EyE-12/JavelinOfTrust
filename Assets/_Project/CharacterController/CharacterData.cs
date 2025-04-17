// using System;
// using NaughtyAttributes;
// using UnityEngine;
//
// [Serializable]
// public class CharacterData
// {
//     
//     public CharacterFrameInput input;
//     
//     [Header("Trajectory")]
//     [ReadOnly, SerializeField, AllowNesting] public Vector2 acceleration;
//     [ReadOnly, SerializeField, AllowNesting] public Vector2 velocity;
//     
//     [Header("Fall")]
//     public Vector2 gravity;
//     [HideInInspector] public Vector2 gravityMultiplier;
//     [Min(0)] public float maxFallSpeed;
//     
//     [Header("Walk")]
//     [Min(0)] public float walkAcceleration;
//     [Min(0)] public float walkDeceleration;
//     [Range(0, 1)] public float stoppingDamping;
//     [Min(0)] public float minimumStopXVelocity;
//     [Min(0)] public float maxWalkSpeed;
//     [HideInInspector] public float walkXAccelerationMultiplier;
//     [HideInInspector] public int facingDirection = 1;
//     [Min(0)] public float maxCrouchWalkSpeed;
//     [Min(0)] public float maxSprintWalkSpeed;
//     //[HideInInspector] public float maxWalkSpeed;
//
//     [Header("Jump")]
//     [Min(0)] public float apexYVelocityThreshold;//
//     [Min(0)] public float apexBonusXAcceleration;//
//     [Min(0)] public float apexAntiGravityMultiplier;//
//     [Min(0)] public float earlyReleaseGravityMultiplier;//
//     [HideInInspector] public bool inApex;
//     [Min(0)] public float jumpForce;
//     [HideInInspector] public bool inJump;
//     [HideInInspector] public bool canJump;
//     [HideInInspector] public float timeOfJumpPress;
//     [Min(0)] public float jumpBufferTime;
//     [HideInInspector] public float timeOfJumpStart;
//     [Min(0)] public float jumpCoyoteTime;
//     [Min(0)] public float jumpDescendAcceleration;//
//     [HideInInspector] public bool earlyReleased;
//     [Min(0)] public float airControlXAccelerationMultiplier;//
//     public AnimationCurve runningStartBonus;//
//     [Min(0)] public float landingAccelerationFrictionMultiplier;//
//     [Min(0)] public float landingVelocityDampingMultiplier;//
//     [Min(0)] public float maxInJumpFallSpeed;//
//     [HideInInspector] public Vector3 startingJumpPoint;
//     [HideInInspector] public bool inJumpArc;
//
//     [Header("Attack")]
//     public Inventory inventory;
//     [HideInInspector] public float timeOfAttackStart;
//     
//     #region Comments
//     //                                                                                                  Calculate in Apex
//     //                                                                                                  Apply Bonus Apex Velocity
//     //                                                                                                  Apply Less Gravity in Apex
//     //                                                                                                  Apply More Gravity on Descend
//     //                                                                                                  Apply More Gravity on Early Release
//     //                                                                                                  Immediate Descend on Release
//     //                                                                                                  Roof Hitter Velocity Stop
//     //                                                                                                  Buffer Jump
//     //                                                                                                  Increase Friction on Land Backwards Pressed (Sticky Feet)
//     //                                                                                                  Air Speed Control
//     //                                                                                                  Coyote Time
//     //                                                                                                  Clamp Fall Speed
//     //                                                                                                  Missed Jump Correction (Upwards Margin)
//     //                                                                                                  Head Collision Avoidance
//     //Upwards Feet Clip Ground Avoidance
//     //                                                                                                  Horizontal Speed => Jump Height Boost
//     //                                                                                                  Jump Velocity Addition
//     //Jump Buffer Still Holding Jump
//     //                                                                                                  Have specific jump fall speed only while in jump and above jump start y
//     
//     //Dash
//     //Sprint
//     //Crouch
//     
//     // slopes
//     // momentum
//     // wall jump
//     // multiple jumps
//     // fall damage
//     // minimum jump height
//     // 
//     // drawing jump
//     // 
//     #endregion
//     
//     [Header("Corrections")]
//     public CharacterCorrectionRay missedLeftJump;
//     public CharacterCorrectionRay missedRightJump;
//     public CharacterCorrectionRay leftHeadAvoidance;
//     public CharacterCorrectionRay rightHeadAvoidance;
//     
//     [Header("Collisions")]
//     public CharacterContact groundContact = new CharacterContact();
//     public CharacterContact leftWallContact = new CharacterContact();
//     public CharacterContact rightWallContact = new CharacterContact();
//     public CharacterContact roofContact = new CharacterContact();
//     
//     [Header("References")]
//     public Transform transform;
//     public Rigidbody2D rigidBody;
//     public CharacterInput inputSystem;
// }
//
// [Serializable]
// public class CharacterControlsData : ICharacterSettingsData
// {
//     [field: SerializeField, Header("References")] public Transform Transform { get; set; }
//     [field: SerializeField] public Rigidbody2D RigidBody { get; set; }
//     [field: SerializeField] public CharacterInput InputSystem { get; set; }
//     [field: SerializeField] public Inventory Inventory { get; set; }
//     
//     [field: SerializeField, Header("Corrections")] public CharacterCorrectionRay MissedLeftJump { get; set; }
//     [field: SerializeField] public CharacterCorrectionRay MissedRightJump { get; set; }
//     [field: SerializeField] public CharacterCorrectionRay LeftHeadAvoidance { get; set; }
//     [field: SerializeField] public CharacterCorrectionRay RightHeadAvoidance { get; set; }
//     
//     [field: SerializeField, Header("Collisions")] public CharacterContact GroundContact { get; set; }
//     [field: SerializeField] public CharacterContact RoofContact { get; set; }
//     [field: SerializeField] public CharacterContact LeftWallContact { get; set; }
//     [field: SerializeField] public CharacterContact RightWallContact { get; set; }
//     
//     [field: SerializeField, ReadOnly, AllowNesting, Header("Trajectory")] public Vector2 CalculatedGravity { get; set; }
//     public float GravityMultiplier { get; set; }
//     [field: SerializeField, ReadOnly, AllowNesting] public Vector2 Velocity { get; set; }
//     [field: SerializeField, ReadOnly, AllowNesting] public Vector2 Acceleration { get; set; }
//     [field: SerializeField, ReadOnly, AllowNesting] public float MaxFallSpeed { get; set; }
//     public Vector3 LeavingGroundPoint { get; set; }
//     public Vector3 LastApexPoint { get; set; }
//     public bool InApex { get; set; }
//     public bool InJump { get; set; }
//     public bool InJumpArc { get; set; }
//     public bool CanJump { get; set; }
//     public int CurrentJumpCount { get; set; }
//     public float TimeOfJumpPress { get; set; }
//     public float TimeOfJumpStart { get; set; }
//     public bool ReleasedEarly { get; set; }
//     public Vector3 StartingJumpPoint { get; set; }
//     public Vector2 FacingDirection { get; set; }
//     
//     [field: SerializeField, Header("Movement")] public Vector2 Gravity { get; set; }
//     [field: SerializeField] public float DefaultMaxFallSpeed { get; set; }
//     [field: SerializeField] public float WalkAcceleration { get; set; }
//     [field: SerializeField] public float WalkTurnAroundAcceleration { get; set; }
//     [field: SerializeField] public float WalkNoMovementDamping { get; set; }
//     [field: SerializeField] public float StopVelocityThreshold { get; set; }
//     [field: SerializeField] public float WalkMaxHorizontalSpeed { get; set; }
//     [field: SerializeField] public float DefaultAirControlMultiplier { get; set; }
//     
//     [field: SerializeField, Header("Sprint")] public float SprintAcceleration { get; set; }
//     [field: SerializeField] public float SprintTurnAroundAcceleration { get; set; }
//     [field: SerializeField] public float SprintNoMovementDamping { get; set; }
//     [field: SerializeField] public float SprintMaxHorizontalSpeed { get; set; }
//     
//     [field: SerializeField, Header("Crouch")] public float CrouchAcceleration { get; set; }
//     [field: SerializeField] public float CrouchTurnAroundAcceleration { get; set; }
//     [field: SerializeField] public float CrouchNoMovementDamping { get; set; }
//     [field: SerializeField] public float CrouchMaxHorizontalSpeed { get; set; }
//     
//     [field: SerializeField, Header("Jump")] public float ApexYVelocityThreshold { get; set; }
//     [field: SerializeField] public float ApexHorizontalBonusAcceleration { get; set; }
//     [field: SerializeField] public float ApexGravityMultiplier { get; set; }
//     [field: SerializeField] public float EarlyReleaseGravityMultiplier { get; set; }
//     [field: SerializeField] public float DescendingGravityMultiplier { get; set; }
//     [field: SerializeField] public float DescendingAccelerationAddition { get; set; }
//     [field: SerializeField] public float JumpAirControlMultiplier { get; set; }
//     [field: SerializeField] public float LandingAccelerationMultiplier { get; set; }
//     [field: SerializeField] public float LandingVelocityMultiplier { get; set; }
//     [field: SerializeField] public float StickyFeetDuration { get; set; }
//     [field: SerializeField] public Curve HorizontalVelocityHeightBonus { get; set; }
//     [field: SerializeField] public float BurstForce { get; set; }
//     [field: SerializeField] public float MinimumHeight { get; set; }
//     [field: SerializeField] public float JumpMaxFallSpeed { get; set; }
//     [field: SerializeField] public float BufferTime { get; set; }
//     [field: SerializeField] public float CoyoteTime { get; set; }
//     [field: SerializeField] public int JumpCount { get; set; }
//     [field: SerializeField] public float VelocityInputLerp { get; set; }
//     
//     [field: SerializeField, Header("Dash")] public float DashSpeed { get; set; }
//     [field: SerializeField] public float Duration { get; set; }
//     [field: SerializeField] public float BouncePercent { get; set; }
// }
//
// public interface ICharacterSettingsData
// {
//     public Transform Transform { get; set; }
//     public Rigidbody2D RigidBody { get; set; }
//     public CharacterInput InputSystem { get; set; }
//     
//     public CharacterCorrectionRay MissedLeftJump { get; set; }
//     public CharacterCorrectionRay MissedRightJump { get; set; }
//     public CharacterCorrectionRay LeftHeadAvoidance { get; set; }
//     public CharacterCorrectionRay RightHeadAvoidance { get; set; }
//
//     public CharacterContact GroundContact { get; set; }
//     public CharacterContact RoofContact { get; set; }
//     public CharacterContact LeftWallContact { get; set; }
//     public CharacterContact RightWallContact { get; set; }
//     
//     
//     
//     
//     public Vector2 CalculatedGravity { get; set; }
//     public float GravityMultiplier { get; set; }
//     public Vector2 Velocity { get; set; }
//     public Vector2 Acceleration { get; set; }
//     public float MaxFallSpeed { get; set; }
//     public Vector3 LeavingGroundPoint { get; set; }
//     public Vector3 LastApexPoint { get; set; }
//     
//     public bool  InApex { get; set; }
//     public bool  InJump { get; set; }
//     public bool  InJumpArc { get; set; }
//     public bool  CanJump { get; set; }
//     public int CurrentJumpCount { get; set; }
//     public float TimeOfJumpPress { get; set; }
//     public float TimeOfJumpStart { get; set; }
//     public bool  ReleasedEarly { get; set; }
//     public Vector3 StartingJumpPoint { get; set; }
//     
//     
//     
//     public Vector2 FacingDirection { get; set; }
//     public float VelocityInputLerp { get; set; }
//     
//     public Vector2 Gravity { get; set; }
//     public float DefaultMaxFallSpeed { get; set; }
//     public float WalkAcceleration { get; set; }
//     public float WalkTurnAroundAcceleration { get; set; }
//     public float WalkNoMovementDamping { get; set; }
//     public float StopVelocityThreshold { get; set; }
//     public float WalkMaxHorizontalSpeed { get; set; }
//     public float DefaultAirControlMultiplier { get; set; }
//     
//     public float SprintAcceleration { get; set; }
//     public float SprintTurnAroundAcceleration { get; set; }
//     public float SprintNoMovementDamping { get; set; }
//     public float SprintMaxHorizontalSpeed { get; set; }
//     
//     public float CrouchAcceleration { get; set; }
//     public float CrouchTurnAroundAcceleration { get; set; }
//     public float CrouchNoMovementDamping { get; set; }
//     public float CrouchMaxHorizontalSpeed { get; set; }
//     
//     public float ApexYVelocityThreshold { get; set; }
//     public float ApexHorizontalBonusAcceleration { get; set; }
//     public float ApexGravityMultiplier { get; set; }
//     public float EarlyReleaseGravityMultiplier { get; set; }
//     public float DescendingGravityMultiplier { get; set; }
//     public float DescendingAccelerationAddition { get; set; }
//     public float JumpAirControlMultiplier { get; set; }
//     public float LandingAccelerationMultiplier { get; set; }
//     public float LandingVelocityMultiplier { get; set; }
//     public float StickyFeetDuration { get; set; }
//     public Curve HorizontalVelocityHeightBonus { get; set; }
//     public float BurstForce { get; set; }
//     public float MinimumHeight { get; set; }
//     public float JumpMaxFallSpeed { get; set; }
//     public float BufferTime { get; set; }
//     public float CoyoteTime { get; set; }
//     public int   JumpCount { get; set; }
//     
//     public float DashSpeed { get; set; }
//     public float Duration { get; set; }
//     public float BouncePercent { get; set; }
// }
