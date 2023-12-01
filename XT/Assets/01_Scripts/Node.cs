using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public Node(int row, int col)
    {
        Row = row;
        Col = col;
    }
    public int Row { get; }
    public int Col { get; }

    public static bool operator ==(Node lhs, Node rhs)
    {
        return lhs.Row == rhs.Row && lhs.Col == rhs.Col;
    }
    public static bool operator !=(Node lhs, Node rhs)
    {
        return !(lhs == rhs);
    }
    
    public override bool Equals(object obj)
    {
        Node? n = obj as Node?;
        return n != null && n.Value == this;
    }
    public override int GetHashCode()
    {
        return Row << 8 + Col;
    }
}