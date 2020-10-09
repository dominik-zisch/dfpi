//==============================================================
// Requires installation of the QuickGraph library
// https://www.nuget.org/packages/YC.QuickGraph
//==============================================================

using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms;

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

}
