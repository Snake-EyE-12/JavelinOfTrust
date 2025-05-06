using System;
using System.Collections.Generic;
using System.Linq;
using CharacterProcess;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

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

[Serializable]
public class CharacterData2
{
    [field:Header("Input")]
    [field:SerializeField] public CharacterInput InputSystem { get; set; }
    /**/
    public CharacterFrameInput Input { get; set; }
    //---------------------------------------------------------------------------------------
    
    [field:Header("Speed")]
    [Question]
    [field:SerializeField] public bool IsUsingSpeedClamp { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingSpeedLimiter { get; set; }
    [field:SerializeField] public float Damping { get; set; }
    /**/
    
    //---------------------------------------------------------------------------------------

    [field:Header("Locomotion")]
    [field:SerializeField] public Velocity Velocity { get; set; }
    [field:SerializeField] public MovementVector2 Acceleration { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingAcceleration { get; set; }
    [field:SerializeField] public CharacterLocomotionState WalkLocomotion { get; set; }
    [field:SerializeField] public float HorizontalCutOffVelocity { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingSprint { get; set; }
    [field:SerializeField] public CharacterLocomotionState SprintLocomotion { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingCrouch { get; set; }
    [field:SerializeField] public CharacterLocomotionState CrouchLocomotion { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingSlopeControl { get; set; }
    [field:SerializeField] public float MaximumClimbAngle { get; set; }
    [field:SerializeField] public float MaximumSlideDownAngle { get; set; }
    [field:SerializeField] public bool StayOnGround { get; set; }
    [field:SerializeField] public float SlopeCheckDistance { get; set; }
    /**/
    public CharacterLocomotionState ActiveLocomotion { get; set; }
    //---------------------------------------------------------------------------------------
    
    [field:Header("Gravity")]
    [Question]
    [field:SerializeField] public bool IsUsingGravity { get; set; }
    [field:SerializeField] public float Gravity { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingTerminalVelocity { get; set; }
    [field:SerializeField] public float TerminalVelocity { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingHoverTime { get; set; }
    [field:SerializeField] public float HoverDuration { get; set; }
    [field:SerializeField] public Curve GravityMultiplierCurve { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingMass { get; set; }
    [field:SerializeField] public float Weight { get; set; }
    /**/
    public float CalculatedMaxFallSpeed { get; set; }
    //---------------------------------------------------------------------------------------
    
    [field:Header("Landing")] //G - bunny hop
    [Question]
    [field:SerializeField] public bool IsUsingLandingStickyFeet { get; set; }
    [field:SerializeField] public float StickyFeetDuration { get; set; }
    [field:SerializeField] public Curve StickyFeetVelocityTimeToMultiplier { get; set; }
    [field:SerializeField] public Curve StickyFeetAccelerationTimeToMultiplier { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingLandingDirectionLock { get; set; }
    [field:SerializeField] public float LandingDirectionLockDuration { get; set; }
    [field:SerializeField] public float LockedOtherDirectionDamper { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingLandingStun { get; set; }
    [field:SerializeField] public Curve LandingStunHeightToDuration { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingLandingVelocityBurst { get; set; }
    [field:SerializeField] public float LandingBurstAngle { get; set; }
    [field:SerializeField] public float LandingBurstForce { get; set; }
    /**/
    public float PeakHeight { get; set; }
    public float FallHeight { get; set; }
    
    //---------------------------------------------------------------------------------------
    
    [field:Header("Jump")]
    [field:SerializeField] public float JumpBurstForce { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpApexBonus { get; set; }
    [field:SerializeField] public float ApexYVelocityThreshold { get; set; }
    [field:SerializeField] public float ApexHorizontalBonusAcceleration { get; set; }
    [field:SerializeField] public float ApexGravityMultiplier { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpEarlyRelease { get; set; }
    [field:SerializeField] public float EarlyReleaseGravityMultiplier { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpImmediateDescend { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpMinimumHeight { get; set; }
    [field:SerializeField] public float MinimumJumpHeight { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpStrongerGravityDescend { get; set; }
    [field:SerializeField] public float DescendingGravityMultiplier { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpInputBuffer { get; set; }
    [field:SerializeField] public float JumpBufferTime { get; set; }
    [field:SerializeField] public bool AppliesIfCanceled { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpCoyoteTime { get; set; }
    [field:SerializeField] public float JumpCoyoteTime { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpUniqueAirControl { get; set; }
    [field:SerializeField] public CharacterLocomotionState AirLocomotion { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpArcTerminalFall { get; set; }
    [field:SerializeField] public float JumpArcTerminalVelocity { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpRunningStart { get; set; }
    [field:SerializeField] public float RunningStartBoostAngle { get; set; }
    [field:SerializeField] public Curve RunningStartBonusVelocity { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingMultipleJumps { get; set; }
    [field:SerializeField] public int MultipleJumpCount { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpChargeUp { get; set; }
    [field:SerializeField] public Curve ChargePowerCurve { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingJumpPreviousVelocityAlteration { get; set; }
    [field:SerializeField] public Vector2 JumpingPreviousVelocityMultiplier { get; set; }
    [field:SerializeField] public Vector2 JumpingPreviousAccelerationMultiplier { get; set; }
    /**/
    public bool InAir { get; set; }
    public bool InApex { get; set; }
    public float TimeOfJumpPressed { get; set; }
    public bool JumpEarlyRelease { get; set; }
    public bool EarlyOutJump { get; set; }
    public bool ShouldJump { get; set; }
    public bool InJump { get; set; }
    public JumpPoint JumpTakeoffPoint { get; set; } = new JumpPoint();
    public SVector2 CalculatedJumpForce { get; set; } = new SVector2();
    //---------------------------------------------------------------------------------------
    
    [field:Header("Dash")]
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
    /**/
    
    //---------------------------------------------------------------------------------------
    
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
    
    [field:Header("Adjustment")]
    [Question]
    [field:SerializeField] public bool IsUsingAdjustmentAscendingGroundCornerClip { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingAdjustmentAscendingRoofCornerClip { get; set; }
    [field:SerializeField] public CharacterCorrectionRay LeftRoofDetector { get; set; }
    [field:SerializeField] public CharacterCorrectionRay RightRoofDetector { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingAdjustmentLedgeLander { get; set; }
    [field:SerializeField] public CharacterCorrectionRay LeftLedgeDetector { get; set; }
    [field:SerializeField] public CharacterCorrectionRay RightLedgeDetector { get; set; }
    /**/
    
    //---------------------------------------------------------------------------------------
    
    [Header("Collision")]
    [Question]
    [field:SerializeField] public bool IsUsingCollisionStandOnEdge { get; set; }
    [field:SerializeField] public CharacterContact GroundContact { get; set; }
    [field:SerializeField] public CharacterContact RoofContact { get; set; }
    [field:SerializeField] public CharacterContact LeftWallContact { get; set; }
    [field:SerializeField] public CharacterContact RightWallContact { get; set; }
    //Dash Rays
    /**/
    
    //---------------------------------------------------------------------------------------

    [field:Header("Physics")]
    [field:SerializeField] public Rigidbody2D RigidBody { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingPhysicsEuler { get; set; }
    [Question]
    [field:SerializeField] public bool IsUsingPhysicsVerlet { get; set; }
    /**/
    
    //---------------------------------------------------------------------------------------
    
}


/*
 
Apex
Early release
Descending gravity
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
    [field:SerializeField] public float Acceleration { get; set; }
    [field:SerializeField] public float ReversalAcceleration { get; set; }
    [field:SerializeField] public float CoastDamping { get; set; }
    [field:SerializeField] public float MaxSpeed { get; set; }
    
}

[Serializable]
public class MovementVector2
{
    private Vector2 previous;

    public float x
    {
        get
        {
            return Value.x;
        }
    }
    public float y
    {
        get
        {
            return Value.y;
        }
    }

    [HideInInspector] public Vector2 Additive;
    [HideInInspector] public Vector2 Multiplicative;
    public Vector2 Value => Additive * Multiplicative;
    public void Reset()
    {
        Additive = Vector2.zero;
        Multiplicative = Vector2.one;
    }
    
    public void Save() => previous = Value;
    public Vector2 Direction() => (Value - previous).normalized;
    public static implicit operator Vector2(MovementVector2 vector) => vector.Value;
    public static Vector2 operator *(MovementVector2 vector, float value) => vector.Value * value;
    public static Vector2 operator +(MovementVector2 vector, Vector2 value) => vector.Value + value;
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

[Serializable]
public class SVector2
{
    public Vector2 Value;
}
[Serializable]
public class Velocity : SVector2
{
    
}

public class JumpPoint
{
    public Vector3 point;
    public float time;
}