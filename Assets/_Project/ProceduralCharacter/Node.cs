using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCharacter
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private float distance;
        [SerializeField] private LineRenderer lineRenderer;
        private void Update()
        {
            if (Vector2.Distance(transform.position, anchor.position) > distance)
            {
                transform.position = GetNewPosition();
            }
        }

        private Vector3 GetNewPosition()
        {
            return anchor.position + (transform.position - anchor.position).normalized * distance;
        }

        private void RenderLine()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, anchor.position);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.03f);
        }
    }
}