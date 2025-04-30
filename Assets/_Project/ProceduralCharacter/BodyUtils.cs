
using System.Collections.Generic;
using UnityEngine;

public static class BodyUtils
{
    public static void Draw(this LineRenderer line, List<Vector3> points)
    {
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }

    public static List<Vector3> ToVector3List(this List<Vector2> points)
    {
        List<Vector3> vec3Points = new();
        for (int i = 0; i < points.Count; i++)
        {
            vec3Points.Add(new Vector3(points[i].x, points[i].y, 0));
        }
        return vec3Points;
    }
}
