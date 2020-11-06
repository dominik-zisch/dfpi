using System;
using System.Collections.Generic;

[Serializable]
public struct Node:IEquatable<Node>
{
    public readonly int Index;

    public Node(int index)
    {
        Index = index;
    }

    public List<Node> Neighbors(Graph g)
    {
        return g.Neighbors(this);
    }
    
    public bool IsValid => Index >= 0;

    public static explicit operator Node(int i)
    {
        return new Node(i);
    }

    public static explicit operator int(Node node)
    {
        return node.Index;
    }
    
    #region EQUALITY COMPARISONS

    public static bool operator ==(Node A, Node B)
    {
        return A.Index == B.Index;
    }

    public static bool operator !=(Node A, Node B)
    {
        return !(A == B);
    }

    public bool Equals(Node other)
    {
        return Index == other.Index;
    }

    public override bool Equals(object obj)
    {
        return obj is Node other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Index;
    }

    #endregion

}
