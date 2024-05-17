   using System.Collections.Generic;
   public class Grid
   {
       private int _row;
       private int _col;
       private bool[,] _props;
       private Node[,] _nodes;
       private List<Node> _neighbors = new List<Node>(8);
       public void Init(bool[,] props)
       {
           int row = props.GetLength(0);
           int col = props.GetLength(1);
           _nodes = new Node[row, col];
           for (int r = 0; r < row; ++r)
           {
               for (int c = 0; c < col; ++c)
               {
                   _nodes[r, c] = new Node(r, c);
               }
           }
           _row = row;
           _col = col;
           _props = props;
       }
       public void Reset()
       {
           foreach (var node in _nodes)
           {
               node.Reset(!_props[node.Row, node.Col]);
           }
       }
       public Node GetNode(int r, int c)
       {
           return IsValid(r, c) ? _nodes[r, c] : null;
       }
       public Node[] Neighbors(Node node)
       {
           _neighbors.Clear();
           int R = node.Row;
           int C = node.Col;
           bool u = IsWalkable(R - 1, C + 0);      // ↑
           bool l = IsWalkable(R + 0, C + 1);      // →
           bool d = IsWalkable(R + 1, C + 0);      // ↓
           bool r = IsWalkable(R + 0, C - 1);      // ←
           
           if (u) _neighbors.Add(_nodes[R - 1, C + 0]);
           if (l) _neighbors.Add(_nodes[R + 0, C + 1]);
           if (d) _neighbors.Add(_nodes[R + 1, C + 0]);
           if (r) _neighbors.Add(_nodes[R + 0, C - 1]);
           
           if (u && r && IsWalkable(R - 1, C - 1)) _neighbors.Add(_nodes[R - 1, C - 1]);   // ↖
           if (u && l && IsWalkable(R - 1, C + 1)) _neighbors.Add(_nodes[R - 1, C + 1]);   // ↗
           if (d && l && IsWalkable(R + 1, C + 1)) _neighbors.Add(_nodes[R + 1, C + 1]);   // ↘
           if (d && r && IsWalkable(R + 1, C - 1)) _neighbors.Add(_nodes[R + 1, C - 1]);   // ↙
           return _neighbors.ToArray();
       }

       public void GetOpenAndClosedList(List<Node> open, List<Node> close)
       {
           open.Clear();
           close.Clear();
           foreach (var n in _nodes)
           {
               if (n.Closed)
                   close.Add(n);
               else if (n.Opened)
                   open.Add(n);
           }
       }
       bool IsValid(int r, int c)
       {
           return (r >= 0 && r < _row) && (c >= 0 && c < _col);
       }
       bool IsWalkable(int r, int c)
       {
           return IsValid(r,c) && _nodes[r, c].Walkable;
       }
   }