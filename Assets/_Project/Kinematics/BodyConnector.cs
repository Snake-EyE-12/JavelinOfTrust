using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform start, end;

    private void Update()
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(start.position);
        points.Add(end.position);
        line.Draw(points);
    }
}
