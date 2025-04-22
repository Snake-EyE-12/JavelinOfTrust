using UnityEngine;

namespace CharacterController.Platformer
{
    public class Walk : PlayerFunctionality
    {
        [SerializeField] private float speed;

        

        protected override void GenerateConditions()
        {
            IfDo(
                CheckSpeed,
                () => speed = 0,
                10);
        }

        protected bool CheckSpeed()
        {
            if (blackboard.GetVariable("Speed", out float s))
            {
                speed = s;
                return true;
            }

            return false;
        }
    }
}