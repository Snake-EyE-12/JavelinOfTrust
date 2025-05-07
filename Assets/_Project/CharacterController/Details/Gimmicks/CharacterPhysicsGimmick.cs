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
    
    public class PhysicsVelocityApplicator : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (!data.physics.IsUsingPhysicsEuler) return;
            data.physics.RigidBody.linearVelocity = data.locomotion.Velocity.Value;
        }
    }
}