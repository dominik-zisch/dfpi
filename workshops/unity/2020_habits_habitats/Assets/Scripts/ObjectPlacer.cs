using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [Header("Object references")]
    /// <summary>
    /// Reference to the object that will be instantiated
    /// </summary>
    public GameObject objectToPlace;
    public GameObject previewObject;
    
    /// <summary>
    /// Reference to the object that will be the parent of all instantiated objects
    /// </summary>
    public Transform objectParent;

    [Header("Raycasting options")]
    public bool performRaycast;
    public Axis axis;
    public LayerMask raycastAgainst;
    
    [Header("Placement Options")]
    [Range(0,100)] public float placeAtDistance = 3;
    [Range(0,10)] public float placeScaled = 1;
    [Range(0, 100)] public float applyTangentialForce;
    
    [Range(0,2)] public float timeUntilNextPlacement = 0;
    [Range(0,2)] public float distanceUntilNextPlacement = 1;
   
    
    Vector3 placementPoint;
    public Vector3 placementDirection;
    RaycastHit[] hits;
    private Vector3 previousPoint;
    private Vector3 lastPlaced;
    private Vector3 velocity;
    private float prevTime;
    
    void Start()
    {
        hits = new RaycastHit[1];

    }
    
    void Update()
    {
        var direction = Vector3.zero;
        
        // Select Axis
        switch (axis)
        {
            case Axis.X:
                direction = transform.right;
                break;
            case Axis.Y:
                direction = transform.up;
                break;
            case Axis.Z:
                direction = transform.forward;
                break;;
            default:
                direction = transform.forward;
                break;
        }
        
        // Generate Ray
        var ray = new Ray(transform.position, direction);
        
        // Perform Raycast or Get manual distance
        if (performRaycast)
        {
          Raycast(ray);
        }
        else
        {
            placementPoint = ray.GetPoint(placeAtDistance);
            placementDirection = direction;
        }

        // Try to place object
        if (Time.time > prevTime + timeUntilNextPlacement && (placementPoint-lastPlaced).magnitude > distanceUntilNextPlacement)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                PlaceObject(placementPoint, placementDirection, velocity * applyTangentialForce, placeScaled);
                lastPlaced = placementPoint;
            }
            
            prevTime = Time.time;
        }

        // Move child
        UpdatePreviewObject();
        
        velocity = placementPoint - previousPoint;
        previousPoint = placementPoint;
        
        Debug.DrawRay(transform.position, direction*placeAtDistance);
    }
    

    void UpdatePreviewObject()
    {
        previewObject.transform.position = placementPoint;
        previewObject.transform.forward = placementDirection;
    }
    
    void Raycast(Ray ray)
    {
        var i = Physics.RaycastNonAlloc(ray, hits, 1000, raycastAgainst.value);
        if (i < 1) return;
        
        placeAtDistance = (hits[0].point - transform.position).magnitude;
        placementPoint = hits[0].point;
        placementDirection =hits[0].normal;
    }
    
    void PlaceObject(Vector3 position, Vector3 direction, Vector3 tangent = default, float scale = 1)
    {
        var o = Instantiate(objectToPlace, objectParent);
        o.transform.position = position;
        
        o.transform.forward = direction;

        o.transform.localScale *= scale;
        
        o.GetComponent<Rigidbody>().AddForce(tangent, ForceMode.Impulse);
        o.GetComponent<PointAttraction>().attractor = transform;
    }


}

public enum Axis
{
    X,
    Y,
    Z
}
