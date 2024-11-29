using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeoUtil
{
    public static Material GetMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        material.renderQueue = 0;

        return material;
    }

    public static Mesh RextXZ(float width = 1F)
    {
        Mesh mesh = new Mesh();

        float hw = width * 0.5F;

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-hw, 0,  hw),
            new Vector3( hw, 0,  hw),
            new Vector3(-hw, 0, -hw),
            new Vector3( hw, 0, -hw)
        };

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0)
        };

        int[] triangles = new int[6]
        {
            0, 1, 2,
            1, 3, 2
        };

        Color[] colors = new Color[4]
        {
            Color.white,
            Color.yellow,
            Color.red,
            Color.green,
        };
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.colors = colors;

        RecalcMesh(mesh);
        return mesh;
    }

    static void RecalcMesh(Mesh mesh)
    {
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
    }
}
