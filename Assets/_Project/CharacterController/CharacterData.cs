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
    [Header("Input")]
    [SerializeField] public CharacterInput InputSystem;
    /**/
    [HideInInspector] public CharacterFrameInput Input;
    //---------------------------------------------------------------------------------------

    [Header("Speed")]
    [SerializeField, AllowNesting, HideIf(nameof(IsUsingSpeedLimiter))] public bool IsUsingSpeedClamp;
    [SerializeField, AllowNesting, HideIf(nameof(IsUsingSpeedClamp))] public bool IsUsingSpeedLimiter;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSpeedLimiter))] public float Damping;
    /**/
    
    //---------------------------------------------------------------------------------------

    [Header("Locomotion")]
    [SerializeField] public Velocity Velocity;
    [SerializeField] public MovementVector2 Acceleration;
    [SerializeField] public CharacterLocomotionState WalkLocomotion;
    [SerializeField] public float HorizontalCutOffVelocity;
    [SerializeField] public bool IsUsingSprint;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSprint))] public CharacterLocomotionState SprintLocomotion;
    [SerializeField] public bool IsUsingCrouch;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingCrouch))] public CharacterLocomotionState CrouchLocomotion;
    [SerializeField] public bool IsUsingSlopeControl;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSlopeControl))] public float MaximumClimbAngle;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSlopeControl))] public float MaximumSlideDownAngle;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSlopeControl))] public bool StayOnGround;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingSlopeControl))] public float SlopeCheckDistance;
    /**/
    [HideInInspector] public CharacterLocomotionState ActiveLocomotion;
    //---------------------------------------------------------------------------------------

    [Header("Gravity")]
    [SerializeField] public bool IsUsingGravity;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingGravity))] public float Gravity;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingGravity))] public bool IsUsingTerminalVelocity;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingGravity), nameof(IsUsingTerminalVelocity))] public float TerminalVelocity;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingGravity))] public bool IsUsingHoverTime;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingGravity), nameof(IsUsingHoverTime))] public float HoverDuration;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingGravity), nameof(IsUsingHoverTime))] public Curve GravityMultiplierCurve;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingGravity))] public bool IsUsingMass;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingGravity), nameof(IsUsingMass))] public float Weight;
    /**/
    [HideInInspector] public float CalculatedMaxFallSpeed;
    [HideInInspector] public MovementVector2 CalculatedGravity;
    //---------------------------------------------------------------------------------------

    [Header("Landing")] //G - bunny hop
    [SerializeField] public bool IsUsingLandingStickyFeet;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public float StickyFeetDuration;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public Curve StickyFeetVelocityTimeToMultiplier;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStickyFeet))] public Curve StickyFeetAccelerationTimeToMultiplier;
    [SerializeField] public bool IsUsingLandingDirectionLock;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingDirectionLock))] public float LandingDirectionLockDuration;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingDirectionLock))] public float LockedOtherDirectionDamper;
    [SerializeField] public bool IsUsingLandingStun;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingStun))] public Curve LandingStunHeightToDuration;
    [SerializeField] public bool IsUsingLandingVelocityBurst;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstAngle;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingLandingVelocityBurst))] public float LandingBurstForce;
    /**/
    [HideInInspector] public float PeakHeight;
    [HideInInspector] public float FallHeight;
    
    //---------------------------------------------------------------------------------------

    [Header("Jump")] // Holding Jump // Pass through breaks early release
    [SerializeField] public float JumpBurstForce;
    [SerializeField] public bool IsUsingJumpApexBonus;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexYVelocityThreshold;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexHorizontalBonusAcceleration;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpApexBonus))] public float ApexGravityMultiplier;
    [SerializeField] public bool IsUsingJumpEarlyRelease;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpEarlyReleaseGravityMultiplier;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingJumpEarlyRelease), nameof(IsUsingJumpEarlyReleaseGravityMultiplier))] public float EarlyReleaseGravityMultiplier;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpImmediateDescend;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpEarlyRelease))] public bool IsUsingJumpMinimumHeight;
    [SerializeField, AllowNesting, ShowIf(EConditionOperator.And, nameof(IsUsingJumpEarlyRelease), nameof(IsUsingJumpMinimumHeight))] public float MinimumJumpHeight;
    [SerializeField] public bool IsUsingJumpStrongerGravityDescend;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpStrongerGravityDescend))] public float DescendingGravityMultiplier;
    [SerializeField] public bool IsUsingJumpInputBuffer;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpInputBuffer))] public float JumpBufferTime;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpInputBuffer))] public bool JumpBuffersTaps;
    [SerializeField] public bool IsUsingJumpCoyoteTime;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpCoyoteTime))] public float JumpCoyoteTime;
    [SerializeField] public bool IsUsingJumpUniqueAirControl;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpUniqueAirControl))] public CharacterLocomotionState AirLocomotion;
    [SerializeField] public bool IsUsingJumpArcTerminalFall;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpArcTerminalFall))] public float JumpArcTerminalVelocity;
    [SerializeField] public bool IsUsingJumpRunningStart;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpRunningStart))] public float RunningStartBoostAngle;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpRunningStart))] public Curve RunningStartBonusVelocity;
    [SerializeField] public bool IsUsingMultipleJumps;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingMultipleJumps))] public int MultipleJumpCount;
    [SerializeField] public bool IsUsingJumpChargeUp;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpChargeUp))] public Curve ChargePowerCurve;
    [SerializeField] public bool IsUsingJumpPreviousVelocityAlteration;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousVelocityMultiplier;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingJumpPreviousVelocityAlteration))] public Vector2 JumpingPreviousAccelerationMultiplier;
    /**/
    [HideInInspector] public bool InApex;
    [HideInInspector] public float TimeOfJumpPressed;
    [HideInInspector] public bool JumpEarlyRelease;
    [HideInInspector] public bool EarlyOutJump;
    [HideInInspector] public bool ShouldJump;
    [HideInInspector] public bool InJump;
    [HideInInspector] public JumpPoint JumpTakeoffPoint = new JumpPoint();
    [HideInInspector] public SVector2 CalculatedJumpForce = new SVector2();
    //---------------------------------------------------------------------------------------
    /*
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
    */
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

    [Header("Adjustment")] //Should use a precision system to determine what is the best correction for the character
    [SerializeField] public bool IsUsingAdjustmentAscendingGroundCornerClip;
    [SerializeField] public bool IsUsingAdjustmentAscendingRoofCornerClip;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingRoofCornerClip))] public CharacterCorrectionRay LeftRoofDetector = new();
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingRoofCornerClip))] public CharacterCorrectionRay RightRoofDetector = new();
    [SerializeField] public bool IsUsingAdjustmentLedgeLander;
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentLedgeLander))] public CharacterCorrectionRay LeftLedgeDetector = new();
    [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentLedgeLander))] public CharacterCorrectionRay RightLedgeDetector = new();
    /**/
    
    //---------------------------------------------------------------------------------------

    [Header("Collision")]
    [SerializeField] public bool IsUsingCollisionStandOnEdge;
    [SerializeField] public CharacterContact GroundContact = new();
    [SerializeField] public CharacterContact RoofContact = new();
    [SerializeField] public CharacterContact LeftWallContact = new();
    [SerializeField] public CharacterContact RightWallContact = new();
    //Dash Rays
    /**/
    
    //---------------------------------------------------------------------------------------

    [Header("Physics")]
    [SerializeField] public Rigidbody2D RigidBody;
    [SerializeField] public bool IsUsingPhysicsEuler;
    [SerializeField] public bool IsUsingPhysicsVerlet;
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