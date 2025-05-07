using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterAdjustmentGimmick : ICharacterGimmick
    {
        [SerializeField] public bool IsUsingAdjustmentAscendingGroundCornerClip;
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
            if (!data.IsUsingAdjustmentLedgeLander) return;
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            if (data.Velocity.Value.y < 0 && !data.GroundContact.Contact)
            {
                if (movementDirection == -1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftLedgeDetector.origin, data.LeftLedgeDetector.direction, data.LeftLedgeDetector.direction.magnitude, data.LeftLedgeDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftLedgeDetector.origin + (Vector3)data.LeftLedgeDetector.correction, data.LeftLedgeDetector.direction, data.LeftLedgeDetector.direction.magnitude, data.LeftLedgeDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.LeftLedgeDetector.correction;
                }
                else if (movementDirection == 1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightLedgeDetector.origin, data.RightLedgeDetector.direction, data.RightLedgeDetector.direction.magnitude, data.RightLedgeDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightLedgeDetector.origin + (Vector3)data.RightLedgeDetector.correction, data.RightLedgeDetector.direction, data.RightLedgeDetector.direction.magnitude, data.RightLedgeDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightLedgeDetector.correction;
                }
            }
        }

    }

    public class HeadCollisionAvoidance : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingAdjustmentAscendingRoofCornerClip) return;
            int movementDirection = MathUtils.Sign(data.Input.direction.x);
            if (data.InJump && !data.JumpEarlyRelease)
            {
                if (movementDirection != -1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.LeftRoofDetector.origin, data.LeftRoofDetector.direction, data.LeftRoofDetector.direction.magnitude, data.LeftRoofDetector.mask).Length > 0 &&
                    Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)(data.LeftRoofDetector.origin + data.LeftRoofDetector.correction), data.LeftRoofDetector.direction, data.LeftRoofDetector.direction.magnitude, data.LeftRoofDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightRoofDetector.correction;
                }
                else if (movementDirection != 1 && Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)data.RightRoofDetector.origin, data.RightRoofDetector.direction, data.RightRoofDetector.direction.magnitude, data.RightRoofDetector.mask).Length > 0 &&
                         Physics2D.RaycastAll(data.RigidBody.transform.position + (Vector3)(data.RightRoofDetector.origin + data.RightRoofDetector.correction), data.RightRoofDetector.direction, data.RightRoofDetector.direction.magnitude, data.RightRoofDetector.mask).Length == 0)
                {
                    data.RigidBody.transform.position += (Vector3)data.RightRoofDetector.correction;
                }
            }
        }

    }
}