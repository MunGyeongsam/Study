using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
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
    
    public static Mesh Cylinder(float radius, float height, int numOfAngle, int numOfSlice, [CanBeNull] Func<float, float, float> func = null)
    {
        var mesh = new Mesh();
        int numOfVtx = (numOfAngle * 2) * (numOfSlice + 1);
        
        //vertices
        var vtx = new Vector3[numOfVtx];
        float stepInRadians = Mathf.PI * 2F / numOfAngle;
        float stepInHeight = height / numOfSlice;
        
        Vector3 pt = Vector3.zero;
        int index = 0;
        
        for (int i = 0; i <= numOfSlice; i++)
        {
            pt.y = height * 0.5F - i * stepInHeight;
            float r = func?.Invoke(radius, pt.y) ?? radius;
            
            pt.x = r;
            pt.z = 0F;
            vtx[index++] = pt;
            
            for (int j = 1; j < numOfAngle; j++)
            {
                pt.x = r * Mathf.Cos(j * stepInRadians);
                pt.z = r * Mathf.Sin(j * stepInRadians);
                vtx[index++] = pt;
                vtx[index++] = pt;
            }
            
            pt.x = r;
            pt.z = 0F;
            vtx[index++] = pt;
        }
        Debug.Assert(index == numOfVtx);
        mesh.vertices = vtx;
        
        //triangles
        var tri = new int[numOfAngle * 6 * numOfSlice];
        int numOfVtxPerSlice = numOfAngle * 2;
        index = 0;
        for(int i=0; i<numOfSlice; i++)
        {
            int baseIndex = i * numOfVtxPerSlice;
            for (int j = 0; j < numOfAngle; j++)
            {
                tri[index++] = baseIndex + j * 2;
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
                
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + 1 + numOfVtxPerSlice;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
            }
        }
        Debug.Assert(index == numOfAngle * 6 * numOfSlice);
        mesh.triangles = tri;
        
        //uv
        var uvs = new Vector2[numOfVtx];
        Vector2 uv = Vector2.zero;
        index = 0;
        
        for(int i=0; i<=numOfSlice; i++)
        {
            uv.y = 1F - i / (float)numOfSlice;
            uv.x = 0F;
            uvs[index++] = uv;
            
            for (int j = 1; j < numOfAngle; j++)
            {
                uv.x = j / (float)numOfAngle;
                uvs[index++] = uv;
                uvs[index++] = uv;
            }
            
            uv.x = 1F;
            uvs[index++] = uv;
        }
        Debug.Assert(index == numOfVtx);
        mesh.uv = uvs;
        
        //colors
        var colors = new Color[numOfVtx];
        float colorStep = 1F / numOfSlice;
        index = 0;
        for (int i = 0; i <= numOfSlice; i++)
        {
            float v = i * colorStep;
            colors[index++] = new Color(v, v, v, 1);
            
            for (int j = 1; j < numOfAngle; j++)
            {
                colors[index++] = new Color(v, v, v, 1);
                colors[index++] = new Color(v, v, v, 1);
            }
            
            colors[index++] = new Color(v, v, v, 1);
        }
        Debug.Assert(index == numOfVtx);
        mesh.colors = colors;
        
        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Sphere1(float radius, int numOfAngle)
    {
        float CalcRadius(float raius, float y)
        {
            if (radius <= Mathf.Abs(y))
                return 0F;
            
            return Mathf.Sqrt(radius * radius - y * y);
        }
        var mesh = Cylinder(radius, radius*2F, numOfAngle, numOfAngle-1, CalcRadius);
        return mesh;
    }
    
    public static Mesh Sphere2(float radius, int numOfAngle)
    {
        var mesh = new Mesh();

        int numOfSlice = numOfAngle - 1;
        
        //vertices
        int numOfVtx = (numOfAngle * 2) * (numOfSlice + 1);
        var vtx = new Vector3[numOfVtx];
        float stepInRadians = Mathf.PI * 2F / numOfAngle;
        
        Vector3 pt = Vector3.zero;
        int index = 0;
        
        for (int i = 0; i <= numOfSlice; i++)
        {
            float y = radius * Mathf.Cos(i * Mathf.PI / numOfSlice);
            float r = radius * Mathf.Sin(i * Mathf.PI / numOfSlice);
            
            pt.y = y;
            pt.x = r;
            pt.z = 0F;
            vtx[index++] = pt;
            
            for (int j = 1; j < numOfAngle; j++)
            {
                pt.x = r * Mathf.Cos(j * stepInRadians);
                pt.y = y;
                pt.z = r * Mathf.Sin(j * stepInRadians);
                vtx[index++] = pt;
                vtx[index++] = pt;
            }
            
            pt.x = r;
            pt.z = 0F;
            vtx[index++] = pt;
        }
        Debug.Assert(index == numOfVtx);
        mesh.vertices = vtx;
        
        //triangles
        var tri = new int[numOfAngle * 6 * numOfSlice];
        int numOfVtxPerSlice = numOfAngle * 2;
        index = 0;
        for(int i=0; i<numOfSlice; i++)
        {
            int baseIndex = i * numOfVtxPerSlice;
            for (int j = 0; j < numOfAngle; j++)
            {
                tri[index++] = baseIndex + j * 2;
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
                
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + 1 + numOfVtxPerSlice;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
            }
        }
        Debug.Assert(index == numOfAngle * 6 * numOfSlice);
        mesh.triangles = tri;
        
        //uv
        var uvs = new Vector2[numOfVtx];
        Vector2 uv = Vector2.zero;
        index = 0;
        
        for(int i=0; i<=numOfSlice; i++)
        {
            uv.y = 1F - i / (float)numOfSlice;
            uv.x = 0F;
            uvs[index++] = uv;
            
            for (int j = 1; j < numOfAngle; j++)
            {
                uv.x = j / (float)numOfAngle;
                uvs[index++] = uv;
                uvs[index++] = uv;
            }
            
            uv.x = 1F;
            uvs[index++] = uv;
        }
        Debug.Assert(index == numOfVtx);
        mesh.uv = uvs;
        
        //colors
        var colors = new Color[numOfVtx];
        float colorStep = 1F / numOfSlice;
        index = 0;
        for (int i = 0; i <= numOfSlice; i++)
        {
            float v = i * colorStep;
            colors[index++] = new Color(v, v, v, 1);
            
            for (int j = 1; j < numOfAngle; j++)
            {
                colors[index++] = new Color(v, v, v, 1);
                colors[index++] = new Color(v, v, v, 1);
            }
            
            colors[index++] = new Color(v, v, v, 1);
        }
        Debug.Assert(index == numOfVtx);
        mesh.colors = colors;
        
        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Sphere3(float radius, int numOfAngle)
    {
        var mesh = new Mesh();

        //int numOfSlice = numOfAngle - 1;
        int numOfSlice = numOfAngle / 2;
        int numOfRect = numOfAngle * numOfSlice;
        int numOfVtx = numOfRect * 4;

        //vertices
        var vtx = new Vector3[numOfVtx];
        float stepInRadians = Mathf.PI * 2F / numOfAngle;
        
        Vector3 pt = Vector3.zero;
        int index = 0;
        
        for (int i = 0; i < numOfSlice; ++i)
        {
            for (int i2 = 0; i2 < 2; ++i2)
            {
                float y = radius * Mathf.Cos((i+i2) * Mathf.PI / numOfSlice);
                float r = radius * Mathf.Sin((i+i2) * Mathf.PI / numOfSlice);

                pt.y = y;
                pt.x = r;
                pt.z = 0F;
                vtx[index++] = pt;

                for (int j = 1; j < numOfAngle; j++)
                {
                    pt.x = r * Mathf.Cos(j * stepInRadians);
                    pt.y = y;
                    pt.z = r * Mathf.Sin(j * stepInRadians);
                    vtx[index++] = pt;
                    vtx[index++] = pt;
                }

                pt.x = r;
                pt.z = 0F;
                vtx[index++] = pt;
            }
        }
        Debug.Assert(index == numOfVtx);
        mesh.vertices = vtx;
        
        //triangles
        var tri = new int[numOfRect * 6];
        int numOfVtxPerSlice = numOfAngle * 2;
        index = 0;
        for(int i=0; i<numOfSlice; i++)
        {
            int baseIndex = i * numOfVtxPerSlice * 2;
            for (int j = 0; j < numOfAngle; j++)
            {
                tri[index++] = baseIndex + j * 2;
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
                
                tri[index++] = baseIndex + j * 2 + 1;
                tri[index++] = baseIndex + j * 2 + 1 + numOfVtxPerSlice;
                tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
            }
        }
        Debug.Assert(index == numOfAngle * 6 * numOfSlice);
        mesh.triangles = tri;
        
        //uv
        var uvs = new Vector2[numOfVtx];
        Vector2 uv = Vector2.zero;
        index = 0;
        
        for(int i=0; i<numOfSlice; i++)
        {
            for (int i2 = 0; i2 < 2; ++i2)
            {
                float y = 0.5F + 0.5F * Mathf.Cos((i+i2) * Mathf.PI / numOfSlice);
                
                
                //uv.y = 1F - (i+i2) / (float)numOfSlice;
                uv.y = y;
                uv.x = 0F;
                uvs[index++] = uv;

                for (int j = 1; j < numOfAngle; j++)
                {
                    uv.x = j / (float)numOfAngle;
                    uvs[index++] = uv;
                    uvs[index++] = uv;
                }

                uv.x = 1F;
                uvs[index++] = uv;
            }
        }
        Debug.Assert(index == numOfVtx);
        mesh.uv = uvs;
        
        //colors
        var colors = new Color[numOfVtx];
        float colorStep = 1F / numOfSlice;
        index = 0;
        for (int i = 0; i < numOfSlice; i++)
        {
            for (int i2 = 0; i2 < 2; ++i2)
            {
                float v = (i+i2) * colorStep;
                colors[index++] = new Color(v, v, v, 1);

                for (int j = 1; j < numOfAngle; j++)
                {
                    colors[index++] = new Color(v, v, v, 1);
                    colors[index++] = new Color(v, v, v, 1);
                }

                colors[index++] = new Color(v, v, v, 1);
            }
        }
        Debug.Assert(index == numOfVtx);
        mesh.colors = colors;
        
        RecalcMesh(mesh);
        return mesh;
    }

    public static Mesh Cone(float height, float radius, int numOfAngle)
    {
        var mesh = new Mesh();

        // Vertices
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var colors = new List<Color>();
        var normals = new List<Vector3>();

        // Bottom center vertex
        vertices.Add(Vector3.zero);
        uv.Add(new Vector2(0.5f, 0.5f));
        colors.Add(Color.white);
        normals.Add(Vector3.down);

        // Bottom circle vertices
        float step = 2 * Mathf.PI / numOfAngle;
        for (int i = 0; i < numOfAngle; i++)
        {
            float angle = i * step;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            vertices.Add(new Vector3(x, 0, z));
            uv.Add(new Vector2((x / radius + 1) * 0.5f, (z / radius + 1) * 0.5f));
            colors.Add(Color.white);
            normals.Add(Vector3.down);
        }

        // Top vertex
        vertices.Add(new Vector3(0, height, 0));
        uv.Add(new Vector2(0.5f, 1f));
        colors.Add(Color.white);
        normals.Add(Vector3.up);

        // Triangles
        var triangles = new List<int>();

        // Bottom circle triangles
        for (int i = 1; i <= numOfAngle; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i % numOfAngle + 1);
        }

        // Side triangles
        int topVertexIndex = vertices.Count - 1;
        for (int i = 1; i <= numOfAngle; i++)
        {
            int nextIndex = i % numOfAngle + 1;
            triangles.Add(i);
            triangles.Add(topVertexIndex);
            triangles.Add(nextIndex);

            // Calculate normals for side faces
            Vector3 normal = Vector3.Cross(vertices[nextIndex] - vertices[i], vertices[topVertexIndex] - vertices[i]).normalized;
            normals[i] = normal;
            normals[nextIndex] = normal;
            normals[topVertexIndex] = normal;

            // Assign different colors to each face
            colors[i] = new Color(i / (float)numOfAngle, 0, 1 - i / (float)numOfAngle, 1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();

        RecalcMesh(mesh);
        return mesh;
    }
    
    public static Mesh Torus(float radius, float tubeRadius, int numOfAngle, int numOfTube)
    {
        var mesh = new Mesh();

        // Vertices
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var colors = new List<Color>();
        var normals = new List<Vector3>();

        float step = 2 * Mathf.PI / numOfAngle;
        float tubeStep = 2 * Mathf.PI / numOfTube;

        for (int i = 0; i < numOfAngle; i++)
        {
            float angle = i * step;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            for (int j = 0; j < numOfTube; j++)
            {
                float tubeAngle = j * tubeStep;
                float tubeX = Mathf.Cos(tubeAngle);
                float tubeZ = Mathf.Sin(tubeAngle);

                Vector3 vertex = new Vector3(
                    (radius + tubeRadius * tubeX) * x,
                    tubeRadius * tubeZ,
                    (radius + tubeRadius * tubeX) * z
                );

                vertices.Add(vertex);
                uv.Add(new Vector2(i / (float)numOfAngle, j / (float)numOfTube));
                colors.Add(Color.white);
                normals.Add(vertex.normalized);
            }
        }

        // Triangles
        var triangles = new List<int>();

        for (int i = 0; i < numOfAngle; i++)
        {
            int nextI = (i + 1) % numOfAngle;
            for (int j = 0; j < numOfTube; j++)
            {
                int nextJ = (j + 1) % numOfTube;

                int index = i * numOfTube + j;
                int nextIndex = nextI * numOfTube + j;
                int nextJIndex = i * numOfTube + nextJ;
                int nextIJIndex = nextI * numOfTube + nextJ;

                triangles.Add(index);
                triangles.Add(nextJIndex);
                triangles.Add(nextIndex);

                triangles.Add(nextIndex);
                triangles.Add(nextJIndex);
                triangles.Add(nextIJIndex);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();

        RecalcMesh(mesh);
        return mesh;
    }
}
