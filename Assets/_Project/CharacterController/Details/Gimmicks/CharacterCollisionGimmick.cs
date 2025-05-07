using System;
using UnityEngine;

namespace CharacterProcess.Gimmicks
{
    [Serializable]
    public class CharacterCollisionGimmick : ICharacterGimmick
    {
        //[SerializeField] public bool IsUsingCollisionStandOnEdge;
        [SerializeField] public CharacterContact GroundContact = new();
        [SerializeField] public CharacterContact RoofContact = new();
        [SerializeField] public CharacterContact LeftWallContact = new();
        [SerializeField] public CharacterContact RightWallContact = new();
    }
    
    public class GroundCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.collision.GroundContact.Contact && data.locomotion.Velocity.Value.y < 0) data.locomotion.Velocity.Value.y = 0;
        }

    }

    public class WallCollisionVelocityZeroer : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            if (data.collision.LeftWallContact.Contact && data.locomotion.Velocity.Value.x < 0) data.locomotion.Velocity.Value.x = 0;
            if (data.collision.RightWallContact.Contact && data.locomotion.Velocity.Value.x > 0) data.locomotion.Velocity.Value.x = 0;
        }

    }
    
    public class RoofCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.collision.RoofContact.Update(data.physics.RigidBody.transform);
        }
    }
    
    public class GroundCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.collision.GroundContact.Update(data.physics.RigidBody.transform);
        }
    }

    public class WallCollisionDetection : BaseCharacterProcessor
    {
        public override void Operate(CharacterDataSettings data)
        {
            data.collision.LeftWallContact.Update(data.physics.RigidBody.transform);
            data.collision.RightWallContact.Update(data.physics.RigidBody.transform);
        }
    }

}