using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class GraphExample : MonoBehaviour
{
    [Range(1,5)]
    public int              connectionsPerNode;
   
    public List<GameObject> nodes;
    public List<HarryPotterScene> hpNodes;

    public Dictionary<string, int> sceneNameLookup;
    
    public AdjacencyGraph<int, SEdge<int>> MyGraph;

    private Vector3[] positions;

    private Color[] colors;
    
    
    void OnEnable()
    {
         PopulateNodesFromChildren();

         MyGraph = QuickGraphUtility.BuildGraph();
         
         AddNodes();
         
         ConnectRandom(MyGraph, connectionsPerNode, 20);
         
        /* 
         List<Vector3> tempPositions = new List<Vector3>();
      
         foreach (var myNode in nodes)
         {
            tempPositions.Add(myNode.transform.position); 
         }

         positions = tempPositions.ToArray();
          */

        positions = nodes.Select(myNode => myNode.transform.position).ToArray();
        colors    = nodes.Select(myNode => Random.ColorHSV()).ToArray();
    }

    
    void Update()
    {
        positions = nodes.Select(myNode => myNode.transform.position).ToArray();
        MyGraph.VisualizeEdgesInEditor(positions, colors);
    }

    
    private void OnValidate()
    {
        ConnectRandom(MyGraph, connectionsPerNode, 20);
    }

    
    public void PopulateNodesFromChildren()
    {
        nodes = new List<GameObject>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            nodes.Add(transform.GetChild(i).gameObject);
            nodes[i].name = $"Node {i}";

            // Normalized coordinate [0...1]
            var t = i / (float) (transform.childCount);
           
            // Points that move around the unit circle
            var x = Mathf.Sin(t * Mathf.PI * 2);
            var y = 0;
            var z = Mathf.Cos(t * Mathf.PI * 2);
           
            // Set position
            nodes[i].transform.position = new Vector3(x,y,z);
        }
    }

    
    public void AddNodes()
    {
          QuickGraphUtility.AddVertices(MyGraph, nodes.Count);
       
          //  Debug.Log(PrintGraphVertices(MyGraph));
         
          Debug.Log(MyGraph.VerticesFriendlyOutput());
    }

    
    public void ConnectRandom(AdjacencyGraph<int, SEdge<int>> g, int numberOfConnectionsPerVertex, int maxTries = 100)
    {
       
        int cnt = g.VertexCount;
        
        int i               = 0;
       
        foreach (var v in g.Vertices)
        {
            g.ClearOutEdges(v);
            
            List<int> neighborsSoFar  = new List<int>();
            int       tries           = 0;
            int       connectionsMade = 0;
           
           
            while (connectionsMade < numberOfConnectionsPerVertex && tries < maxTries)
            {
                int connection = Random.Range(0, cnt);
                
                if (connection != i && !neighborsSoFar.Contains(connection))
                {
                    g.AddEdge(new SEdge<int>(i, connection));
                   
                    neighborsSoFar.Add(connection);
                    connectionsMade++;
                }

                tries++;
            }
             
            i++;
        }
        
     //   Debug.Log(MyGraph.EdgesFriendlyOutput());
    }


    public void ConnectHPScenes(AdjacencyGraph<int, SEdge<int>> g, List<HarryPotterScene> hpn)
    {
        foreach (var hp in hpn)
        {
            foreach (var neighbor in hp.connectedScenes)
            {
                
                g.AddEdge(new SEdge<int>(hp.id, sceneNameLookup[neighbor]));
            }
        }
    }
    
    
    public string PrintGraphVertices(AdjacencyGraph<int, SEdge<int>> g)
    {
        var    verts = g.Vertices;
        string text  = "Graph Vertices\n";
        text += "----------------------\n";
      
        foreach (var v in verts)
        {
            text += $"Vertex: {v}\n";
        }

        return text;
    }
    
}


[Serializable]
public class HarryPotterScene
{
    public string    name;
    public int       id;
    public List<string> connectedScenes;

}