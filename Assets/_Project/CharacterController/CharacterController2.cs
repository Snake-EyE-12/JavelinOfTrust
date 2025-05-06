using System;
using System.Collections.Generic;
using CharacterProcess;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class CharacterController2 : MonoBehaviour
{
    private OperationalController<CharacterData2> chain = new();

    private void BuildProcess()
    {
        chain
            .SetNext(new InputSetter())
            .SetNext(new JumpBufferListener())
            .SetNext(new GroundCollisionDetection()) 
            .SetNext(new GroundContactJumpResetter())
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
            .SetNext(new HeadCollisionAvoidance())
            ;
    }


    private void Awake()
    {
        BuildProcess();
    }
    [SerializeField] private CharacterData2 data;
    private void Update()
    {
        chain.Process(data);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if(data.RigidBody != null) GizmoExtensions.DrawBounds(data.GroundContact.bounds, data.RigidBody.transform.position);
        if(data.RigidBody != null) GizmoExtensions.DrawBounds(data.RoofContact.bounds, data.RigidBody.transform.position);
        if(data.RigidBody != null) GizmoExtensions.DrawBounds(data.LeftWallContact.bounds, data.RigidBody.transform.position);
        if(data.RigidBody != null) GizmoExtensions.DrawBounds(data.RightWallContact.bounds, data.RigidBody.transform.position);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + (Vector3)data.LeftLedgeDetector.origin + (Vector3)data.LeftLedgeDetector.correction, data.LeftLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.RightLedgeDetector.origin + (Vector3)data.RightLedgeDetector.correction, data.RightLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.LeftRoofDetector.origin + (Vector3)data.LeftRoofDetector.correction, data.LeftRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.RightRoofDetector.origin + (Vector3)data.RightRoofDetector.correction, data.RightRoofDetector.direction);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + (Vector3)data.LeftLedgeDetector.origin, data.LeftLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.RightLedgeDetector.origin, data.RightLedgeDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.LeftRoofDetector.origin, data.LeftRoofDetector.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.RightRoofDetector.origin, data.RightRoofDetector.direction);
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