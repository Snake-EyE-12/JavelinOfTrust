using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DrawWithGL : MonoBehaviour
{
    private static Material _lineMaterial;

    private void CreateLineMaterial()
    {
        if (!_lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _lineMaterial = new Material(shader);
            _lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            _lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    private void OnRenderObject()
    {
        if (Camera.current != Camera.main) return;
        CreateLineMaterial();
        _lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex(new Vector3(0.1f, 0.5f, 0));
        GL.Vertex(new Vector3(0.9f, 0.5f, 0));
        GL.End();

        GL.PopMatrix();
    }

}
