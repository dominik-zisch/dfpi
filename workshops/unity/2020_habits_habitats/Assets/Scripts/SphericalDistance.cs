using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphericalDistance : MonoBehaviour
{
    [Header("Object References")]
    public Transform A;
    public Transform B;

    [Header("Parameters")]
    [Range(0,10)]
    public float Radius;
    
    [Header("Read-Only")]
    public float GreatCircleDistance;
    
    private MeshRenderer mr;
    
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        transform.localScale = Vector3.one * Radius * 2;
                               
        var v1 = A.position - transform.position;
        var v2 = B.position - transform.position;

        var sv1 = v1.normalized ;
        var sv2 = v2.normalized ;
        
        GreatCircleDistance = Mathf.Acos(Vector3.Dot(sv1, sv2));
        
        // SETTING DATA TO THE SHADER
        mr.sharedMaterial.SetVector("_PosA", sv1);
        mr.sharedMaterial.SetVector("_PosB", sv2);
    }
}
