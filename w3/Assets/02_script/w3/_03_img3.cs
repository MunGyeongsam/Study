using System;
using System.Collections;
using System.Collections.Generic;
using _02_script.w3;
using UnityEngine;

public class _03_img3 : MonoBehaviour
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
        _mat = W3Util.CreateMaterial(_texture);
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

    Mesh CreateMesh(Texture2D texture)
    {
        Mesh mesh = new Mesh();
        
        Prop[,] map = GetPropMap(texture);
        
        int numOfRow = map.GetLength(0);
        int numOfCol = map.GetLength(1);
        
        int numOfVertices = numOfRow * numOfCol * 4;
        int numOfTriangles = numOfRow * numOfCol * 6;

        List<Vector3> vertices = new List<Vector3>(numOfVertices);
        List<Vector2> uvs = new List<Vector2>(numOfVertices);
        List<Color> colors = new List<Color>(numOfVertices);
        List<int> triangles = new List<int>(numOfTriangles);

        Span<Vector2> uvBuffer = stackalloc Vector2[4];
        Span<Vector3> vtxBuffer = stackalloc Vector3[4];

        Vector3 leftBottom = Vector3.zero;
        for (int r = 0; r < numOfRow; r++)
        {
            leftBottom.x = 0F;
            for (int c = 0; c < numOfCol; c++)
            {
                Prop prop = map[r, c];
                if (prop.IsAny)
                {
                    W3Util.GetVtx(vtxBuffer, leftBottom, _tileSize);
                    W3Util.GetedgeUvs(uvBuffer, prop);
                    
                    vertices.Add(vtxBuffer[0]);
                    vertices.Add(vtxBuffer[1]);
                    vertices.Add(vtxBuffer[2]);
                    vertices.Add(vtxBuffer[3]);
                    
                    uvs.Add(uvBuffer[0]);
                    uvs.Add(uvBuffer[1]);
                    uvs.Add(uvBuffer[2]);
                    uvs.Add(uvBuffer[3]);

                    colors.Add(Color.white);
                    colors.Add(Color.white);
                    colors.Add(Color.white);
                    colors.Add(Color.white);

                    int iv = vertices.Count - 4;
                    triangles.Add(iv + 0);
                    triangles.Add(iv + 1);
                    triangles.Add(iv + 2);
                    triangles.Add(iv + 1);
                    triangles.Add(iv + 3);
                    triangles.Add(iv + 2);
                }

                leftBottom.x += _tileSize;
            }
            
            leftBottom.z+= _tileSize;
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();
        
        W3Util.RecalcMesh(mesh);

        return mesh;
    }
    
    Prop[,] GetPropMap(Texture2D texture)
    {
        int numOfRow = texture.height;
        int numOfCol = texture.width;
        
        Prop[,] map = new Prop[numOfRow+1, numOfCol+1];

        for (int r = 0; r < numOfRow; r++)
        {
            for (int c = 0; c < numOfCol; c++)
            {
                Color color = texture.GetPixel(c, r);
                
                if (color == Color.white)
                    continue;

                map[r + 1, c + 0].RB = true;    map[r + 1, c + 1].LB = true;
                map[r + 0, c + 0].RT = true;    map[r + 0, c + 1].LT = true;
            }
        }

        return map;
    }
}
