using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _01_Img : MonoBehaviour
{
    //       +v
    //        |
    //        |  0     1     2     3     4     5     6     7                                 
    //   1.00 *-----*-----*-----*-----*-----*-----*-----*-----*(1.0, 1.0)
    //        |     |     |     |     |     |     |     |     |       
    //     3  |15   |0    |1    |2    |16   |17   |18   |19   |       
    //   0.75 *-----*-----*-----*-----*-----*-----*-----*-----*      0.75
    //        |     |     |     |     |     |     |     |     |          
    //     2  |3    |4    |5    |6    |20   |21   |22   |23   |          
    //   0.50 *-----*-----*-----*-----*-----*-----*-----*-----*      0.50
    //        |     |     |     |     |     |     |     |     |          
    //     1  |7    |8    |9    |10   |24   |25   |26   |27   |          
    //   0.25 *-----*-----*-----*-----*-----*-----*-----*-----*      0.25
    //        |     |     |     |     |     |     |     |     |          
    //     0  |11   |12   |13   |14   |28   |29   |30   |31   |          
    //  (0, 0)*-----*-----*-----*-----*-----*-----*-----*-----*      0.00 --- +u
    //       0.0   0.125 0.25  0.375 0.5   0.625 0.75  0.876 1.0
    
    
    //       +v
    //        |
    //        |  0     1     2     3     4     5     6     7                                 
    //   1.00 *-----*-----*-----*-----*-----*-----*-----*-----*(1.0, 1.0)
    //        | 1 1 | 0 0 | 0 0 | 0 0 | 1 1 | 1 1 | 1 1 | 1 1 |       
    //     3  | 1 1 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |       
    //   0.75 *-----*-----*-----*-----*-----*-----*-----*-----*      0.75
    //        | 0 1 | 0 1 | 0 1 | 0 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     2  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //   0.50 *-----*-----*-----*-----*-----*-----*-----*-----*      0.50
    //        | 1 0 | 1 0 | 1 0 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     1  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //   0.25 *-----*-----*-----*-----*-----*-----*-----*-----*      0.25
    //        | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     0  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //  (0, 0)*-----*-----*-----*-----*-----*-----*-----*-----*      0.00 --- +u
    //       0.0   0.125 0.25  0.375 0.5   0.625 0.75  0.876 1.0
    
    (int row, int col) GetUvIndex(Texture2D texture, int r, int c)
    {
        bool lb = texture.GetPixel(r+0, c+0)  != Color.white;
        bool rb = texture.GetPixel(r+0, c+1)  != Color.white;
        bool lt = texture.GetPixel(r+1, c+0)  != Color.white;
        bool rt = texture.GetPixel(r+1, c+1)  != Color.white;
        
        if (!lb && !rb && !lt && !rt)
            return (-1, -1);
        
        int row = -1;
        int col = -1;

        if (lb && rb && lt && rt)
        {
            row = UnityEngine.Random.Range(0, 4);
            col = UnityEngine.Random.Range(4, 8);
            return (row, col);
        }
        
        if (lt && rt) {
            
            row = 0;
            
            if (lb)
                col = 2;
            else if (rb)
                col = 1;
            else
                col = 0;
            
        } else if (lt) {
            
            row = 1;

            if (!lb && !rb) 
                col = 0;
            else if (lb && rb)
                col = 3;
            else if (rb) 
                col = 1;
            else
                col = 2;
            
        } else if (rt) {
            
            row = 2;

            if (!lb && !rb) 
                col = 0;
            else if (lb && rb)
                col = 3;
            else if (rb) 
                col = 1;
            else
                col = 2;
        } else {
            
            row = 3;

            if (lb)
                col = 2;
            else if (rb)
                col = 1;
            else
                col = 3;
        }
        
        return (row, col);
    }


    //  +v
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +u
    static void GetUv(Span<Vector2> buffer, int r, int c)
    {
        float vStep = 1.0F / 4.0F;
        float uStep = 1.0F / 8.0F;
        
        buffer[0] = new Vector2((c+0)*uStep, (r+1)*vStep);
        buffer[1] = new Vector2((c+1)*uStep, (r+1)*vStep);
        buffer[2] = new Vector2((c+0)*uStep, (r+0)*vStep);
        buffer[3] = new Vector2((c+1)*uStep, (r+0)*vStep);
    }
    

    //  +z
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +x
    static void GetVtx(Span<Vector3> buffer, int r, int c, float tileSize)
    {
        buffer[0] = new Vector3((c+0)*tileSize, 0, (r+1)*tileSize);
        buffer[1] = new Vector3((c+1)*tileSize, 0, (r+1)*tileSize);
        buffer[2] = new Vector3((c+0)*tileSize, 0, (r+0)*tileSize);
        buffer[3] = new Vector3((c+1)*tileSize, 0, (r+0)*tileSize);
    }
    
    static void GetTri(Span<int> buffer, int startIdx)
    {
        buffer[0] = startIdx + 0;
        buffer[1] = startIdx + 1;
        buffer[2] = startIdx + 2;
        
        buffer[3] = startIdx + 1;
        buffer[4] = startIdx + 3;
        buffer[5] = startIdx + 2;
    }
    
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Texture2D _map;
    [SerializeField, Range(1F, 10F)] private float _tileSize = 1.0F;
    
    Mesh _mesh;
    Material _mat;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"map size : {_map.width} x {_map.height}");
        Debug.Assert(_map.width % 4 == 0);
        Debug.Assert(_map.height % 4 == 0);
        
        _mesh = CreateMesh(_map);
        _mat = W3Util.CreateCustomMaterial(_texture);
    }

    Mesh CreateMesh(Texture2D texture)
    {
        int numOfRow = texture.height / 4;
        int numOfCol = texture.width / 4;
        
        int numOfVtx = numOfRow * numOfCol * 4;
        int numOfTri = numOfRow * numOfCol * 2;
        
        List<Vector3> vtxList = new List<Vector3>(numOfVtx);
        List<Vector2> uvList = new List<Vector2>(numOfVtx);
        List<Color> colorList = new List<Color>(numOfVtx);
        List<int> triList = new List<int>(numOfTri * 3);
        
        Span<Vector3> vtxBuffer = stackalloc Vector3[4];
        Span<Vector2> uvBuffer = stackalloc Vector2[4];
        Span<int> triBuffer = stackalloc int[6];
        
        for(int r=0; r<numOfRow; r++)
        {
            for(int c=0; c<numOfCol; c++)
            {
                (int row, int col) = GetUvIndex(texture, r*4, c*4);
                if (row == -1 || col == -1)
                    continue;
                
                GetVtx(vtxBuffer, r, c, _tileSize);
                GetUv(uvBuffer, row, col);
                GetTri(triBuffer, vtxList.Count);
                
                for(int i=0; i<4; i++)
                {
                    vtxList.Add(vtxBuffer[i]);
                    uvList.Add(uvBuffer[i]);
                }
                colorList.Add(Color.white);
                colorList.Add(Color.red);
                colorList.Add(Color.green);
                colorList.Add(Color.blue);
                
                for(int i=0; i<6; i++)
                {
                    triList.Add(triBuffer[i]);
                }
            }
        }

        if (vtxList.Count == 0)
            return null;
        
        Mesh mesh = new Mesh();
        
        mesh.vertices = vtxList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.colors = colorList.ToArray();
        mesh.triangles = triList.ToArray();
    
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if (_mesh == null)
            return;
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
            _mat = W3Util.CreateMaterial(_texture);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            _mat = W3Util.CreateCustomMaterial(_texture);
        
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _mat, 0);
    }
}
