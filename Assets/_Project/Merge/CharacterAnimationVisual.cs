using UnityEngine;

namespace _Project.Merge
{
    public class CharacterAnimationVisual : MonoBehaviour
    {
        public Vector2 _headPos;
        public Vector2 _bodyPos;
        public Vector2 _leftHandPos;
        public Vector2 _rightHandPos;
        public Vector2 _leftFootPos;
        public Vector2 _rightFootPos;

        [SerializeField] private Color color = Color.cyan;
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position + (Vector3)_headPos, 0.1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)_bodyPos, 0.1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)_leftHandPos, 0.1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)_rightHandPos, 0.1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)_leftFootPos, 0.1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)_rightFootPos, 0.1f);
        }
    }
}