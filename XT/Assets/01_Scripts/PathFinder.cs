using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

namespace _01_Scripts
{
    ///*
    struct Node
    {
        public Node(int r, int c)
        {
            Row = r;
            Col = c;
        }
        public int Row { get; }
        public int Col { get; }
    }
    static class PathFinder
    {
        private static bool[,] _mapProps;
        private static Vector2 _leftTop;
        private static float _tileSize;
        
        public static void SetUp(bool[,] mapProps
            , float tileSize
            )
        {
            _mapProps = mapProps;
            _tileSize = tileSize;

            float halfTileSize = _tileSize * 0.5F;

            int row = _mapProps.GetLength(0);
            int col = _mapProps.GetLength(1);

            Vector2 max = new Vector2(col * tileSize * 0.5F - halfTileSize, row * tileSize * 0.5F - halfTileSize);
            Vector2 min = -max;

            _leftTop = new(min.x, max.y);
        }

        public static Vector2 ToVector2(Node node)
        {
            return new Vector2(_leftTop.x + node.Col*_tileSize, _leftTop.y - node.Row*_tileSize);
        }

        public static List<Node> Find(Node from, Node to)
        {
            List<Node> rt = new();

            int dy = to.Row - from.Row;
            int dx = to.Col - from.Col;

            int step = dy < 0 ? -1 : 1;
            for (int i = 0; i != dy; i += step)
            {
                rt.Add(new(from.Row + i, from.Col));
            }

            step = dx < 0 ? -1 : 1;
            for (int i = 0; i != dx; i += step)
            {
                rt.Add(new(to.Row, from.Col+i));   
            }

            return rt;
        }

        public static Mesh CreateMesh(float scale)
        {
            float s = scale * _tileSize * 0.5F;
            Mesh rt = new Mesh();

            float z = -1F;
            var vtx = new Vector3[4]
            {
                new Vector3(-s, +s, z),
                new Vector3(+s, +s, z),
                new Vector3(+s, -s, z),
                new Vector3(-s, -s, z)
            };
            var uvs = new Vector2[4]
            {
                new Vector2(0F, 0F),
                new Vector2(1F, 0F),
                new Vector2(1F, 1F),
                new Vector2(0F, 1F)
            };
            var tri = new int[3 * 2]
            {
                0,1,2,
                2,3,0
            };

            rt.vertices = vtx;
            rt.uv = uvs;
            rt.triangles = tri;
            
            rt.RecalculateNormals();
            rt.RecalculateTangents();
            rt.RecalculateBounds();

            return rt;
        }
    }
    
    //*/
}