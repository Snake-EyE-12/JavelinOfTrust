using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class WalkControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [Serializable]
        public class AccelerationGimmick : Gimmick
        {
            [SerializeField, Min(0)] private float acceleration;
            [SerializeField, Min(0)] private float reversalAcceleration;
            [SerializeField, Min(0)] private float coastalDamping;
            [SerializeField, Min(0)] private float stopThreshold;

            [Prioritized]
            private void Accelerate()
            {
                if (board.GetVariable("Velocity", out Vector2 v))
                {
                    if (board.GetVariable("DirectionInput", out Vector2 d))
                    {
                        int movementDirection = PlayerBehaviorUtilities.Sign(d.x);
                        int velDirection = PlayerBehaviorUtilities.Sign(v.x);
                        if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
                        {
                            board.SetVariable("HorizontalAcceleration", acceleration);
                        }
                    }
                }
            }

            [Prioritized]
            private void Decelerate()
            {
                if (board.GetVariable("Velocity", out Vector2 v))
                {
                    if (board.GetVariable("DirectionInput", out Vector2 d))
                    {
                        int movementDirection = PlayerBehaviorUtilities.Sign(d.x);
                        int velDirection = PlayerBehaviorUtilities.Sign(v.x);
                        if (movementDirection != 0 && (velDirection == movementDirection || velDirection == 0))
                        {
                            board.SetVariable("HorizontalAcceleration", acceleration);
                        }
                    }
                }
            }
        }

        [Serializable]
        public class VelocityMovementGimmick : Gimmick
        {
            [SerializeField, Min(0)] private float speed;
            [SerializeField, Min(0)] private bool normalized;

            [Prioritized]
            private void Move()
            {

            }
        }

        [SerializeField] private VelocityMovementGimmick snapMovement;
        [SerializeField] private AccelerationGimmick dynamicAcceleration;

        protected override void OnLoad()
        {
            Load(snapMovement);
            Load(dynamicAcceleration);
        }
    }
}