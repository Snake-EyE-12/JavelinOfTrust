using System;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class CollisionControl : PlatformerPlayerBehavior
    {
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] private BoxCollider2D boxCollider;
        
        [field: SerializeField, Header("Collisions")] public CharacterContact GroundContact { get; set; }
        [field: SerializeField] public CharacterContact RoofContact { get; set; }
        [field: SerializeField] public CharacterContact LeftWallContact { get; set; }
        [field: SerializeField] public CharacterContact RightWallContact { get; set; }
        protected override void OnLoad()
        {
            //Do(UpdateGroundContact, 100);
            //Do(UpdateRoofContact, 100);
            //Do(UpdateLeftWallContact, 100);
            //Do(UpdateRightWallContact, 100);
        }

        private void UpdateGroundContact()
        {
            if(blackboard.GetVariable("Transform", out Transform character))
            {
                GroundContact.Update(character);
            }
        }
        
        private void UpdateRoofContact()
        {
            if(blackboard.GetVariable("Transform", out Transform character))
            {
                RoofContact.Update(character);
            }
        }
        
        private void UpdateLeftWallContact()
        {
            if(blackboard.GetVariable("Transform", out Transform character))
            {
                LeftWallContact.Update(character);
            }
        }
        
        private void UpdateRightWallContact()
        {
            if(blackboard.GetVariable("Transform", out Transform character))
            {
                RightWallContact.Update(character);
            }
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
        public void Update(Transform character)
        {
            bool contacting = Physics2D.OverlapBoxAll(character.position + bounds.center, bounds.size, character.rotation.eulerAngles.z, mask).Length > 0;
            if (inContact && !contacting)
            {
                lastTimeInContact = Time.time;
            }
            
            newlyContacted = !inContact && contacting;
            
            inContact = contacting;
        }
        
        public bool Contact => inContact;
        public bool EnteredContact => newlyContacted;
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