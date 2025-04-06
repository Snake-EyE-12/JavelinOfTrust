using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class CharacterProcessor : BaseProcessor<CharacterData>
{
}

public class InputReceiver : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.input = data.inputSystem.GetFrameInput();
        
        base.Process(data);
    }
}

public class VelocityApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.rigidBody.linearVelocity = data.velocity;
        if (data.velocity.x != 0) data.facingDirection = (int)data.velocity.x;
        
        base.Process(data);
    }
}

public class WalkAcceleration : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        int velDirection = MathUtils.Sign(data.velocity.x);

        if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
        {
            data.acceleration.x += data.walkAcceleration * data.walkXAccelerationMultiplier * movementDirection;
        }

        data.walkXAccelerationMultiplier = 1;
        
        base.Process(data);
    }
}

public class AirControlXAccelerationMultiplier : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.inJump) data.walkXAccelerationMultiplier *= data.airControlXAccelerationMultiplier;
        
        base.Process(data);
    }
}

public class AccelerationApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.velocity += data.acceleration * Time.deltaTime;
        data.acceleration = Vector2.zero;
        
        base.Process(data);
    }
}

public class WalkDeceleration : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        int velDirection = MathUtils.Sign(data.velocity.x);

        if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
        {
            data.acceleration.x += data.walkDeceleration * -velDirection;
        }
        
        base.Process(data);
    }
}

public class StoppingDamping : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        int velDirection = MathUtils.Sign(data.velocity.x);

        if (movementDirection == 0 && velDirection != 0)
        {
            data.velocity.x *= data.stoppingDamping;
        }
        
        base.Process(data);
    }
}

public class MinimumStopXVelocity : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (MathUtils.Sign(data.input.direction.x) == 0 && data.velocity.x < data.minimumStopXVelocity && data.velocity.x > -data.minimumStopXVelocity)
        {
            data.velocity.x = 0;
            data.acceleration.x = 0;
        }
        
        base.Process(data);
    }
}

public class GravityApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.acceleration += data.gravity * data.gravityMultiplier;
        data.gravityMultiplier = Vector2.one;
        base.Process(data);
    }
}

public class GroundCollisionVelocityZeroer : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.groundContact.Contact && data.velocity.y < 0) data.velocity.y = 0;
        
        base.Process(data);
    }
   
}

public class WallCollisionVelocityZeroer : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.leftWallContact.Contact && data.velocity.x < 0) data.velocity.x = 0;
        if (data.rightWallContact.Contact && data.velocity.x > 0) data.velocity.x = 0;
        
        base.Process(data);
    }
   
}


public class GroundCollisionDetection : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.groundContact.Update(
                Physics2D.OverlapBoxAll(data.transform.position + data.groundCheckBounds.center, data.groundCheckBounds.size, 0f, data.groundLayerMask).Length > 0
            );
        
        base.Process(data);
    }
}

public class WallCollisionDetection : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        bool leftWallContact = Physics2D.OverlapBoxAll(data.transform.position + data.leftWallBounds.center, data.leftWallBounds.size, 0f, data.groundLayerMask).Length > 0;
        bool rightWallContact = Physics2D.OverlapBoxAll(data.transform.position + data.rightWallBounds.center, data.rightWallBounds.size, 0f, data.groundLayerMask).Length > 0;
        
        data.leftWallContact.Update(leftWallContact);
        data.rightWallContact.Update(rightWallContact);
        
        base.Process(data);
    }
}

public class FallSpeedClamper : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.velocity.y < -data.maxFallSpeed) data.velocity.y = -data.maxFallSpeed;
        
        base.Process(data);
    }
}

public class WalkSpeedClamper : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.velocity.x > data.maxWalkSpeed) data.velocity.x = data.maxWalkSpeed;
        if (data.velocity.x < -data.maxWalkSpeed) data.velocity.x = -data.maxWalkSpeed;

        base.Process(data);
    }

}

public class JumpBufferer : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.input.jumpStarted) data.timeOfJumpPress = Time.time;
        
        base.Process(data);
    }
}

public class ImmediateDescendOnReleaseJump : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.input.jumpEnded && data.inJump)
        {
            if (data.velocity.y > 0) data.velocity.y = 0;
            if (data.acceleration.y > 0) data.acceleration.y = 0;
        }
        
        base.Process(data);
    }
}

public class JumpOnGroundInformant : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.timeOfJumpPress + data.jumpBufferTime > Time.time && data.groundContact.Contact)
        {
            data.canJump = true;
        }
        
        base.Process(data);
    }
}

public class GroundContactJumpResetter : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.groundContact.Contact) data.inJump = false;
        
        base.Process(data);
    }
}

public class JumpCoyoteInformant : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.input.jumpStarted && data.groundContact.TimeOfContactExit + data.jumpCoyoteTime > Time.time)
        {
            data.canJump = true;
        }
        
        base.Process(data);
    }
    
}
public class AttemptJumpApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.canJump && !data.inJump)
        {
            data.inJump = true;
            data.acceleration.y = 0;
            data.velocity.y = data.jumpForce * data.runningStartBonus.Evaluate(Mathf.Abs(data.velocity.x));
            data.timeOfJumpStart = Time.time;
        }

        data.canJump = false;
        
        base.Process(data);
    }
}


public class GravityJumpDescendAccelerator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.inJump && data.velocity.y < 0) data.acceleration.y -= data.jumpDescendAcceleration;
        
        base.Process(data);
    }
}
public class ApexCalculator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.inApex = data.inJump && Mathf.Abs(data.velocity.y) < data.apexYVelocityThreshold;
        
        base.Process(data);
    }
}

public class ApexXVelocityApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.inApex)
        {
            data.acceleration.x += data.apexBonusXAcceleration * MathUtils.Sign(data.input.direction.x);
        }
        
        base.Process(data);
    }
}

public class ApexAntiGravityMultiplierApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.inApex) data.gravityMultiplier *= data.apexAntiGravityMultiplier;
        
        base.Process(data);
    }
}

public class EarlyReleaseGravityMultiplierApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.earlyReleased)
        {
            data.gravityMultiplier *= data.earlyReleaseGravityMultiplier;
        }
        
        base.Process(data);
    }
}

public class EarlyReleaseCalculator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.input.jumpEnded && data.inJump) data.earlyReleased = true;
        if (data.groundContact.Contact) data.earlyReleased = false;
        
        base.Process(data);
    }
}

public class RoofCollisionVelocityZeroer : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if(data.roofContact.Contact && data.velocity.y > 0) data.velocity.y = 0;
        
        base.Process(data);
    }
}

public class RoofCollisionDetection : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        data.roofContact.Update(
            Physics2D.OverlapBoxAll(data.transform.position + data.roofCheckBounds.center, data.roofCheckBounds.size, 0f, data.groundLayerMask).Length > 0
        );
        
        base.Process(data);
    }
}

public class GapPositionCorrector : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        if (data.velocity.y < 0 && !data.groundContact.Contact)
        {
            if (movementDirection == -1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedLeftJump.origin, data.missedLeftJump.direction, data.missedLeftJump.direction.magnitude, data.groundLayerMask).Length > 0)
            {
                data.transform.position += (Vector3)data.missedLeftJump.correction;
            }
            else if (movementDirection == 1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedRightJump.origin, data.missedRightJump.direction, data.missedRightJump.direction.magnitude, data.groundLayerMask).Length > 0)
            {
                data.transform.position += (Vector3)data.missedRightJump.correction;
            }
        }
        
        base.Process(data);
    }
    
}

public class HeadCollisionAvoidance : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        if (data.inJump && !data.earlyReleased)
        {
            if (movementDirection != -1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.leftHeadAvoidance.origin, data.leftHeadAvoidance.direction, data.leftHeadAvoidance.direction.magnitude).Length == 0 &&
                Physics2D.RaycastAll(data.transform.position + (Vector3)(data.leftHeadAvoidance.origin - data.leftHeadAvoidance.correction), data.leftHeadAvoidance.direction, data.leftHeadAvoidance.direction.magnitude).Length > 0)
            {
                data.transform.position += (Vector3)data.leftHeadAvoidance.correction;
            }
            else if (movementDirection != 1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.rightHeadAvoidance.origin, data.rightHeadAvoidance.direction, data.rightHeadAvoidance.direction.magnitude).Length == 0 &&
                     Physics2D.RaycastAll(data.transform.position + (Vector3)(data.rightHeadAvoidance.origin - data.rightHeadAvoidance.correction), data.rightHeadAvoidance.direction, data.rightHeadAvoidance.direction.magnitude).Length > 0)
            {
                data.transform.position += (Vector3)data.rightHeadAvoidance.correction;
            }
        }
        
        base.Process(data);
    }
    
}

public class LandingAccelerationFrictionApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        int velocityDirection = MathUtils.Sign(data.velocity.x);
        if (movementDirection != 0 && data.groundContact.EnteredContact && movementDirection != velocityDirection)
        {
            data.walkXAccelerationMultiplier *= data.landingAccelerationFrictionMultiplier;
        }
        
        base.Process(data);
    }
    
}
public class LandingVelocityDampingApplicator : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        int movementDirection = MathUtils.Sign(data.input.direction.x);
        int velocityDirection = MathUtils.Sign(data.velocity.x);
        if (movementDirection != 0 && data.groundContact.EnteredContact && movementDirection != velocityDirection)
        {
            data.velocity.x *= data.landingVelocityDampingMultiplier;
        }
        
        base.Process(data);
    }
}

public class ManageAttack : CharacterProcessor
{
    public override void Process(CharacterData data)
    {
        if (data.input.attackStarted)
        {
            Vector2 useDirection = data.input.direction;
            if (useDirection == Vector2.zero) useDirection.x = data.facingDirection;
            data.inventory.GetSelectedItem().StartUse(new ItemUseData(data.transform, useDirection, data.velocity));
        }
        if(data.input.attackEnded) data.inventory.GetSelectedItem().EndUse(null);
        
        base.Process(data);
    }
}

[Serializable]
public class CharacterData
{
    public Inventory inventory = new Inventory();
    public CharacterFrameInput input;
    [HideInInspector] public Vector2 acceleration;
    [HideInInspector] public Vector2 velocity;
    
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

    [Header("Jump")]
    [Min(0)] public float apexYVelocityThreshold;
    [Min(0)] public float apexBonusXAcceleration;
    [Min(0)] public float apexAntiGravityMultiplier;
    [Min(0)] public float earlyReleaseGravityMultiplier;
    [HideInInspector] public bool inApex;
    [Min(0)] public float jumpForce;
    [HideInInspector] public bool inJump;
    [HideInInspector] public bool canJump;
    [HideInInspector] public float timeOfJumpPress;
    [Min(0)] public float jumpBufferTime;
    [HideInInspector] public float timeOfJumpStart;
    [Min(0)] public float jumpCoyoteTime;
    [Min(0)] public float jumpDescendAcceleration;
    [HideInInspector] public bool earlyReleased;
    [Min(0)] public float airControlXAccelerationMultiplier;
    public AnimationCurve runningStartBonus;
    [Min(0)] public float landingAccelerationFrictionMultiplier;
    [Min(0)] public float landingVelocityDampingMultiplier;
    
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
    public Bounds groundCheckBounds;
    public LayerMask groundLayerMask;
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

public class CharacterContact
{
    private bool inContact;
    private float lastTimeInContact;
    private bool newlyContacted;
    public void Update(bool contacting)
    {
        if (inContact && !contacting)
        {
            lastTimeInContact = Time.time;
        }
        
        newlyContacted = !inContact && contacting;
        
        inContact = contacting;
    }

    public bool Contact => inContact;
    public bool EnteredContact => newlyContacted;
    public float TimeOfContactExit => lastTimeInContact;
}
[Serializable]
public class CharacterCorrectionRay
{
    public Vector2 origin;
    public Vector2 direction;
    public Vector2 correction;
}