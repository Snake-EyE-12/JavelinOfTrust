using UnityEngine;

namespace CharacterController.Platformer
{
    public class PhysicsControl : PlayerBehavior
    {
        [SerializeField] private Rigidbody2D rigidBody;
        protected override void LoadGimmicks()
        {
            blackboard.SetVariable("Velocity", Vector2.zero);
            blackboard.SetVariable("Acceleration", Vector2.zero);
            
            //Do(ApplyAcceleration, 997);
            //Do(LoadVelocity, 1000);
        }

        private void ApplyAcceleration()
        {
            if (blackboard.GetVariable("Acceleration", out Vector2 a))
            {
                if(blackboard.GetVariable("Velocity", out Vector2 v))
                {
                    if(blackboard.GetVariable("HorizontalAcceleration", out float h))
                    {
                        blackboard.SetVariable("Velocity", v + a + (h * Vector2.right));
                        blackboard.SetVariable("Acceleration", Vector2.zero);
                        blackboard.SetVariable("HorizontalAcceleration", Vector2.zero);
                    }
                }
            }
        }
        private void LoadVelocity()
        {
            if (blackboard.GetVariable("Velocity", out Vector2 v))
            {
                rigidBody.linearVelocity = v;
            }
        }
    }
}