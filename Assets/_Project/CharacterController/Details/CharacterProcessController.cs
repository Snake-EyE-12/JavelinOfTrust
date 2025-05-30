using System;
using System.Collections.Generic;
using CharacterProcess.Gimmicks;
using UnityEngine;

public class CharacterProcessController : MonoBehaviour
{
    private OperationalController<CharacterDataSettings> chain = new();

    private void BuildProcess()
    {
        chain
            .SetNext(new InputSetter())
            .SetNext(new JumpBufferListener())
            .SetNext(new StartAttackListener())
            .SetNext(new GroundCollisionDetection()) 
            .SetNext(new GroundContactJumpResetter())
            .SetNext(new JumpCounterResetter())
            .SetNext(new JumpingEvaluationResetter())
            .SetNext(new WallCollisionDetection())
            .SetNext(new RoofCollisionDetection())
            .SetNext(new LocomotionWalkDetector())
            .SetNext(new LocomotionSprintDetector())
            .SetNext(new LocomotionCrouchDetector())
            .SetNext(new LocomotionAirDetector())
            .SetNext(new ProcessLocomotionForwardAcceleration())
            .SetNext(new ProcessLocomotionReversalAcceleration())
            .SetNext(new ProcessLocomotionCoasting())
            .SetNext(new ApplyAimingSlowness())
            .SetNext(new JumpCountUser())
            .SetNext(new JumpBufferApplicator())
            .SetNext(new JumpCoyoteInformant())
            .SetNext(new GroundedJumpApplicator())
            .SetNext(new CalculateJumpForce())
            .SetNext(new CalculateRunningJumpForce())
            .SetNext(new GoingToJumpMovementAdjuster())
            .SetNext(new AttemptJumpApplicator())
            .SetNext(new EnforceMinimumJumpHeight())
            .SetNext(new ImmediateDescendOnReleaseJump())
            .SetNext(new GravityJumpDescendAccelerator())
            .SetNext(new ApexCalculator())
            .SetNext(new ApexXVelocityApplicator())
            .SetNext(new ApexAntiGravityMultiplierApplicator())
            .SetNext(new EarlyReleaseCalculator())
            .SetNext(new EarlyReleaseGravityMultiplierApplicator())
            .SetNext(new RoofCollisionVelocityZeroer())
            .SetNext(new JumpArcDetector())
            .SetNext(new JumpArcTerminalFall())
            .SetNext(new TerminalFallSpeedCalculator())
            .SetNext(new GravityApplicator())
            .SetNext(new GroundCollisionVelocityZeroer())
            .SetNext(new WallCollisionVelocityZeroer())
            .SetNext(new LandingFrictionApplicator())
            .SetNext(new LandingDirectionLockApplicator())
            .SetNext(new ApexHeightCalculator())
            .SetNext(new FallHeightCalculator())
            .SetNext(new GravityToAcceleration())
            .SetNext(new LandingStunApplicator())
            .SetNext(new AccelerationApplicator())
            .SetNext(new FallSpeedClamper())
            .SetNext(new ProcessVelocityCutOffImmediateStop())
            .SetNext(new LocomotionSpeedClamper())
            .SetNext(new LocomotionSpeedLimiter())
            .SetNext(new PhysicsVelocityApplicator())
            .SetNext(new GapPositionCorrector())
            .SetNext(new MakeJumpCorrector())
            .SetNext(new HeadCollisionAvoidance())
            .SetNext(new Attack())
            ;
    }


    private void Awake()
    {
        BuildProcess();
    }
    [SerializeField] private CharacterDataSettings data;
    private void Update()
    {
        chain.Process(data);
    }
    public CharacterDataSettings GetData() => data;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; //Bounds
        if(data.physics.RigidBody != null) GizmoExtensions.DrawBounds(data.collision.GroundContact.bounds, data.physics.RigidBody.transform.position);
        if(data.physics.RigidBody != null) GizmoExtensions.DrawBounds(data.collision.RoofContact.bounds, data.physics.RigidBody.transform.position);
        if(data.physics.RigidBody != null) GizmoExtensions.DrawBounds(data.collision.LeftWallContact.bounds, data.physics.RigidBody.transform.position);
        if(data.physics.RigidBody != null) GizmoExtensions.DrawBounds(data.collision.RightWallContact.bounds, data.physics.RigidBody.transform.position);
        
        Gizmos.color = Color.blue; //Adjustments
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftLedgeDetector.origin + (Vector3)data.adjustment.LeftLedgeDetector.correction, data.adjustment.LeftLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightLedgeDetector.origin + (Vector3)data.adjustment.RightLedgeDetector.correction, data.adjustment.RightLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftRoofDetector.origin + (Vector3)data.adjustment.LeftRoofDetector.correction, data.adjustment.LeftRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightRoofDetector.origin + (Vector3)data.adjustment.RightRoofDetector.correction, data.adjustment.RightRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftGroundDetector.origin + (Vector3)data.adjustment.LeftGroundDetector.correction, data.adjustment.LeftGroundDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightGroundDetector.origin + (Vector3)data.adjustment.RightGroundDetector.correction, data.adjustment.RightGroundDetector.direction);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftLedgeDetector.origin, data.adjustment.LeftLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightLedgeDetector.origin, data.adjustment.RightLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftRoofDetector.origin, data.adjustment.LeftRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightRoofDetector.origin, data.adjustment.RightRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.LeftGroundDetector.origin, data.adjustment.LeftGroundDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.adjustment.RightGroundDetector.origin, data.adjustment.RightGroundDetector.direction);
        
            
            /*
        Gizmos.color = Color.red;
        Vector3 jumpCenter = data.jump.InJumpArc ? data.jump.JumpTakeoffPoint.point + data.collision.GroundContact.bounds.center : transform.position + data.collision.GroundContact.bounds.center;
        float height = data.jump.JumpBurstForce * data.jump.JumpBurstForce / (2 * data.gravity.Gravity);
        float jumpDistanceAtHeight = data.locomotion.WalkLocomotion.MaxSpeed * data.jump.JumpBurstForce / data.gravity.Gravity;
        float totalDistance = data.locomotion.WalkLocomotion.MaxSpeed * -data.jump.JumpBurstForce / data.gravity.Gravity;
        
        Vector3 peak = jumpCenter + new Vector3(jumpDistanceAtHeight, height);
        Gizmos.DrawLine(jumpCenter, peak);
        float threshHeight = ((data.jump.ApexYVelocityThreshold * data.jump.ApexYVelocityThreshold) - (data.jump.JumpBurstForce * data.jump.JumpBurstForce)) / (2 * -data.gravity.Gravity);
        //Gizmos.DrawWireSphere(jumpCenter + Vector3.up * threshHeight, 0.1f);
        
        float timeInUpwardsApex = data.jump.ApexYVelocityThreshold / data.gravity.Gravity;
        float timeInDownwardsApex = data.jump.ApexYVelocityThreshold / (data.gravity.Gravity * data.jump.ApexGravityMultiplier);
        float distanceTraveledInApex = data.locomotion.WalkLocomotion.MaxSpeed * (timeInUpwardsApex + timeInDownwardsApex);
        
        Gizmos.DrawLine(peak, peak + Vector3.right * distanceTraveledInApex);
        
        Gizmos.color = new Color(0.75f, 0.5f, 0.0f);
        Gizmos.DrawLine(peak + Vector3.up * (threshHeight - height), peak + Vector3.right * distanceTraveledInApex + Vector3.up * (threshHeight - height));
        
        // //Draw Jump Indicators
        float bufferZone = data.gravity.TerminalVelocity * data.jump.JumpBufferTime;
        Vector2 coyoteZone = new Vector2(data.locomotion.WalkLocomotion.MaxSpeed * data.jump.JumpCoyoteTime, 0.5f * -data.gravity.Gravity * data.jump.JumpCoyoteTime * data.jump.JumpCoyoteTime);
        Gizmos.color = Color.yellow;
        Vector2 coyoteEndPoint = jumpCenter + (Vector3)coyoteZone;
        Vector2 bufferEndPoint = jumpCenter + Vector3.down * bufferZone;
        Gizmos.DrawLine(jumpCenter, coyoteEndPoint);
        Gizmos.DrawLine(coyoteEndPoint + (0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized), coyoteEndPoint + (-0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized));
        Gizmos.DrawLine(jumpCenter, bufferEndPoint);
        Gizmos.DrawLine(bufferEndPoint + (0.1f * Vector2.left), bufferEndPoint + (0.1f * Vector2.right));
        */
        
    }
    

    


    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireCube(transform.position + data.groundCheckBounds.center, data.groundCheckBounds.size);
    //     Gizmos.DrawWireCube(transform.position + data.leftWallBounds.center, data.leftWallBounds.size);
    //     Gizmos.DrawWireCube(transform.position + data.rightWallBounds.center, data.rightWallBounds.size);
    //     Gizmos.DrawWireCube(transform.position + data.roofCheckBounds.center, data.roofCheckBounds.size);
    //     
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawRay(transform.position + (Vector3)data.missedLeftJump.origin + (Vector3)data.missedLeftJump.correction, data.missedLeftJump.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.missedRightJump.origin + (Vector3)data.missedRightJump.correction, data.missedRightJump.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.leftHeadAvoidance.origin + (Vector3)data.leftHeadAvoidance.correction, data.leftHeadAvoidance.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.rightHeadAvoidance.origin + (Vector3)data.rightHeadAvoidance.correction, data.rightHeadAvoidance.direction);
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawRay(transform.position + (Vector3)data.missedLeftJump.origin, data.missedLeftJump.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.missedRightJump.origin, data.missedRightJump.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.leftHeadAvoidance.origin, data.leftHeadAvoidance.direction);
    //     Gizmos.DrawRay(transform.position + (Vector3)data.rightHeadAvoidance.origin, data.rightHeadAvoidance.direction);
    //     //
    //     Gizmos.color = Color.red;
    //     Vector3 jumpCenter = data.inJumpArc ? data.startingJumpPoint + data.groundCheckBounds.center : transform.position + data.groundCheckBounds.center;
    //     float height = data.jumpForce * data.jumpForce / (2 * data.gravity.magnitude);
    //     float jumpDistanceAtHeight = data.maxWalkSpeed * data.jumpForce / data.gravity.magnitude;
    //     float totalDistance = data.maxWalkSpeed * -data.jumpForce / data.gravity.magnitude;
    //     
    //     Vector3 peak = jumpCenter + new Vector3(jumpDistanceAtHeight, height);
    //     Gizmos.DrawLine(jumpCenter, peak);
    //     float threshHeight = ((data.apexYVelocityThreshold * data.apexYVelocityThreshold) - (data.jumpForce * data.jumpForce)) / (2 * -data.gravity.magnitude);
    //     //Gizmos.DrawWireSphere(jumpCenter + Vector3.up * threshHeight, 0.1f);
    //
    //     float timeInUpwardsApex = data.apexYVelocityThreshold / data.gravity.magnitude;
    //     float timeInDownwardsApex = data.apexYVelocityThreshold / (data.gravity.magnitude * data.apexAntiGravityMultiplier);
    //     float distanceTraveledInApex = data.maxWalkSpeed * (timeInUpwardsApex + timeInDownwardsApex);
    //     
    //     Gizmos.DrawLine(peak, peak + Vector3.right * distanceTraveledInApex);
    //     
    //     //Gizmos.color = new Color(0.75f, 0.5f, 0.0f);
    //     //Gizmos.DrawLine(peak + Vector3.up * (threshHeight - height), peak + Vector3.right * distanceTraveledInApex + Vector3.up * (threshHeight - height));
    //     
    //     // //Draw Jump Indicators
    //     float bufferZone = data.maxFallSpeed * data.jumpBufferTime;
    //     Vector2 coyoteZone = new Vector2(data.maxWalkSpeed * data.jumpCoyoteTime, 0.5f * -data.gravity.magnitude * data.jumpCoyoteTime * data.jumpCoyoteTime);
    //     Gizmos.color = Color.yellow;
    //     Vector2 coyoteEndPoint = jumpCenter + (Vector3)coyoteZone;
    //     Vector2 bufferEndPoint = jumpCenter + Vector3.down * bufferZone;
    //     Gizmos.DrawLine(jumpCenter, coyoteEndPoint);
    //     Gizmos.DrawLine(coyoteEndPoint + (0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized), coyoteEndPoint + (-0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized));
    //     Gizmos.DrawLine(jumpCenter, bufferEndPoint);
    //     Gizmos.DrawLine(bufferEndPoint + (0.1f * Vector2.left), bufferEndPoint + (0.1f * Vector2.right));
    // }
}