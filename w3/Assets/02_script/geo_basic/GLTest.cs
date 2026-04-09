using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLTest : MonoBehaviour
{
    Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        _mat = MeshUtil.CreateMaterial(null);
    }

    // Update is called once per frame
    void OnPostRender()
    {
        if (!_mat)
            return;
        
        GL.PushMatrix();
        _mat.SetPass(0);
        GL.LoadOrtho();
        
        GL.Begin(GL.TRIANGLES);
        GL.Color(new Color(1, 0, 0, 1));
        GL.Vertex3(0.50F, 0.25F, 0);
        GL.Vertex3(0.25F, 0.25F, 0);
        GL.Vertex3(0.375F, 0.50F, 0);
        GL.End();
    }
}
