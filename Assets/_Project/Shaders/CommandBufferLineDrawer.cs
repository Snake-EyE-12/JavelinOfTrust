using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferLineDrawer : MonoBehaviour
{
    public Camera cam;
    private CommandBuffer commandBuffer;
    private Material lineMaterial;
    private Mesh lineMesh;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        // Create line material
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;

        // Create simple mesh for line: two vertices, one line segment
        lineMesh = new Mesh();
        Vector3[] vertices = new Vector3[2] { Vector3.zero, Vector3.up };
        int[] indices = new int[2] { 0, 1 };
        lineMesh.vertices = vertices;
        lineMesh.SetIndices(indices, MeshTopology.Lines, 0);

        // Setup command buffer
        commandBuffer = new CommandBuffer();
        commandBuffer.name = "Draw Line";

        cam.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    void Update()
    {
        commandBuffer.Clear();

        // Draw the line mesh with the material, no transformation (identity)
        commandBuffer.DrawMesh(lineMesh, Matrix4x4.identity, lineMaterial);
    }

    private void OnDisable()
    {
        if (cam != null && commandBuffer != null)
        {
            cam.RemoveCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
        }
    }
}