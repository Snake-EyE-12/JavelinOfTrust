using System;
using NaughtyAttributes;
using UnityEngine;

namespace ProceduralCharacter
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private LineRenderer lineRenderer;

        private void Update()
        {
            Realign();
        }
        [Button]
        private void Realign()
        {
            if (lineRenderer == null || start == null || end == null) return;
            lineRenderer.SetPositions(new Vector3[]{start.position, end.position});
        }
    }
}