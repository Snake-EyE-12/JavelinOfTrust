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
            if (data.input.Input.sprint.Pressed) data.locomotion.ActiveLocomotion = data.sprint.SprintLocomotion;
        }
    }

}