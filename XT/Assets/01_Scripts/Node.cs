using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node(int row, int col)
    {
        Row = row;
        Col = col;
    }
    public int Row { get; }
    public int Col { get; }
    public float G { get; set; }
    public float H { get; set; }
    public float F
    {
        get { return G + H; }
    }
    public Node Parent { get; set; }
    public bool Walkable { get; private set; }
    public bool Closed { get; set; }
    public bool Opened { get; set; }

    public void Reset(bool walkable)
    {
        G = 0F;
        H = 0F;
        Parent = null;
        Walkable = false;
        Closed = false;
        Opened = false;
        Walkable = walkable;
    }

    public static bool IsEqual(Node lhs, Node rhs)
    {
        return lhs.Row == rhs.Row && lhs.Col == rhs.Col;
    }
}