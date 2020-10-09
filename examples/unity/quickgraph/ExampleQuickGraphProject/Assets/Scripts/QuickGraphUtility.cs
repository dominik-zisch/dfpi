//==============================================================
// Requires installation of the QuickGraph library
// https://www.nuget.org/packages/YC.QuickGraph
//==============================================================

using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms;
using UnityEngine;
using Random = UnityEngine.Random;

public static class QuickGraphUtility
{
    public static AdjacencyGraph<int, SEdge<int>> BuildGraph()
    {
        var graph = new AdjacencyGraph<int, SEdge<int>>();
        return graph;
    }

    public static void AddVertices(AdjacencyGraph<int, SEdge<int>> graph, int vertexCount)
    {
        for (int i =0; i<vertexCount; i++)
        {
            graph.AddVertex(i);
        }
    }

    /// <summary>
    ///  Connect graph vertices from an incoming index array to an outgoing index array. The mapping is one-to-one,
    /// which means that the arrays MUST BE OF EQUAL LENGTH. If not, an IndexOutOfRange exception will be thrown.
    /// </summary>
    /// <param name="graph"> The graph to create edges for</param>
    /// <param name="from">Incoming indices</param>
    /// <param name="to">Outgoing indices</param>
    public static void AddEdgesOneToOne(AdjacencyGraph<int, SEdge<int>> graph, int[] from, int[] to)
    {
        for (int i =0; i<from.Length; i++)
        {
            graph.AddEdge(new SEdge<int>(from[i], to[i]));
        }
    }
    
    public static List<int> GetNeighbors(AdjacencyGraph<int, SEdge<int>> graph, int vertex)
    {
        List<int> neighbors = new List<int>();

        var edges = graph.OutEdges(vertex);

        foreach (var edge in edges)
        {
            neighbors.Add(edge.Target);
        }

        return neighbors;
    }

    public static List<int> ShortestPath_SingleSourceSingleTarget(AdjacencyGraph<int, SEdge<int>> graph, int source, int target)
    {
        List<int> vertexPath = new List<int>();
        
        vertexPath.Add(source);

        Func<SEdge<int>, double> edgeCost = e => 1;

        var tryGetPaths = graph.ShortestPathsDijkstra(edgeCost, source);

        if (tryGetPaths(target, out var edgePath))
        {
            foreach (SEdge<int> edge in edgePath)
            {
                vertexPath.Add(edge.Target);
            }
        }

        return vertexPath;
    }

    public static List<int> ShortestPath_SingleSourceSingleTarget(AdjacencyGraph<int, SEdge<int>> graph, int source, int target, Dictionary<string, double> costs)
    {
        List<int> vertexPath = new List<int>();
        vertexPath.Add(source);

        // Basic cost
        Func<SEdge<int>, double> edgeCost = e => 1;

        if (costs != null && costs.Count>0)
        {
            // COST FUNCTION
            edgeCost = e => 1 + costs[String.Format("{0}_{1}", e.Source, e.Target)]; // custom cost
        }

        var tryGetPaths = graph.ShortestPathsDijkstra(edgeCost, source);

        if (tryGetPaths(target, out var edgePath))
        {
            foreach (SEdge<int> edge in edgePath)
            {
                vertexPath.Add(edge.Target);
            }
        }

        return vertexPath;
    }

    public static string VerticesFriendlyOutput(this AdjacencyGraph<int, SEdge<int>> g)
    {
        var    verts = g.Vertices;
        
        var text  = "Graph Vertices\n";
        
        text += "----------------------\n";
      
        foreach (var v in verts)
        {
            text += $"Vertex: {v}\n";
        }

        return text;
    }
    
    public static string EdgesFriendlyOutput(this AdjacencyGraph<int, SEdge<int>> g)
    {
        var edges = g.Edges;
        
        var text = "Graph Edges\n";
        
        text += "----------------------\n";

        int i = 0;
      
        foreach (var e in edges)
        {
            text += $"Edge {i}: From [Vertex {e.Source}] --> To [Vertex {e.Target}]\n";

            i++;
        }

        return text;
    }
    
    public static void VisualizeEdgesInEditor(this AdjacencyGraph<int, SEdge<int>> g, Vector3[] positions, Color[] colors = null)
    {
        var edges = g.Edges;


        if (colors == null)
        {
            colors =  new Color[positions.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Random.ColorHSV();
            }
        }
        
        
        foreach (var e in edges)
        {
            var from = e.Source;
            var to   = e.Target;

            var posFrom = positions[from];
            var posTo   = positions[to];
            
            Debug.DrawLine(posFrom, posTo, colors[from]);
        }
    }
}
