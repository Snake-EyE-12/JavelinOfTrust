using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private CharacterProcessor chain;

    private void BuildProcess()
    {

        chain = new InputReceiver();
        chain.SetNext(new GroundCollisionDetection())
            .SetNext(new HeadCollisionAvoidance())
            .SetNext(new RoofCollisionDetection())
            .SetNext(new WallCollisionDetection())
            .SetNext(new ApexCalculator())
            .SetNext(new EarlyReleaseCalculator())
            .SetNext(new EarlyReleaseGravityMultiplierApplicator())
            .SetNext(new AirControlXAccelerationMultiplier())
            .SetNext(new WalkAcceleration())
            .SetNext(new WalkDeceleration())
            .SetNext(new StoppingDamping())
            .SetNext(new WalkSpeedClamper())
            .SetNext(new MinimumStopXVelocity())
            .SetNext(new ApexAntiGravityMultiplierApplicator())
            .SetNext(new GravityApplicator())
            .SetNext(new GroundCollisionVelocityZeroer())
            .SetNext(new WallCollisionVelocityZeroer())
            .SetNext(new JumpBufferer())
            .SetNext(new JumpOnGroundInformant())
            .SetNext(new JumpCoyoteInformant())
            .SetNext(new GroundContactJumpResetter())
            .SetNext(new AttemptJumpApplicator())
            .SetNext(new RoofCollisionVelocityZeroer())
            .SetNext(new ImmediateDescendOnReleaseJump())
            .SetNext(new GravityJumpDescendAccelerator())
            .SetNext(new ApexXVelocityApplicator())
            .SetNext(new LandingAccelerationFrictionApplicator())
            .SetNext(new AccelerationApplicator())
            .SetNext(new FallSpeedClamper())
            .SetNext(new LandingVelocityDampingApplicator())
            .SetNext(new VelocityApplicator())
            .SetNext(new GapPositionCorrector())
            //.SetNext(new ManageAttack())
            ;
    }


    private void Awake()
    {
        BuildProcess();
    }
    [SerializeField] private CharacterData data;
    private void Update()
    {
        chain.Process(data);
        
    }
    
    
    
    
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + data.groundCheckBounds.center, data.groundCheckBounds.size);
        Gizmos.DrawWireCube(transform.position + data.leftWallBounds.center, data.leftWallBounds.size);
        Gizmos.DrawWireCube(transform.position + data.rightWallBounds.center, data.rightWallBounds.size);
        Gizmos.DrawWireCube(transform.position + data.roofCheckBounds.center, data.roofCheckBounds.size);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + (Vector3)data.missedLeftJump.origin, data.missedLeftJump.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.missedRightJump.origin, data.missedRightJump.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.leftHeadAvoidance.origin, data.leftHeadAvoidance.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.rightHeadAvoidance.origin, data.rightHeadAvoidance.direction);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + (Vector3)data.missedLeftJump.origin - (Vector3)data.missedLeftJump.correction, data.missedLeftJump.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.missedRightJump.origin - (Vector3)data.missedRightJump.correction, data.missedRightJump.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.leftHeadAvoidance.origin - (Vector3)data.leftHeadAvoidance.correction, data.leftHeadAvoidance.direction);
        Gizmos.DrawRay(transform.position + (Vector3)data.rightHeadAvoidance.origin - (Vector3)data.rightHeadAvoidance.correction, data.rightHeadAvoidance.direction);
        //
        Gizmos.color = Color.red;
        Vector3 jumpCenter = transform.position + data.groundCheckBounds.center;
        // Handles.color = Color.red;
        float height = data.jumpForce * data.jumpForce / (2 * data.gravity.magnitude);
        float jumpDistanceAtHeight = data.maxWalkSpeed * data.jumpForce / data.gravity.magnitude;
        float totalDistance = data.maxWalkSpeed * -data.jumpForce / data.gravity.magnitude;
        //
        // DrawGizmoArc(transform.position + data.groundCheckBounds.center, transform.position + data.groundCheckBounds.center + new Vector3(jumpDistanceAtHeight, height));
        // DrawGizmoArc(transform.position + data.groundCheckBounds.center, transform.position + data.groundCheckBounds.center + new Vector3(totalDistance, height));
        //
        Vector3 peak = jumpCenter + new Vector3(jumpDistanceAtHeight, height);
        Gizmos.DrawLine(jumpCenter, peak);
        float threshHeight = ((data.apexYVelocityThreshold * data.apexYVelocityThreshold) - (data.jumpForce * data.jumpForce)) / (2 * -data.gravity.magnitude);
        Gizmos.DrawWireSphere(jumpCenter + Vector3.up * threshHeight, 0.1f);
        //float airDistance = 

        float timeInUpwardsApex = data.apexYVelocityThreshold / data.gravity.magnitude;
        float timeInDownwardsApex = data.apexYVelocityThreshold / (data.gravity.magnitude * data.apexAntiGravityMultiplier);
        float distanceTraveledInApex = data.maxWalkSpeed * (timeInUpwardsApex + timeInDownwardsApex);
        
        Gizmos.DrawLine(peak, peak + Vector3.right * distanceTraveledInApex);
        
        
        // //Gizmos.DrawLine(transform.position + data.groundCheckBounds.center + Vector3.right * 0.1f, transform.position + data.groundCheckBounds.center + Vector3.right * 0.1f + Vector3.up * threshHeight);
        // //Handles.DrawWireArc(transform.position + data.groundCheckBounds.center + new Vector3(jumpDistanceAtHeight, height), Vector3.forward, Vector3.left, -90, height);
        // //Draw Jump Indicators
        float bufferZone = data.maxFallSpeed * data.jumpBufferTime;
        Vector2 coyoteZone = new Vector2(data.maxWalkSpeed * data.jumpCoyoteTime, 0.5f * -data.gravity.magnitude * data.jumpCoyoteTime * data.jumpCoyoteTime);
        Gizmos.color = Color.yellow;
        Vector2 coyoteEndPoint = jumpCenter + (Vector3)coyoteZone;
        Vector2 bufferEndPoint = jumpCenter + Vector3.down * bufferZone;
        Gizmos.DrawLine(jumpCenter, coyoteEndPoint);
        Gizmos.DrawLine(coyoteEndPoint + (0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized), coyoteEndPoint + (-0.1f * Vector2.Perpendicular(coyoteEndPoint - (Vector2)jumpCenter).normalized));
        Gizmos.DrawLine(jumpCenter, bufferEndPoint);
        Gizmos.DrawLine(bufferEndPoint + (0.1f * Vector2.left), bufferEndPoint + (0.1f * Vector2.right));
    }
    //
    // private void DrawGizmoArc(Vector3 start, Vector3 end)
    // {
    //     Gizmos.DrawLine(start, end);
    // }
    //
    
}