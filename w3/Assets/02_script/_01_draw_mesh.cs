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
public class _01_draw_mesh : MonoBehaviour
{
    private Mesh _mesh;
    private Material _mat;
    
    [SerializeField] private Texture texture;
    
    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0)
        };
        
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        
        int[] triangles = new int[6]
        {
            0, 1, 2,
            1, 3, 2
        };
        
        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        
        Material material = new Material(Shader.Find("Standard"));

        material.SetTexture("_MainTex", texture);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 0;

        _mat = material;
    }
    

    // Update is called once per frame
    void Update()
    {
    }
    
    
    void OnRenderObject()
    {
        
        // 메쉬 그리기
        Graphics.DrawMesh(_mesh, Vector3.zero, Quaternion.identity, _mat, 0);
    }
}
