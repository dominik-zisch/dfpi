using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PolylineBuilder : MonoBehaviour
{
    public Transform observedObject;

    public List<Vector3> trackedPoints;

    [Range(0.0001f,0.1f)]
    public float minDistanceThreshold;

    [Range(2,64)]
    public int minAcceptablePointCount;
    
    
    [Range(2, 64)]
    public int ignoreFirstPoints;

    
    public string saveDirectory;
    
    
    private void OnEnable()
    {
        // Initialize the list when the game begins
        trackedPoints = new List<Vector3>();
      
        // Get the StreamingAssets directory and add the folder Recorded Polylines, if it doesn't already exist
        saveDirectory = Path.Combine(Application.streamingAssetsPath, "Recorded Polylines");
      
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    private void Update()
    {

        // Do nothing if we observe nothing
        if (observedObject == null)
            return;

        // Get current observed position
        var currentPoint = observedObject.transform.position;
        
        // Do nothing if points is too close to 0,0,0
        if (currentPoint.magnitude < 0.1f)
            return;
        
        // If it's the first point, add it immediately and do nothing else
        if (trackedPoints.Count == 0)
        {
            trackedPoints.Add(currentPoint);
            return;
        }
        
        // Get last saved point from the list
        var previousPoint = trackedPoints[trackedPoints.Count - 1];

        // Measure distance
        var distance = Vector3.Distance(previousPoint, currentPoint);
        
        // Do nothing if points are too close
        if (distance < minDistanceThreshold)
            return;
        
        // If everything is OK, add the point to the list
        trackedPoints.Add(currentPoint);
    }

    private void OnDestroy()
    {
        SavePolyline();
    }

    public void SavePolyline()
    {
        // Do not save small accidental polylines
        if (trackedPoints.Count - ignoreFirstPoints < minAcceptablePointCount)
            return;
        
        // Construct the name of the file
        var name  = BuildPolylineName();
        
        // Initialize a new StringBuilder
        var sb    = new StringBuilder();
        
        // Get the current point count
        var count = trackedPoints.Count;
        
        // Start writing each point as a new line
        for (int i = ignoreFirstPoints; i < count; i++)
        {
            // Get point coordinates
            var point     = trackedPoints[i];
            
            // Write line as text following the format x,y,z
            sb.AppendLine($"{point.x},{point.y},{point.z}");
        }
        
        // Write everything to a file
        File.WriteAllText(Path.Combine(saveDirectory,name), sb.ToString());
    }

    private string BuildPolylineName()
    {
        return $"Polyline_{trackedPoints.Count}-Vertices_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.csv";
    }
}
