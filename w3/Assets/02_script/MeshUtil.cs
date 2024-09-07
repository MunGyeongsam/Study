using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtil
{
    //       +v
    //        |
    //        |
    //   1.00 *-----*-----*-----*-----*-----*-----*-----*-----*(1.0, 1.0)
    //        |     |     |     |     |     |     |     |     |       
    //        |15   |0    |1    |2    |16   |17   |18   |19   |       
    //   0.75 *-----*-----*-----*-----*-----*-----*-----*-----*      0.75
    //        |     |     |     |     |     |     |     |     |          
    //        |3    |4    |5    |6    |20   |21   |22   |23   |          
    //   0.50 *-----*-----*-----*-----*-----*-----*-----*-----*      0.50
    //        |     |     |     |     |     |     |     |     |          
    //        |7    |8    |9    |10   |24   |25   |26   |27   |          
    //   0.25 *-----*-----*-----*-----*-----*-----*-----*-----*      0.25
    //        |     |     |     |     |     |     |     |     |          
    //        |11   |12   |13   |14   |28   |29   |30   |31   |          
    //  (0, 0)*-----*-----*-----*-----*-----*-----*-----*-----*      0.00 --- +u
    //       0.0   0.125 0.25  0.375 0.5   0.625 0.75  0.876 1.0


    //  +v
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +u


    //  +z
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +x
    public static Material CreateMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Standard"));

        material.mainTexture = texture;
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.EnableKeyword("_ALPHATEST_ON");
        material.SetPass(0);

        return material;
    }

    public static Material CreateCustomMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        return material;
    }

    public static Mesh Rect(float width, float height)
    {
        Mesh mesh = new Mesh();

        float px = width * 0.5F;    //positive x
        float nx = -px;             //negative x

        float pz = height * 0.5F;   //positive z
        float nz = -pz;


        //  +z
        //   |
        // 0 *---* 1
        //   | / |
        // 2 *---* 3 -- +x
        mesh.vertices = new Vector3[4]
        {
            new Vector3 (nx, 0F, pz),
            new Vector3 (px, 0F, pz),
            new Vector3 (nx, 0F, nz),
            new Vector3 (px, 0F, nz),
        };

        mesh.triangles = new int[2*3]
        {
            0, 1, 2,
            2, 1, 3
        };

        mesh.uv = new Vector2[4]
        {
            new Vector2 (0F, 1F),
            new Vector2 (1F, 1F),
            new Vector2 (0F, 0F),
            new Vector2 (1F, 0F),
        };

        mesh.colors = new Color[4]
        {
            Color.red,
            Color.white,
            Color.white,
            Color.white,
        };

        RecalcMesh(mesh);
        return mesh;
    }

    private static void RecalcMesh(Mesh mesh)
    {
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public static Mesh Cube(float size)
    {
        var mesh = new Mesh();

        //to be implemented

        float hs = size * 0.5F;
        mesh.vertices = new Vector3[]
        {
            //top
            new Vector3(-hs, hs, hs),
            new Vector3( hs, hs, hs),
            new Vector3(-hs, hs,-hs),
            new Vector3( hs, hs,-hs),

            //front
            new Vector3(-hs, hs,-hs),
            new Vector3( hs, hs,-hs),
            new Vector3(-hs,-hs,-hs),
            new Vector3( hs,-hs,-hs),

            //right
            new Vector3( hs, hs,-hs),
            new Vector3( hs, hs, hs),
            new Vector3( hs,-hs,-hs),
            new Vector3( hs,-hs, hs),

            //back
            new Vector3( hs, hs, hs),
            new Vector3(-hs, hs, hs),
            new Vector3( hs,-hs, hs),
            new Vector3(-hs,-hs, hs),

            //left
            new Vector3(-hs, hs, hs),
            new Vector3(-hs, hs,-hs),
            new Vector3(-hs,-hs, hs),
            new Vector3(-hs,-hs,-hs),

            //bottom
            new Vector3(-hs,-hs,-hs),
            new Vector3( hs,-hs,-hs),
            new Vector3(-hs,-hs, hs),
            new Vector3( hs,-hs, hs),
        };

        var tri = new int[6 * 6];
        for(int i=0; i<6; i++)
        {
            tri[i * 6 + 0] = i * 4 + 0;
            tri[i * 6 + 1] = i * 4 + 1;
            tri[i * 6 + 2] = i * 4 + 2;

            tri[i * 6 + 3] = i * 4 + 1;
            tri[i * 6 + 4] = i * 4 + 3;
            tri[i * 6 + 5] = i * 4 + 2;
        }
        mesh.triangles = tri;

        var uv = new Vector2[6 * 4];
        for (int i = 0; i < 6; i++)
        {
            uv[i * 4 + 0] = new Vector2(0, 1);
            uv[i * 4 + 1] = new Vector2(1, 1);
            uv[i * 4 + 2] = new Vector2(0, 0);
            uv[i * 4 + 3] = new Vector2(1, 0);
        }
        mesh.uv = uv;

        var colors = new Color[6 * 4];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.white;

        colors[0 * 4] = Color.red;
        colors[1 * 4] = Color.green;
        colors[2 * 4] = Color.blue;
        colors[3 * 4] = Color.yellow;
        colors[4 * 4] = Color.magenta;
        colors[5 * 4] = Color.cyan;

        mesh.colors = colors;

        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Pyramid(float size)
    {
        var mesh = new Mesh();

        //to be implemented
        float hs = size * 0.5F;

        mesh.vertices = new Vector3[]
        {
            new Vector3( 0F, size, 0F),
            new Vector3( hs,   0F,-hs),
            new Vector3(-hs,   0F,-hs),

            new Vector3( 0F, size, 0F),
            new Vector3( hs,   0F, hs),
            new Vector3( hs,   0F,-hs),

            new Vector3( 0F, size, 0F),
            new Vector3(-hs,   0F, hs),
            new Vector3( hs,   0F, hs),

            new Vector3( 0F, size, 0F),
            new Vector3(-hs,   0F,-hs),
            new Vector3(-hs,   0F, hs),

            new Vector3(-hs, 0F,-hs),
            new Vector3( hs, 0F,-hs),
            new Vector3(-hs, 0F, hs),
            new Vector3( hs, 0F, hs),
        };

        var tri = new int[3 * 4 + 6];
        int i = 0;
        for (; i < 12; i++)
        {
            tri[i] = i;
        }
        int btm0 = i;
        tri[i++] = btm0 + 0;
        tri[i++] = btm0 + 1;
        tri[i++] = btm0 + 2;

        tri[i++] = btm0 + 1;
        tri[i++] = btm0 + 3;
        tri[i++] = btm0 + 2;
        mesh.triangles = tri;

        var uv = new Vector2[3 * 4 + 4];
        for(i = 0; i<12; i+=3)
        {
            uv[i + 0] = new Vector2(0.5F, 1F);
            uv[i + 1] = new Vector2(0F, 0F);
            uv[i + 2] = new Vector2(1F, 0F);
        }
        uv[i++] = new Vector2(0F, 1F);
        uv[i++] = new Vector2(1F, 1F);
        uv[i++] = new Vector2(0F, 0F);
        uv[i++] = new Vector2(1F, 0F);
        mesh.uv = uv;

        var colors = new Color[3*4 + 4];
        for (i = 0; i < 3 * 4 + 4; i++)
            colors[i] = Color.white;
        colors[0 * 3] = Color.red;
        colors[1 * 3] = Color.green;
        colors[2 * 3] = Color.blue;
        colors[3 * 3] = Color.yellow;
        mesh.colors = colors;

        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Sphere(float radius)
    {
        var mesh = new Mesh();

        //to be implemented

        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Circle(float radius, int numOfAngle)
    {
        var mesh = new Mesh();

        //to be implemented

        float stepInRadians = Mathf.PI * 2F / numOfAngle;
        
        int numOfVtx = 1 + numOfAngle;

        
        var uv = new Vector2[numOfVtx];
        var vtx = new Vector3[numOfVtx];
        for (int i = 0; i < vtx.Length; i++)
        {
            vtx[i] = Vector3.zero;
            uv[i] = Vector2.zero;
        }


        uv[0].x = 0.5F;
        uv[0].y = 0.5F;
        
        for (int i = 0; i<numOfAngle; i++)
        {
            float c = Mathf.Cos((i + 1) * stepInRadians);
            float s = Mathf.Sin((i+1) * stepInRadians);
            vtx[1 + i].x = radius * c;
            vtx[1 + i].z = radius * s;

            uv[1 + i].x = c * 0.5F + 0.5F;
            uv[1 + i].y = s * 0.5F + 0.5F;
        }
        mesh.vertices = vtx;
        mesh.uv = uv;

        var tri = new int[numOfAngle * 3];
        for (int i = 0, j=2; i < numOfAngle*3-1; i +=3, ++j)
        {
            tri[i + 0] = 0;
            tri[i + 1] = j;
            tri[i + 2] = j - 1;
        }

        tri[^3] = 0;
        tri[^2] = 1;
        tri[^1] = numOfAngle;
        
        mesh.triangles = tri;

        var colors = new Color[numOfVtx];
        float colorStep = 1F / numOfAngle;
        colors[0] = Color.white;

        for (int i = 1; i < colors.Length; i++)
        {
            float v = i * colorStep;
            colors[i] = new Color(v,v,v,1);
        }

        mesh.colors = colors;

        RecalcMesh(mesh);
        return mesh;
    }
}
