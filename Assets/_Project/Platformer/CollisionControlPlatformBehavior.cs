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
            }
        }
        
        [SerializeField] private CollisionDetectionGimmick GroundContact;
        [SerializeField] private CollisionDetectionGimmick RoofContact;
        [SerializeField] private CollisionDetectionGimmick LeftWallContact;
        [SerializeField] private CollisionDetectionGimmick RightWallContact;
        protected override void OnLoad()
        {
            Load(GroundContact);
            Load(RoofContact);
            Load(LeftWallContact);
            Load(RightWallContact);
            ICustomCharacterSettingsData data = blackboard.Value<CustomCharacterSettingsData>();
            data.GroundContact = GroundContact.contact;
            data.RoofContact = RoofContact.contact;
            data.LeftWallContact = LeftWallContact.contact;
            data.RightWallContact = RightWallContact.contact;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if(GroundContact.Active) GroundContact.contact.DrawGizmos(transform);
            if(RoofContact.Active) RoofContact.contact.DrawGizmos(transform);
            if(LeftWallContact.Active) LeftWallContact.contact.DrawGizmos(transform);
            if(RightWallContact.Active) RightWallContact.contact.DrawGizmos(transform);
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