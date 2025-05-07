using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterAdjustmentGimmick : ICharacterGimmick // Precise Corrections
    {
        [SerializeField] public bool IsUsingAdjustmentAscendingGroundCornerClip;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingGroundCornerClip))] public CharacterCorrectionRay LeftGroundDetector = new();
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingGroundCornerClip))] public CharacterCorrectionRay RightGroundDetector = new();
        [SerializeField] public bool IsUsingAdjustmentAscendingRoofCornerClip;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingRoofCornerClip))] public CharacterCorrectionRay LeftRoofDetector = new();
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentAscendingRoofCornerClip))] public CharacterCorrectionRay RightRoofDetector = new();
        [SerializeField] public bool IsUsingAdjustmentLedgeLander;
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentLedgeLander))] public CharacterCorrectionRay LeftLedgeDetector = new();
        [SerializeField, AllowNesting, ShowIf(nameof(IsUsingAdjustmentLedgeLander))] public CharacterCorrectionRay RightLedgeDetector = new();
        /**/

    }
    
    public class GapPositionCorrector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.adjustment.IsUsingAdjustmentLedgeLander) return;
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            if (data.locomotion.Velocity.Value.y < 0 && !data.collision.GroundContact.Contact)
            {
                if (movementDirection == -1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.LeftLedgeDetector.origin, data.adjustment.LeftLedgeDetector.direction, data.adjustment.LeftLedgeDetector.direction.magnitude, data.adjustment.LeftLedgeDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.LeftLedgeDetector.origin + (Vector3)data.adjustment.LeftLedgeDetector.correction, data.adjustment.LeftLedgeDetector.direction, data.adjustment.LeftLedgeDetector.direction.magnitude, data.adjustment.LeftLedgeDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.LeftLedgeDetector.correction;
                }
                else if (movementDirection == 1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.RightLedgeDetector.origin, data.adjustment.RightLedgeDetector.direction, data.adjustment.RightLedgeDetector.direction.magnitude, data.adjustment.RightLedgeDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.RightLedgeDetector.origin + (Vector3)data.adjustment.RightLedgeDetector.correction, data.adjustment.RightLedgeDetector.direction, data.adjustment.RightLedgeDetector.direction.magnitude, data.adjustment.RightLedgeDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.RightLedgeDetector.correction;
                }
            }
        }
    }
    public class MakeJumpCorrector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.adjustment.IsUsingAdjustmentAscendingGroundCornerClip) return;
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            if (data.locomotion.Velocity.Value.y > 0 && !data.collision.GroundContact.Contact)
            {
                if (movementDirection == -1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.LeftGroundDetector.origin, data.adjustment.LeftGroundDetector.direction, data.adjustment.LeftGroundDetector.direction.magnitude, data.adjustment.LeftGroundDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.LeftGroundDetector.origin + (Vector3)data.adjustment.LeftGroundDetector.correction, data.adjustment.LeftGroundDetector.direction, data.adjustment.LeftGroundDetector.direction.magnitude, data.adjustment.LeftGroundDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.LeftGroundDetector.correction;
                }
                else if (movementDirection == 1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.RightGroundDetector.origin, data.adjustment.RightGroundDetector.direction, data.adjustment.RightGroundDetector.direction.magnitude, data.adjustment.RightGroundDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.RightGroundDetector.origin + (Vector3)data.adjustment.RightGroundDetector.correction, data.adjustment.RightGroundDetector.direction, data.adjustment.RightGroundDetector.direction.magnitude, data.adjustment.RightGroundDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.RightGroundDetector.correction;
                }
            }
        }

    }

    public class HeadCollisionAvoidance : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.adjustment.IsUsingAdjustmentAscendingRoofCornerClip) return;
            int movementDirection = MathUtils.Sign(data.input.Input.direction.x);
            if (data.jump.InJump && !data.jump.JumpEarlyRelease)
            {
                if (movementDirection != -1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.LeftRoofDetector.origin, data.adjustment.LeftRoofDetector.direction, data.adjustment.LeftRoofDetector.direction.magnitude, data.adjustment.LeftRoofDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)(data.adjustment.LeftRoofDetector.origin + data.adjustment.LeftRoofDetector.correction), data.adjustment.LeftRoofDetector.direction, data.adjustment.LeftRoofDetector.direction.magnitude, data.adjustment.LeftRoofDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.RightRoofDetector.correction;
                }
                else if (movementDirection != 1 && Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)data.adjustment.RightRoofDetector.origin, data.adjustment.RightRoofDetector.direction, data.adjustment.RightRoofDetector.direction.magnitude, data.adjustment.RightRoofDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.physics.RigidBody.transform.position + (Vector3)(data.adjustment.RightRoofDetector.origin + data.adjustment.RightRoofDetector.correction), data.adjustment.RightRoofDetector.direction, data.adjustment.RightRoofDetector.direction.magnitude, data.adjustment.RightRoofDetector.mask).Length == 0)
                {
                    data.physics.RigidBody.transform.position += (Vector3)data.adjustment.RightRoofDetector.correction;
                }
            }
        }

    }
}