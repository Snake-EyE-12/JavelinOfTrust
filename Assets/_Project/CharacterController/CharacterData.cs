using System;
using System.Collections.Generic;
using System.Linq;
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
    public LayerMask groundLayerMask;
    public LayerMask roofLayerMask;
    public Bounds groundCheckBounds;
    public Bounds leftWallBounds;
    public Bounds rightWallBounds;
    public Bounds roofCheckBounds;
    
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
    
    
    
    
    public Vector2 CalculatedGravity { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public float MaxFallSpeed { get; set; }
    
    public bool  InApex { get; set; }
    public bool  InJump { get; set; }
    public bool  CanJump { get; set; }
    public float TimeOfJumpPress { get; set; }
    public float TimeOfJumpStart { get; set; }
    public bool  ReleasedEarly { get; set; }
    
    
    
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

public class CharacterData2
{
    [Header("Locomotion")]
    [Question]
    public bool IsUsingAcceleration { get; set; }
    public CharacterLocomotionState WalkLocomotion { get; set; }
    public float HorizontalCutOffVelocity { get; set; }
    [Question]
    public bool IsUsingSprint { get; set; }
    public CharacterLocomotionState SprintLocomotion { get; set; }
    [Question]
    public bool IsUsingCrouch { get; set; }
    public CharacterLocomotionState CrouchLocomotion { get; set; }
    [Question]
    public bool IsUsingSlopeControl { get; set; }
    public float MaximumClimbAngle { get; set; }
    public float MaximumSlideDownAngle { get; set; }
    public bool StayOnGround { get; set; }
    public float SlopeCheckDistance { get; set; }
    
    [Header("Gravity")]
    [Question]
    public bool IsUsingGravity { get; set; }
    public float Gravity { get; set; }
    public float TerminalVelocity { get; set; }
    [Question]
    public bool IsUsingHoverTime { get; set; }
    public float HoverDuration { get; set; }
    public Curve GravityMultiplierCurve { get; set; }
    [Question]
    public bool IsUsingMass { get; set; }
    public float Weight { get; set; }
    
    [Header("Landing")]
    [Question]
    public bool IsUsingLandingStickyFeet { get; set; }
    public float StickyFeetDuration { get; set; }
    public Curve StickyFeetVelocityTimeToMultiplier { get; set; }
    public Curve StickyFeetAccelerationTimeToMultiplier { get; set; }
    [Question]
    public bool IsUsingLandingDirectionLock { get; set; }
    public float LandingDirectionLockDuration { get; set; }
    [Question]
    public bool IsUsingLandingStun { get; set; }
    public Curve LandingStunHeightToDuration { get; set; }
    [Question]
    public bool IsUsingLandingVelocityBurst { get; set; }
    public float LandingBurstAngle { get; set; }
    public float LandingBurstForce { get; set; }
    
    
    [Header("Jump")]
    public float JumpBurstForce { get; set; }
    [Question]
    public bool IsUsingJumpApexBonus { get; set; }
    public float ApexYVelocityThreshold { get; set; }
    public float ApexHorizontalBonusAcceleration { get; set; }
    public float ApexGravityMultiplier { get; set; }
    [Question]
    public bool IsUsingJumpEarlyRelease { get; set; }
    public float EarlyReleaseGravityMultiplier { get; set; }
    [Question]
    public bool IsUsingJumpImmediateDescend { get; set; }
    [Question]
    public bool IsUsingJumpMinimumHeight { get; set; }
    public float MinimumJumpHeight { get; set; }
    [Question]
    public bool IsUsingJumpStrongerGravityDescend { get; set; }
    public float DescendingGravityMultiplier { get; set; }
    [Question]
    public bool IsUsingJumpInputBuffer { get; set; }
    public float BufferTime { get; set; }
    [Question]
    public bool IsUsingJumpCoyoteTime { get; set; }
    public float CoyoteTime { get; set; }
    [Question]
    public bool IsUsingJumpUniqueAirControl { get; set; }
    public CharacterLocomotionState AirLocomotion { get; set; }
    [Question]
    public bool IsUsingJumpArcTerminalFall { get; set; }
    public float JumpArcTerminalVelocity { get; set; }
    [Question]
    public bool IsUsingJumpRunningStart { get; set; }
    public float RunningStartBoostAngle { get; set; }
    public Curve RunningStartBonusVelocity { get; set; }
    [Question]
    public bool IsUsingMultipleJumps { get; set; }
    public int MultipleJumpCount { get; set; }
    [Question]
    public bool IsUsingJumpChargeUp { get; set; }
    public Curve ChargePowerCurve { get; set; }
    [Question]
    public bool IsUsingJumpPreviousVelocityAlteration { get; set; }
    public Vector2 PreviousVelocityMultiplier { get; set; }
    
    [Header("Dash")]
    public float DashTime { get; set; }
    public float DashDuration { get; set; }
    [Question]
    public bool IsUsingDashChargeUp { get; set; }
    [Question]
    public bool IsUsingDashInVelocityDirection { get; set; }
    [Question]
    public bool IsUsingDashCooldown { get; set; }
    [Question]
    public bool IsUsingDashLimitToGrounded { get; set; }
    [Question]
    public bool IsUsingDashLimitToAirborne { get; set; }
    [Question]
    public bool IsUsingDashSurfaceBounce { get; set; }
    [Question]
    public bool IsUsingDashLimitedCount { get; set; }
    [Question]
    public bool IsUsingDashStopGravity { get; set; }
    [Question]
    public bool IsUsingDashAimAssist { get; set; } // Figure Out if this needs to aim towards something DashTarget
    [Question]
    public bool IsUsingDashInputSteering { get; set; }
    [Question]
    public bool IsUsingDashDashBuffer { get; set; }
    public float DashBufferTime { get; set; }
    [Question]
    public bool IsUsingDashEarlyRelease { get; set; }
    [Question]
    public bool IsUsingDashPreviousVelocityAlteration { get; set; }
    [Question]
    public bool IsUsingDashCollisionCancel { get; set; }
    
    /*
Clipping Correction
Corner Correction
Charge Time
Distance
Velocity
In Current Vel Direction
In Input Direction
Recharge
Bounce
Ground / Air
Multiple
Gravity
Aim Assist
Input Steering
Dash Buffer
Early Release
Full Velocity Cancel
Early Out - Ground | Wall | Jump

     */
    
    [Header("Adjustment")]
    [Question]
    public bool IsUsingAdjustmentAscendingGroundCornerClip { get; set; }
    [Question]
    public bool IsUsingAdjustmentAscendingRoofCornerClip { get; set; }
    [Question]
    public bool IsUsingAdjustmentLedgeLander { get; set; }
    
    [Header("Collision")]
    [Question]
    public bool IsUsingCollisionStandOnEdge { get; set; }
    public CharacterContact GroundContact { get; set; }
    public CharacterContact RoofContact { get; set; }
    public CharacterContact WallContact { get; set; }
    public CharacterCorrectionRay LedgeDetector { get; set; }
    public CharacterCorrectionRay RoofDetector { get; set; }
    //Dash Rays
    
    [Header("Physics")]
    [Question]
    public bool IsUsingPhysicsEuler { get; set; }
    [Question]
    public bool IsUsingPhysicsVerlet { get; set; }
    
    [Header("Speed")]
    [Question]
    public bool IsUsingSpeedClamp { get; set; }
    [Question]
    public bool IsUsingSpeedLimiter { get; set; }
    public float Damping { get; set; }
    public float MaxSpeed { get; set; }
    
}


/*
 
Apex
Early release
Decending gravity
Roof descend
Jump buffer
Coyote time
Sticky feet
Air control
Air break
Terminal fall
Clip roof ascend
Running start
multiple

 */
[Serializable]
public class CharacterLocomotionState
{
    public string Name { get; set; }
    public float Speed { get; set; }
    public bool Normalized { get; set; }
    public float Acceleration { get; set; }
    public float ReversalAcceleration { get; set; }
    public float CoastDamping { get; set; }
    
}


public class QuestionAttribute : Attribute
{
    
}


    
[Serializable]
public class CharacterContact
{
    [field: SerializeField] public Bounds bounds { get; private set; }
    [field: SerializeField] public LayerMask mask { get; private set; }
    private bool inContact;
    private float lastTimeInContact;
    private bool newlyContacted;
    private float timeOfContactEntered;
    public void Update(Transform character)
    {
        bool contacting = Physics2D.OverlapBoxAll(character.position + bounds.center, bounds.size, character.rotation.eulerAngles.z, mask).Length > 0;
        if (inContact && !contacting)
        {
            lastTimeInContact = Time.time;
        }
            
        newlyContacted = !inContact && contacting;
        if (newlyContacted) timeOfContactEntered = Time.time;
            
        inContact = contacting;
    }
    public void DrawGizmos(Transform character)
    {
        Gizmos.DrawWireCube(character.position + bounds.center, bounds.size);
    }
        
    public bool Contact => inContact;
    public bool EnteredContact => newlyContacted;
    public float TimeOfContactEnter => timeOfContactEntered;
    public float TimeOfContactExit => lastTimeInContact;
}
    
[Serializable]
public class CharacterCorrectionRay
{
    public LayerMask mask;
    public Vector2 origin;
    public Vector2 direction;
    public Vector2 correction;
}