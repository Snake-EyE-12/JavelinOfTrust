using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterCrouchGimmick : ICharacterGimmick
    {
        [SerializeField] public CharacterLocomotionState CrouchLocomotion;
    }
    
    public class LocomotionCrouchDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingCrouch) return;
            if (data.Input.crouch.Pressed) data.ActiveLocomotion = data.CrouchLocomotion;
        }
    }
}