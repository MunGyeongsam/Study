using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02_draw_cube : MonoBehaviour
{

    [SerializeField] private Texture texture;

    Material mat;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        float size = 1F;
        float hs = size * 0.5F;

        mesh = new Mesh();

        var vtx = new Vector3[]
        {
            new Vector3(-hs, hs, hs),
            new Vector3( hs, hs, hs),
            new Vector3(-hs, hs,-hs),
            new Vector3( hs, hs,-hs),

            new Vector3(-hs, hs,-hs),
            new Vector3( hs, hs,-hs),
            new Vector3(-hs,-hs,-hs),
            new Vector3( hs,-hs,-hs),

            new Vector3( hs, hs,-hs),
            new Vector3( hs, hs, hs),
            new Vector3( hs,-hs,-hs),
            new Vector3( hs,-hs, hs),
        };
        mesh.vertices = vtx;

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),

            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),

            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
        };
        mesh.uv = uv;

        int[] triangles = new int[]
        {
            4*0+0, 4*0+1, 4*0+2,
            4*0+1, 4*0+3, 4*0+2,

            4*1+0, 4*1+1, 4*1+2,
            4*1+1, 4*1+3, 4*1+2,

            4*2+0, 4*2+1, 4*2+2,
            4*2+1, 4*2+3, 4*2+2,
        };
        mesh.triangles = triangles;

        Color[] colors = new Color[]
        {
            Color.red,
            Color.white,
            Color.white,
            Color.white,

            Color.green,
            Color.white,
            Color.white,
            Color.white,

            Color.blue,
            Color.white,
            Color.white,
            Color.white,
        };
        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mat = new Material(Shader.Find("Custom/TexWithColor"));

        mat.SetTexture("_MainTex", texture);
        mat.renderQueue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        Graphics.DrawMesh(mesh, pos, Quaternion.identity, mat, 0);
    }
}
