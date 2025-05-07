using System;
using CharacterProcess.Gimmicks;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class CharacterDataSettings
{
    [SerializeField] public CharacterInputGimmick input;
    [SerializeField] public CharacterSpeedGimmick speed;
    [SerializeField] public CharacterLocomotionGimmick locomotion;
    [SerializeField] public CharacterGravityGimmick gravity;
    [SerializeField] public CharacterLandingGimmick landing;
    [SerializeField] public CharacterSprintGimmick sprint;
    [SerializeField] public CharacterCrouchGimmick crouch;
    [SerializeField] public CharacterJumpGimmick jump;
    [SerializeField] public CharacterAdjustmentGimmick adjustment;
    [SerializeField] public CharacterCollisionGimmick collision;
    [SerializeField] public CharacterPhysicsGimmick physics;

    
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
    [field:SerializeField, Min(0)] public float Acceleration { get; set; }
    [field:SerializeField, Min(0)] public float ReversalAcceleration { get; set; }
    [field:SerializeField, Range(0,1)] public float CoastDamping { get; set; }
    [field:SerializeField, Min(0)] public float MaxSpeed { get; set; }
    
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