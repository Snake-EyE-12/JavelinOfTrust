using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterCollisionGimmick : ICharacterGimmick
    {
        [SerializeField] public bool IsUsingCollisionStandOnEdge;
        [SerializeField] public CharacterContact GroundContact = new();
        [SerializeField] public CharacterContact RoofContact = new();
        [SerializeField] public CharacterContact LeftWallContact = new();
        [SerializeField] public CharacterContact RightWallContact = new();
    }
    
    public class GroundCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.GroundContact.Contact && data.Velocity.Value.y < 0) data.Velocity.Value.y = 0;
        }

    }

    public class WallCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.LeftWallContact.Contact && data.Velocity.Value.x < 0) data.Velocity.Value.x = 0;
            if (data.RightWallContact.Contact && data.Velocity.Value.x > 0) data.Velocity.Value.x = 0;
        }

    }
    
    public class RoofCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.RoofContact.Update(data.RigidBody.transform);
        }
    }
    
    public class GroundCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.GroundContact.Update(data.RigidBody.transform);
        }
    }

    public class WallCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.LeftWallContact.Update(data.RigidBody.transform);
            data.RightWallContact.Update(data.RigidBody.transform);
        }
    }

}