using System;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class CollisionControlPlatformBehavior : PlatformerPlayerBehavior
    {
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] private BoxCollider2D boxCollider;

        [Serializable]
        public class CollisionDetectionGimmick : Gimmick
        {
            [SerializeField] public CharacterContact contact;
            
            [Prioritized]
            private void UpdateContact()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                contact.Update(data.Transform);
                if(data.Velocity.X > 100f) Debug.Log("Warning");
            }
        }

        [Serializable]
        public class VelocityStopGimmick : Gimmick
        {
            [Prioritized]
            private void ZeroGround()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.GroundContact.Contact && data.Velocity.Y < 0)
                {
                    data.Velocity.multiplier.y = 0;
                    data.Acceleration.multiplier.y = 0;
                }
            }

            [Prioritized]
            private void ZeroWall()
            {
                ICustomCharacterSettingsData data = board.Value<CustomCharacterSettingsData>();
                if (data.LeftWallContact.Contact && data.Velocity.X < 0)
                {
                    data.Velocity.multiplier.x = 0;
                    data.Acceleration.multiplier.x = 0;
                }

                if (data.RightWallContact.Contact && data.Velocity.X > 0)
                {
                    data.Velocity.multiplier.x = 0;
                    data.Acceleration.multiplier.x = 0;
                }
            }
        }
        
        [SerializeField] private CollisionDetectionGimmick groundContact;
        [SerializeField] private CollisionDetectionGimmick roofContact;
        [SerializeField] private CollisionDetectionGimmick leftWallContact;
        [SerializeField] private CollisionDetectionGimmick rightWallContact;
        [SerializeField] private VelocityStopGimmick velocityStopper;
        protected override void OnLoad()
        {
            Load(groundContact);
            Load(roofContact);
            Load(leftWallContact);
            Load(rightWallContact);
            Load(velocityStopper);
            ICustomCharacterSettingsData data = blackboard.Value<CustomCharacterSettingsData>();
            data.GroundContact = groundContact.contact;
            data.RoofContact = roofContact.contact;
            data.LeftWallContact = leftWallContact.contact;
            data.RightWallContact = rightWallContact.contact;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if(groundContact.Active) groundContact.contact.DrawGizmos(transform);
            if(roofContact.Active) roofContact.contact.DrawGizmos(transform);
            if(leftWallContact.Active) leftWallContact.contact.DrawGizmos(transform);
            if(rightWallContact.Active) rightWallContact.contact.DrawGizmos(transform);
        }
    }
    
    
    
    
    [Serializable]
    public class CharacterContact
    {
        [field: SerializeField] public Bounds bounds { get; private set; }
        [field: SerializeField] public LayerMask mask { get; private set; }
        private bool inContact;
        private float lastTimeInContact;
        private bool newlyContacted;
        private float timeOfContactEntered;
        public void Update(Transform character)
        {
            bool contacting = Physics2D.OverlapBoxAll(character.position + bounds.center, bounds.size, character.rotation.eulerAngles.z, mask).Length > 0;
            if (inContact && !contacting)
            {
                lastTimeInContact = Time.time;
            }
            
            newlyContacted = !inContact && contacting;
            if (newlyContacted) timeOfContactEntered = Time.time;
            
            inContact = contacting;
        }
        public void DrawGizmos(Transform character)
        {
            Gizmos.DrawWireCube(character.position + bounds.center, bounds.size);
        }
        
        public bool Contact => inContact;
        public bool EnteredContact => newlyContacted;
        public float TimeOfContactEnter => timeOfContactEntered;
        public float TimeOfContactExit => lastTimeInContact;
    }
    
    [Serializable]
    public class CharacterCorrectionRay
    {
        public LayerMask mask;
        public Vector2 origin;
        public Vector2 direction;
        public Vector2 correction;
    }
}