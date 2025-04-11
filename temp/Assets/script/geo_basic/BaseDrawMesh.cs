using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDrawMesh : MonoBehaviour
{
    Mesh mesh;

    [SerializeField] private Texture texture;

    private LineRenderer lineRenderer;

    protected IDraw? _draw = null;


    protected virtual void StepVertex(Mesh mesh) { throw new System.Exception("to be implemented"); }
    protected virtual void StepUv(Mesh mesh) { throw new System.Exception("to be implemented"); }
    protected virtual void StepColor(Mesh mesh) { throw new System.Exception("to be implemented"); }
    protected virtual void StepTriangle(Mesh mesh) { throw new System.Exception("to be implemented"); }

    protected virtual void StepDrawSetting(Texture texture) { }
    protected virtual void ChangeDraw(int n, Texture texture) { }

    public void TempleteMethodeBuild()
    {
        mesh = new Mesh();

        StepVertex(mesh);
        StepUv(mesh);
        StepColor(mesh);
        StepTriangle(mesh);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        StepDrawSetting(texture);
    }

    protected virtual void Start0() { }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        Start0();
        TempleteMethodeBuild();

        lineRenderer.startWidth = 0.01F;
        lineRenderer.endWidth = 0.01F;
        lineRenderer.material = _draw.Material;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        lineRenderer.positionCount = mesh.vertexCount;
        lineRenderer.SetPositions(mesh.vertices);
        //lineRenderer

        lineRenderer.enabled = false;

    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ChangeDraw(1, texture);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            ChangeDraw(2, texture);
        }
        _draw?.Draw(mesh, transform.position);
    }

    public void OnPostRender()
    {

        var mat = _draw?.Material;

        if (mat == null)
            return;

        mat.SetPass(0);

        var tri = mesh.triangles;
        var vtx = mesh.vertices;

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        for (int i=0; i<tri.Length; i+=3)
        {
            int _0 = tri[i];
            int _1 = tri[i+1];
            int _2 = tri[i+2];

            GL.Vertex(vtx[_0]); 
            GL.Vertex(vtx[_1]);

            GL.Vertex(vtx[_1]);
            GL.Vertex(vtx[_2]);

            GL.Vertex(vtx[_2]);
            GL.Vertex(vtx[_0]);
        }
        GL.End();
    }
}


public interface IDraw
{
    void Draw(Mesh mesh, Vector3 pos);
    void Draw(Mesh mesh, Transform transform);
    Material Material { get; }
}

public class DrawCustom : IDraw
{
    Material material;

    Material IDraw.Material => material;
    
    public DrawCustom(Texture texture)
    {
        material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        material.renderQueue = 0;
    }

    public void Draw(Mesh mesh, Vector3 pos)
    {
        Graphics.DrawMesh(mesh, pos, Quaternion.identity, material, 0);
    }
    public void Draw(Mesh mesh, Transform transform)
    {
        Graphics.DrawMesh(mesh, transform.localToWorldMatrix, material, 0);
    }
}

public class DrawStandard : IDraw
{
    Material material;


    Material IDraw.Material => material;

    public DrawStandard(Texture texture)
    {
        material = new Material(Shader.Find("Standard"));

        material.SetTexture("_MainTex", texture);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 0;
    }

    public void Draw(Mesh mesh, Vector3 pos)
    {
        Graphics.DrawMesh(mesh, pos, Quaternion.identity, material, 0);
    }
    public void Draw(Mesh mesh, Transform transform)
    {
        Graphics.DrawMesh(mesh, transform.localToWorldMatrix, material, 0);
    }
}