using System;
using System.Collections.Generic;


[Serializable]
public class Graph 
{
    public List<Node> nodes
    {
        get;
        private set;
    }

    public List<Edge> edges
    {
        get;
        private set;
    }

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();
    }
   
    public Graph(int nodeCount)
    {
        nodes = new List<Node>();
     
        for (int i = 0; i < nodeCount; i++)
        {
            nodes.Add(new Node(i));
        }
        
        edges = new List<Edge>();
    }


    #region NECESSARY FUNCTIONS

    public void AddNode(Node node)
    {
        nodes.Add(node);
    }
    
    public void AddNodes(IEnumerable<Node> _nodes)
    {
        nodes.AddRange(_nodes);
    }
    
    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to));
    }
    
    public void AddEdges(IEnumerable<Edge> _edges)
    {
        edges.AddRange(_edges);
    }
    
    public List<Node> Neighbors(Node node)
    {
        var neighbors = new List<Node>();

        foreach (var edge in edges)
        {
            if (edge.TryGetNeighbor(node, out var neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    #endregion

    
    #region NICE TO HAVE FUNCTIONS
   
    public void AddEdges(Node from, Node[] to)
    {
        var _edges = new Edge[to.Length];
     
        for (var i = 0; i < _edges.Length; i++)
        {
            _edges[i] = new Edge(from, to[i]);
        }
        
        edges.AddRange(_edges);
    }
    
    
    public void AddEdges(Node[] from, Node to)
    {
        var _edges = new Edge[from.Length];
     
        for (var i = 0; i < _edges.Length; i++)
        {
            _edges[i] = new Edge(from[i], to);
        }
        
        edges.AddRange(_edges);
    }
 
      
    public void AddEdgesOneToOne(Node[] from, Node[] to)
    {
        if (from.Length != to.Length)
            throw new IndexOutOfRangeException("from.Length must be equal to to.Length");
        
        var _edges = new Edge[from.Length];
     
        for (var i = 0; i < _edges.Length; i++)
        {
            _edges[i] = new Edge(from[i], to[i]);
        }
        
        edges.AddRange(_edges);
    }
   
    
    public void AddEdgesOneToOne(int[] from, int[] to)
    {
        if (from.Length != to.Length)
            throw new IndexOutOfRangeException("from.Length must be equal to to.Length");
        
        var _edges = new Edge[from.Length];
     
        for (var i = 0; i < _edges.Length; i++)
        {
            _edges[i] = new Edge((Node)from[i], (Node)to[i]);
        }
        
        edges.AddRange(_edges);
    }


    public void AddEdgesAllToAll(Node[] from, Node[] to)
    {
        var _edges = new Edge[from.Length * to.Length];

        var cnt = 0;
      
        for (var i = 0; i < from.Length; i++)
        {
            for (var j = 0; j < to.Length; j++)
            {
                _edges[cnt] = new Edge(from[i], to[j]);
                cnt++;
            }

        }
        
        edges.AddRange(_edges);
    }
    
    #endregion


   
}
