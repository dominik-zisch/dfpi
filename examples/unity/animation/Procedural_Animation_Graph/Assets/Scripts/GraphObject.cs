using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GraphObject : MonoBehaviour
{
    [Serializable]
    public class GraphRebuildEvent:UnityEvent<Graph>{}
  
    [HideInInspector]
    public Graph graph;

    [Range(2,128)]
    public int nodeCount;
    
    [Range(2, 128)]
    public int randomEdgeCount = 2;

    public GraphRebuildEvent OnGraphRebuild;
    
    private void OnEnable()
    {
        if (Application.isPlaying)
            return;
       
        GenerateRandomGraph(nodeCount, randomEdgeCount);
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;
        
        if (!enabled)
            return;
        
        GenerateRandomGraph(nodeCount, randomEdgeCount);
    }
    
    public void GenerateRandomGraph(int nodeCount, int edgeCount)
    {
        graph = new Graph(nodeCount);

        if (edgeCount < 1)
            return;
        
        var temp = new List<int >();
      
        while (temp.Count < edgeCount)
        {
            temp.Add(Mathf.FloorToInt(Random.Range(0f, 1f) * nodeCount));
        }

        var   From = temp.ToArray();
        var To   = new int[From.Length];
        

        for (var i = 0; i < From.Length; i++)
        {
            var f = From[i];
          
            var t = Mathf.FloorToInt(Random.Range(0f, 1f) * nodeCount);
           
            while (t == f)
            {
                t = Mathf.FloorToInt(Random.Range(0f, 1f) * nodeCount);
            }

            To[i] = t;
        }
        
        graph.AddEdgesOneToOne(From, To);
        
        OnGraphRebuild.Invoke(graph);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
