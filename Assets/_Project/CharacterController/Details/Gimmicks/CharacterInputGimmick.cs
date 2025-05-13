using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterInputGimmick : ICharacterGimmick
    {
        [SerializeField] public CharacterInput InputSystem;
        /**/
        [NonSerialized] public CharacterFrameInput Input;
        [NonSerialized] public Vector2 lastMovementDirection = Vector2.right;
    }
    
    public class InputSetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.input.Input = data.input.InputSystem.GetFrameInput();
            if(data.input.Input.direction != Vector2.zero) data.input.lastMovementDirection = data.input.Input.direction;
        }
    }
}