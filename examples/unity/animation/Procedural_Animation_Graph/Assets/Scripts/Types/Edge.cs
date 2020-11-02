using System;

[Serializable]
public readonly struct Edge:IEquatable<Edge>
{
    public readonly Node From;
    public readonly Node To;

    public Edge(Node from, Node to)
    {
        From = from;
        To   = to;
    }

    public bool TryGetNeighbor(Node node, out Node neighbor)
    {
        
        if (node == From)
        {
            neighbor = To;
            return true;
        }

        if (node == To)
        {
            neighbor = From;
            return true;
        }

        neighbor = new Node(int.MinValue);
        return false;
    }

    #region EQUALITY COMPARISONS

    public static bool operator ==(Edge A, Edge B)
    {
        return A.From == B.From && A.To == B.To;
    }

    public static bool operator !=(Edge A, Edge B)
    {
        return !(A == B);
    }
    
    public bool Equals(Edge other)
    {
        return From.Equals(other.From) && To.Equals(other.To);
    }

    public override bool Equals(object obj)
    {
        return obj is Edge other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (From.GetHashCode() * 397) ^ To.GetHashCode();
        }
    }

    #endregion

    public override string ToString()
    {
        return $"{From.Index}->{To.Index}";
    }
}
