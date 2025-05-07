using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterSlopeGimmick : ICharacterGimmick
    {
        [SerializeField] public float MaximumClimbAngle;
        [SerializeField] public float MaximumSlideDownAngle;
        [SerializeField] public bool StayOnGround;
        [SerializeField] public float SlopeCheckDistance;
    }
}