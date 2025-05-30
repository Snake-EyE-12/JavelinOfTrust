using UnityEngine;
public class DrawGLLine : MonoBehaviour
{
    public Color lineColor = Color.red;

    Material lineMaterial;

    void Awake()
    {
        // must be called before trying to draw lines..
        CreateLineMaterial();
    }

    void CreateLineMaterial()
    {
        // Unity has a built-in shader that is useful for drawing simple colored things
        var shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
        
        lineMaterial.renderQueue = 3000;
        lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }


    // cannot call this on update, line wont be visible then.. and if used OnPostRender() thats works when attached to camera only
    void OnRenderObject()
    {
        Debug.Log("RENDERING");
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        
        GL.Begin(GL.LINES);
        GL.Color(lineColor);
        // start line from transform position
        GL.Vertex(transform.position);
        // end line 100 units forward from transform position
        GL.Vertex(transform.position + transform.up * 100);

        GL.End();
        GL.PopMatrix();
    }
}