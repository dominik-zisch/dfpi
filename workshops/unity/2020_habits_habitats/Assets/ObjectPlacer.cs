using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    /// <summary>
    /// Reference to the object that will be instantiated
    /// </summary>
    public GameObject objectToPlace;
    public GameObject objectToPreview;

    /// <summary>
    /// Reference to the object that will be the parent of all instantiated objects
    /// </summary>
    public Transform objectParent;

    /// <summary>
    /// Projection axis
    /// </summary>
    public Axis axis;

    
    public bool raycastWorld;
    public LayerMask raycastAgainst;
    
    [Range(0,100)]
    public float Distance;

    [Range(0,10)]
    public float Scale;
    
    public float timeUntilNextPlacement;
    public float distanceUntilNextPlacement;
   
    Vector3 placementPoint;
    Vector3 placementDirection;
    RaycastHit[] hits;
    GameObject previewObject;

    private Vector3 previousPoint;
    private Vector3 lastPlaced;
    private Vector3 velocity;
    private float prevTime;
    
    void Start()
    {
        hits = new RaycastHit[1];
        GeneratePreviewObject();
    }
    
    void Update()
    {
        var direction = Vector3.zero;
        
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
        
        var ray = new Ray(transform.position, direction);
        
        if (raycastWorld)
        {
          Raycast(ray);
        }
        else
        {
            placementPoint = ray.GetPoint(Distance);
            placementDirection = direction;
        }

        if (Time.time > prevTime + timeUntilNextPlacement && (placementPoint-lastPlaced).magnitude > distanceUntilNextPlacement)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                PlaceObject(placementPoint, placementDirection, velocity, Scale);
                lastPlaced = placementPoint;
            }

           
            prevTime = Time.time;
        }

        UpdatePreviewObject();
        Debug.DrawRay(transform.position, direction*Distance);
        
        velocity = placementPoint - previousPoint;
        previousPoint = placementPoint;
    }

    void GeneratePreviewObject()
    {
        previewObject = Instantiate(objectToPreview, transform);
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
        
        Distance = (hits[0].point - transform.position).magnitude;
        placementPoint = placementPoint *0.8f + 0.2f* hits[0].point;
        placementDirection = placementDirection*0.8f + 0.2f* hits[0].normal;
    }
    
    void PlaceObject(Vector3 position, Vector3 direction, Vector3 tangent = default, float scale = 1)
    {
        var o = Instantiate(objectToPlace, objectParent);
        o.transform.position = position;
        o.transform.forward = direction;
        o.transform.up = tangent;
        o.transform.localScale = Vector3.one* scale;
        
        o.GetComponent<Rigidbody>().AddForce(tangent *10, ForceMode.Impulse);
        o.GetComponent<PointAttraction>().attractor = transform;
    }


}

public enum Axis
{
    X,
    Y,
    Z
}
