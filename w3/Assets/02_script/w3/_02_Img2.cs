using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02_Img2 : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Texture2D _map;
    [SerializeField, Range(1F, 10F)] private float _tileSize = 1.0F;
    
    Mesh _mesh;
    Material _mat;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"map size : {_map.width} x {_map.height}");
        
        _mesh = CreateMesh(_map);
        _mat = W3Util.CreateCustomMaterial(_texture);
    }

    Mesh CreateMesh(Texture2D texture)
    {
        int numOfRow = texture.height;
        int numOfCol = texture.width;
        
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
                (int row, int col) = GetUvIndex(texture, r, c);
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
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            _mesh = CreateMesh(_map);
        
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _mat, 0);
    }


    (int row, int col) GetUvIndex(Texture2D texture, int r, int c)
    {
        int row = -1;
        int col = -1;

        int x = c;
        int y = r;
        
        if (IsFilled(texture, x, y))
        {
            return RandomUvIndex();
        }
        //*
        bool lb = IsFilled(texture, x-1, y) || IsFilled(texture, x, y-1) || IsFilled(texture, x-1, y-1); 
        bool rb = IsFilled(texture, x+1, y) || IsFilled(texture, x, y-1) || IsFilled(texture, x+1, y-1);
        bool lt = IsFilled(texture, x-1, y) || IsFilled(texture, x, y+1) || IsFilled(texture, x-1, y+1);
        bool rt = IsFilled(texture, x+1, y) || IsFilled(texture, x, y+1) || IsFilled(texture, x+1, y+1);
        
        if (lb && rb && lt && rt)
        {
            return RandomUvIndex();
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
        } else if (lb || rb) {
            
            row = 3;

            if (lb && rb)
                col = 3;
            else if (lb)
                col = 2;
            else
                col = 1;
        }
        //*/
        
        return (row, col);
    }
    
    (int row, int col) RandomUvIndex()
    {
        var n = UnityEngine.Random.Range(0, 18);

        return n switch
        {
            0 => (3, 0),
            1 => (0, 3),
                
            2 => (3, 4),
            3 => (3, 5),
            4 => (3, 6),
            5 => (3, 7),
                
            6 => (2, 4),
            7 => (2, 5),
            8 => (2, 6),
            9 => (2, 7),
                
            10=> (1, 4),
            11=> (1, 5),
            12=> (1, 6),
            13=> (1, 7),
                
            14=> (0, 4),
            15=> (0, 5),
            16=> (0, 6),
            17=> (0, 7),
        };
    }

    bool IsFilled(Texture2D texture, int x, int y)
    {
        if (y < 0 || y >= texture.height || x < 0 || x >= texture.width)
            return false;
        
        return texture.GetPixel(x, y) != Color.white;
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
}
