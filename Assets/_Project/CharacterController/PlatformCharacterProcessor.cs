// using System;
// using UnityEngine;
//
// public abstract class PlatformCharacterProcessor : BaseProcessor<ICharacterSettingsData>
// {
// }
//
// public class PCP_ : PlatformCharacterProcessor
// {
//     public override void Process(ICharacterSettingsData data)
//     {
//         
//         
//         base.Process(data);
//     }
// }
//
//
// public class PCP_GravityApplicator : PlatformCharacterProcessor
// {
//     public override void Process(ICharacterSettingsData data)
//     {
//         data.Acceleration += data.CalculatedGravity * data.GravityMultiplier;
//         data.CalculatedGravity = Vector2.zero;
//         data.GravityMultiplier = 1;
//         
//         base.Process(data);
//     }
// }
// public class PCP_AccelerationApplicator : PlatformCharacterProcessor
// {
//     public override void Process(ICharacterSettingsData data)
//     {
//         data.Velocity += data.Acceleration * Time.deltaTime;
//         data.Acceleration = Vector2.zero;
//         
//         base.Process(data);
//     }
// }
// public class PCP_VelocityApplicator : PlatformCharacterProcessor
// {
//     public override void Process(ICharacterSettingsData data)
//     {
//         data.RigidBody.linearVelocity = data.Velocity;
//         
//         base.Process(data);
//     }
// }
// public class PCP_HorizontalAcceleration : PlatformCharacterProcessor
// {
//     public override void Process(ICharacterSettingsData data)
//     {
//         int movementDirection = Utils.Sign(data.InputSystem.GetFrameInput().InputDirection.Direction.x);
//         int velDirection = Utils.Sign(data.Velocity.x);
//         if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
//         {
//             float acc = data.WalkAcceleration;
//             if (data.InputSystem.GetFrameInput().Sprint.pressed) acc = data.SprintAcceleration;
//             if (data.InputSystem.GetFrameInput().Crouch.pressed) acc = data.CrouchAcceleration;
//
//             //data.Acceleration += acc;
//         }
//         
//         base.Process(data);
//     }
// }
// public class PCP_HorizontalVelocityApplicator : BaseProcessor<ICharacterSettingsData>
// {
//     // public override void Process(ICharacterSettingsData data)
//     // {
//     //     int movementDirection = Utils.Sign(data.InputSystem.GetFrameInput().InputDirection.Direction.x);
//     //     int velDirection = Utils.Sign(data.Velocity.x);
//     //
//     //     if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
//     //     {
//     //         data.acceleration.x += data.walkAcceleration * data.walkXAccelerationMultiplier * movementDirection;
//     //         data.Acceleration.x += data.
//     //     }
//     //
//     //     data.walkXAccelerationMultiplier = 1;
//     //     base.Process(data);
//     // }
// }
// public class WalkAcceleration : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         int velDirection = Utils.Sign(data.velocity.x);
//
//         // if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
//         // {
//         //     data.acceleration.x += data.walkAcceleration * data.walkXAccelerationMultiplier * movementDirection;
//         // }
//
//         data.walkXAccelerationMultiplier = 1;
//         base.Process(data);
//     }
// }
//
// public class AirControlXAccelerationMultiplier : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if(data.inJump) data.walkXAccelerationMultiplier *= data.airControlXAccelerationMultiplier;
//         
//         base.Process(data);
//     }
// }
//
//
// public class WalkDeceleration : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         int velDirection = Utils.Sign(data.velocity.x);
//
//         // if (movementDirection != 0 && velDirection != 0 && movementDirection != velDirection)
//         // {
//         //     data.acceleration.x += data.walkDeceleration * -velDirection;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class StoppingDamping : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         int velDirection = Utils.Sign(data.velocity.x);
//
//         // if (movementDirection == 0 && velDirection != 0)
//         // {
//         //     data.velocity.x *= data.stoppingDamping;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class MinimumStopXVelocity : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // if (MathUtils.Sign(data.input.direction.x) == 0 && data.velocity.x < data.minimumStopXVelocity && data.velocity.x > -data.minimumStopXVelocity)
//         // {
//         //     data.velocity.x = 0;
//         //     data.acceleration.x = 0;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class GravityApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         data.acceleration += data.gravity * data.gravityMultiplier;
//         data.gravityMultiplier = Vector2.one;
//         base.Process(data);
//     }
// }
//
// public class GroundCollisionVelocityZeroer : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.groundContact.Contact && data.velocity.y < 0) data.velocity.y = 0;
//         
//         base.Process(data);
//     }
//    
// }
//
// public class WallCollisionVelocityZeroer : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.leftWallContact.Contact && data.velocity.x < 0) data.velocity.x = 0;
//         if (data.rightWallContact.Contact && data.velocity.x > 0) data.velocity.x = 0;
//         
//         base.Process(data);
//     }
//    
// }
//
//
// public class GroundCollisionDetection : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // data.groundContact.Update(
//         //         Physics2D.OverlapBoxAll(data.transform.position + data.groundContact.bounds.center, data.groundContact.bounds.size, 0f, data.groundContact.mask).Length > 0
//         //     );
//         
//         base.Process(data);
//     }
// }
//
// public class WallCollisionDetection : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         bool leftWallContact = Physics2D.OverlapBoxAll(data.transform.position + data.leftWallContact.bounds.center, data.leftWallContact.bounds.size, 0f, data.leftWallContact.mask).Length > 0;
//         bool rightWallContact = Physics2D.OverlapBoxAll(data.transform.position + data.rightWallContact.bounds.center, data.rightWallContact.bounds.size, 0f, data.rightWallContact.mask).Length > 0;
//         
//         //data.leftWallContact.Update(leftWallContact);
//         //data.rightWallContact.Update(rightWallContact);
//         
//         base.Process(data);
//     }
// }
//
// public class FallSpeedClamper : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         float maxFallSpeed = data.maxFallSpeed;
//         if (data.inJumpArc) maxFallSpeed = data.maxInJumpFallSpeed;
//         if (data.velocity.y < -maxFallSpeed) data.velocity.y = -maxFallSpeed;
//         
//         base.Process(data);
//     }
// }
// //
// // public class CalculateWalkState : CharacterProcessor
// // {
// //     public override void Process(CharacterData data)
// //     {
// //         
// //         
// //         base.Process(data);
// //     }
// // }
//
// public class WalkSpeedClamper : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.velocity.x > data.maxWalkSpeed) data.velocity.x = data.maxWalkSpeed;
//         if (data.velocity.x < -data.maxWalkSpeed) data.velocity.x = -data.maxWalkSpeed;
//
//         base.Process(data);
//     }
//
// }
//
// public class JumpBufferer : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //if(data.input.jumpStarted) data.timeOfJumpPress = Time.time;
//         
//         base.Process(data);
//     }
// }
//
// public class ImmediateDescendOnReleaseJump : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // if (data.input.jumpEnded && data.inJump)
//         // {
//         //     if (data.velocity.y > 0) data.velocity.y = 0;
//         //     if (data.acceleration.y > 0) data.acceleration.y = 0;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class JumpOnGroundInformant : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.timeOfJumpPress + data.jumpBufferTime > Time.time && data.groundContact.Contact)
//         {
//             data.canJump = true;
//         }
//         
//         base.Process(data);
//     }
// }
//
// public class GroundContactJumpResetter : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.groundContact.Contact) data.inJump = false;
//         
//         base.Process(data);
//     }
// }
//
// public class JumpCoyoteInformant : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // if (data.input.jumpStarted && data.groundContact.TimeOfContactExit + data.jumpCoyoteTime > Time.time)
//         // {
//         //     data.canJump = true;
//         // }
//         
//         base.Process(data);
//     }
//     
// }
// public class AttemptJumpApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.canJump && !data.inJump)
//         {
//             data.inJump = true;
//             data.startingJumpPoint = data.transform.position;
//             data.acceleration.y = 0;
//             data.velocity.y = data.jumpForce * data.runningStartBonus.Evaluate(Mathf.Abs(data.velocity.x));
//             data.timeOfJumpStart = Time.time;
//         }
//
//         data.canJump = false;
//         
//         base.Process(data);
//     }
// }
//
//
// public class GravityJumpDescendAccelerator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if(data.inJump && data.velocity.y < 0) data.acceleration.y -= data.jumpDescendAcceleration;
//         
//         base.Process(data);
//     }
// }
// public class ApexCalculator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         data.inApex = data.inJump && Mathf.Abs(data.velocity.y) < data.apexYVelocityThreshold;
//         
//         base.Process(data);
//     }
// }
//
// public class ApexXVelocityApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.inApex)
//         {
//             //data.acceleration.x += data.apexBonusXAcceleration * MathUtils.Sign(data.input.direction.x);
//         }
//         
//         base.Process(data);
//     }
// }
//
// public class ApexAntiGravityMultiplierApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if(data.inApex) data.gravityMultiplier *= data.apexAntiGravityMultiplier;
//         
//         base.Process(data);
//     }
// }
//
// public class EarlyReleaseGravityMultiplierApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if (data.earlyReleased)
//         {
//             data.gravityMultiplier *= data.earlyReleaseGravityMultiplier;
//         }
//         
//         base.Process(data);
//     }
// }
//
// public class EarlyReleaseCalculator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //if(data.input.jumpEnded && data.inJump) data.earlyReleased = true;
//         if (data.groundContact.Contact) data.earlyReleased = false;
//         
//         base.Process(data);
//     }
// }
//
// public class RoofCollisionVelocityZeroer : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         if(data.roofContact.Contact && data.velocity.y > 0) data.velocity.y = 0;
//         
//         base.Process(data);
//     }
// }
//
// public class RoofCollisionDetection : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // data.roofContact.Update(
//         //     Physics2D.OverlapBoxAll(data.transform.position + data.roofContact.bounds.center, data.roofContact.bounds.size, 0f, data.roofContact.mask).Length > 0
//         // );
//         
//         base.Process(data);
//     }
// }
//
// public class GapPositionCorrector : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         if (data.velocity.y < 0 && !data.groundContact.Contact)
//         {
//             // if (movementDirection == -1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedLeftJump.origin, data.missedLeftJump.direction, data.missedLeftJump.direction.magnitude, data.missedLeftJump.mask).Length > 0 &&
//             //     Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedLeftJump.origin + (Vector3)data.missedLeftJump.correction, data.missedLeftJump.direction, data.missedLeftJump.direction.magnitude, data.missedLeftJump.mask).Length == 0)
//             // {
//             //     data.transform.position += (Vector3)data.missedLeftJump.correction;
//             // }
//             // else if (movementDirection == 1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedRightJump.origin, data.missedRightJump.direction, data.missedRightJump.direction.magnitude, data.missedRightJump.mask).Length > 0 &&
//             //          Physics2D.RaycastAll(data.transform.position + (Vector3)data.missedRightJump.origin + (Vector3)data.missedRightJump.correction, data.missedRightJump.direction, data.missedRightJump.direction.magnitude, data.missedRightJump.mask).Length == 0)
//             // {
//             //     data.transform.position += (Vector3)data.missedRightJump.correction;
//             // }
//         }
//         
//         base.Process(data);
//     }
//     
// }
//
// public class HeadCollisionAvoidance : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         if (data.inJump && !data.earlyReleased)
//         {
//             // if (movementDirection != -1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.leftHeadAvoidance.origin, data.leftHeadAvoidance.direction, data.leftHeadAvoidance.direction.magnitude).Length > 0 &&
//             //     Physics2D.RaycastAll(data.transform.position + (Vector3)(data.leftHeadAvoidance.origin + data.leftHeadAvoidance.correction), data.leftHeadAvoidance.direction, data.leftHeadAvoidance.direction.magnitude).Length == 0)
//             // {
//             //     data.transform.position += (Vector3)data.leftHeadAvoidance.correction;
//             // }
//             // else if (movementDirection != 1 && Physics2D.RaycastAll(data.transform.position + (Vector3)data.rightHeadAvoidance.origin, data.rightHeadAvoidance.direction, data.rightHeadAvoidance.direction.magnitude).Length > 0 &&
//             //          Physics2D.RaycastAll(data.transform.position + (Vector3)(data.rightHeadAvoidance.origin + data.rightHeadAvoidance.correction), data.rightHeadAvoidance.direction, data.rightHeadAvoidance.direction.magnitude).Length == 0)
//             // {
//             //     data.transform.position += (Vector3)data.rightHeadAvoidance.correction;
//             // }
//         }
//         
//         base.Process(data);
//     }
//     
// }
//
// public class LandingAccelerationFrictionApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         int velocityDirection = Utils.Sign(data.velocity.x);
//         // if (movementDirection != 0 && data.groundContact.EnteredContact && movementDirection != velocityDirection)
//         // {
//         //     data.walkXAccelerationMultiplier *= data.landingAccelerationFrictionMultiplier;
//         // }
//         
//         base.Process(data);
//     }
//     
// }
// public class LandingVelocityDampingApplicator : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         //int movementDirection = MathUtils.Sign(data.input.direction.x);
//         int velocityDirection = Utils.Sign(data.velocity.x);
//         // if (movementDirection != 0 && data.groundContact.EnteredContact && movementDirection != velocityDirection)
//         // {
//         //     data.velocity.x *= data.landingVelocityDampingMultiplier;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class CalculateWithinJumpArc : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         data.inJumpArc = data.inJump && data.transform.position.y > data.startingJumpPoint.y;
//         
//         base.Process(data);
//     }
// }
//
// public class BeginChargedAttack : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // if (data.input.attackStarted && data.inventory.IsHoldingObject())
//         // {
//         //     data.timeOfAttackStart = Time.time;
//         // }
//         
//         base.Process(data);
//     }
// }
//
// public class Attack : CharacterProcessor
// {
//     public override void Process(CharacterData data)
//     {
//         // if (data.input.attackEnded && data.inventory.IsHoldingObject())
//         // {
//         //     Vector2 throwDirection = data.input.direction;
//         //     if (throwDirection == Vector2.zero) throwDirection.x = data.facingDirection;
//         //     data.inventory.UseObject(Time.time - data.timeOfAttackStart, data.velocity, data.transform, throwDirection.normalized);
//         // }
//         
//         base.Process(data);
//     }
// }
