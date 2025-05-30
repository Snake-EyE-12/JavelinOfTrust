using UnityEngine;

public class GLLineDrawer : MonoBehaviour
{
    public Material lineMaterial; // Create a material in the editor and assign it to this script
    public Vector3[] points;

    void OnPostRender()
    {
        // Set the material and color
        lineMaterial.SetPass(0);
        GL.Color(lineMaterial.color);

        // Begin drawing lines
        GL.Begin(GL.LINES);

        // Draw each line segment
        for (int i = 1; i < points.Length; i++)
        {
            GL.Vertex3(points[i - 1].x, points[i - 1].y, points[i - 1].z);
            GL.Vertex3(points[i].x, points[i].y, points[i].z);
        }

        // End drawing
        GL.End();
    }
}