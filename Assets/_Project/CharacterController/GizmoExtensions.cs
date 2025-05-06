using UnityEngine;

public static class GizmoExtensions
{
    public static void DrawBounds(Bounds bounds, Vector3 point)
    {
        Gizmos.DrawWireCube(point + bounds.center, bounds.size);
    }
}
