using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class StaticBodySegment : MonoBehaviour
{
    [SerializeField] private List<Transform> positions = new();
    [SerializeField] private LineRenderer line;

    private void Update()
    {
        Draw();
    }

    [Button]
    private void Draw()
    {
        List<Vector3> points = new();
        for(int i = positions.Count - 1; i >= 0; i--)
        {
            points.Add(positions[i].position);
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}