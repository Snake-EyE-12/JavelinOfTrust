using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterInputGimmick : ICharacterGimmick
    {
        [SerializeField] public CharacterInput InputSystem;
        /**/
        [HideInInspector] public CharacterFrameInput Input;
    }
    
    public class InputSetter : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.Input = data.InputSystem.GetFrameInput();
        }
    }
}