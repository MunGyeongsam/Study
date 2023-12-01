using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class PathFinder
{
    static bool[,] _mapProps;
    static float _tileSize;

    static Vector2 _leftTop;

    static List<Node> _F = new List<Node>();
    static List<Node> _f = new List<Node>();
    static List<Node> _T = new List<Node>();
    static List<Node> _t = new List<Node>();

    public static void Init(char[,]map, bool[,] mapProps, float tileSize)
    {
        _mapProps = mapProps;
        _tileSize = tileSize;

        float hTileSize = tileSize * 0.5F;

        int row = mapProps.GetLength(0);
        int col = mapProps.GetLength(1);

        Vector2 max = new Vector2(col * hTileSize - hTileSize, row * hTileSize - hTileSize);
        Vector2 min = -max;

        _leftTop = new Vector2(min.x, max.y);

        InitFromAndTo(map);

        Random.InitState(System.DateTime.Now.Millisecond);
    }

    static void InitFromAndTo(char[,] map)
    {
        _F.Clear();
        _f.Clear();
        _T.Clear();
        _t.Clear();

        int ROW = map.GetLength(0);
        int COL = map.GetLength(1);

        for(int i=0;i<ROW; ++i)
        {
            for(int j=0; j<COL; ++j)
            {
                switch(map[i,j])
                {
                    case 'F': _F.Add(new Node(i, j)); break;
                    case 'f': _f.Add(new Node(i, j)); break;
                    case 'T': _T.Add(new Node(i, j)); break;
                    case 't': _t.Add(new Node(i, j)); break;
                }
            }
        }
    }

    public static (Node from, Node to) FromTo()
    {
        bool isOdd = Random.Range(0, 2) == 1;

        List<Node> F = isOdd ? _F : _f;
        List<Node> T = isOdd ? _T : _t;

        int nf = Random.Range(0, F.Count);
        int nt = Random.Range(0, T.Count);

        return (F[nf], T[nt]);
    }


    public static Vector2 ToPos(Node node)
    {
        return new Vector2 (
            _leftTop.x + node.Col * _tileSize,
            _leftTop.y - node.Row * _tileSize
        );
    }

    public static void Find(List<Node> path, Node from, Node to)
    {
        path.Clear();

        path.Add(from);
        if (from == to)
            return;

        int r0 = from.Row;
        int c0 = from.Col;

        int r1 = to.Row;
        int c1 = to.Col;

        int dx = c1 - c0;
        int dy = r1 - r0;

        int stepy;
        int stepx;

        while (dx != 0 || dy != 0)
        {
            stepy = Delta2Step(dy);
            stepx = Delta2Step(dx);

            from = new Node(r0 + stepy, c0 + stepx);
            path.Add(from);

            r0 = from.Row;
            c0 = from.Col;

            dx = c1 - c0;
            dy = r1 - r0;
        }
    }

    static Node NextWalkerbleNode(Node n, int stepX, int stepY)
    {
        if (_mapProps[n.Row + stepY, n.Col + stepX])
        {
            return n;
        }
        else
        {
            return new Node(n.Row + stepY, n.Col + stepX);
        }
    }

    static int Delta2Step(int delta)
    {
        return (delta == 0) ? 0 : (delta > 0 ? 1 : -1);
    }
}
