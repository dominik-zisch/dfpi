using System.Collections.Generic;
using UnityEngine;


public class PolylineBuilder_Naive : MonoBehaviour
{
    [Tooltip("The object we observe.")]
    public Transform     observedObject;
    
    [Tooltip("The collection of all observed positions.")]
    public List<Vector3> trackedPoints;
    
    private void Start()
    {
        // Initialize an empty container
        trackedPoints = new List<Vector3>();
    }

    private void Update()
    {
        // Add a new point per frame
        trackedPoints.Add(observedObject.transform.position);
    }

    private void SavePolyline()
    {
        
        
    }
    
}
