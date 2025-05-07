using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterPhysicsGimmick : ICharacterGimmick
    {
        [SerializeField] public Rigidbody2D RigidBody;
        [SerializeField] public bool IsUsingPhysicsEuler;
        [SerializeField] public bool IsUsingPhysicsVerlet;
    }
}