using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class _01_draw_rect : MonoBehaviour
{
    private Mesh mesh;
    private Material material;

    [SerializeField] private Texture texture;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        float hw = 1F * 0.5F;

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

        material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        material.renderQueue = 0;
    }


    // Update is called once per frame
    void Update()
    {
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
    }


    void OnRenderObject()
    {
    }
}