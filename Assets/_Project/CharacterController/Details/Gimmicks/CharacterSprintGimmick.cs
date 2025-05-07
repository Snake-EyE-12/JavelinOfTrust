using System;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterSprintGimmick : ICharacterGimmick
    {
        [SerializeField] public CharacterLocomotionState SprintLocomotion;
    }
    
    public class LocomotionSprintDetector : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.IsUsingSprint) return;
            if (data.Input.crouch.Pressed) data.ActiveLocomotion = data.CrouchLocomotion;
        }
    }

}