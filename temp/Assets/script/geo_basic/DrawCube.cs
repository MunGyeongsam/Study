using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCube : BaseDrawMesh
{
    [SerializeField] float _size = 1F;

    protected override void StepVertex(Mesh mesh)
    {
        var hs = _size * 0.5F;

        mesh.vertices = new Vector3[]
        {
            //top
            new Vector3 (-hs, hs, hs),
            new Vector3 ( hs, hs, hs),
            new Vector3 (-hs, hs,-hs),
            new Vector3 ( hs, hs,-hs),

            //front
            new Vector3 (-hs, hs,-hs),
            new Vector3 ( hs, hs,-hs),
            new Vector3 (-hs,-hs,-hs),
            new Vector3 ( hs,-hs,-hs),

            //bottom
            new Vector3 (-hs,-hs,-hs),
            new Vector3 ( hs,-hs,-hs),
            new Vector3 (-hs,-hs, hs),
            new Vector3 ( hs,-hs, hs),

            //left
            new Vector3 (-hs, hs, hs),
            new Vector3 (-hs, hs,-hs),
            new Vector3 (-hs,-hs, hs),
            new Vector3 (-hs,-hs,-hs),

            //right
            new Vector3 ( hs, hs,-hs),
            new Vector3 ( hs, hs, hs),
            new Vector3 ( hs,-hs,-hs),
            new Vector3 ( hs,-hs, hs),

            //back
            new Vector3 ( hs, hs, hs),
            new Vector3 (-hs, hs, hs),
            new Vector3 ( hs,-hs, hs),
            new Vector3 (-hs,-hs, hs),
        };
    }

    protected override void StepUv(Mesh mesh)
    {
        var uvs = new Vector2[4 * 6];
        for(int i= 0; i < 4*6; i+=4)
        {
            uvs[i + 0] = new Vector3(0, 1);
            uvs[i + 1] = new Vector3(1, 1);
            uvs[i + 2] = new Vector3(0, 0);
            uvs[i + 3] = new Vector3(1, 0);
        }

        mesh.uv = uvs;
    }

    protected override void StepColor(Mesh mesh)
    {
        Color[] colors = new Color[4 * 6];
        for (int i = 0; i < 6; i += 4)
        {
            colors[i * 4 + 0] = Color.white;
            colors[i * 4 + 1] = Color.white;
            colors[i * 4 + 2] = Color.white;
            colors[i * 4 + 3] = Color.white;
        }

        colors[0 * 4 + 0] = Color.red;      //top
        colors[1 * 4 + 0] = Color.green;    //front
        colors[2 * 4 + 0] = Color.blue;     //bottom
        
        colors[3 * 4 + 0] = Color.yellow;   //left
        colors[4 * 4 + 0] = Color.gray;     //right
        colors[5 * 4 + 0] = Color.black;    //back

        mesh.colors = colors;
    }
    protected override void StepTriangle(Mesh mesh) 
    {
        int[] tris = new int[6 * 6];
        for(int i = 0;i < 6; i++)
        {
            tris[i * 6 + 0] = i*4 + 0;
            tris[i * 6 + 1] = i*4 + 1;
            tris[i * 6 + 2] = i*4 + 2;

            tris[i * 6 + 3] = i*4 + 1;
            tris[i * 6 + 4] = i*4 + 3;
            tris[i * 6 + 5] = i*4 + 2;
        }

        mesh.triangles = tris;
    }

    protected override void StepDrawSetting(Texture texture)
    {
        _draw = new DrawStandard(texture);
    }
    protected override void ChangeDraw(int n, Texture texture)
    {
        _draw = (n == 2) ? new DrawStandard(texture) : new DrawCustom(texture);
    }
}
