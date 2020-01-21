using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphericalDistance : MonoBehaviour
{
    public Transform A;

    public Transform B;

    public float Radius;

    public float GreatCircleDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Radius * 2;
                               
        var v1 = A.position - transform.position;
        var v2 = B.position - transform.position;

        var sv1 = v1.normalized * Radius;
        var sv2 = v2.normalized * Radius;

        
        
        GreatCircleDistance = Mathf.Acos(Vector3.Dot(sv1, sv2));
        
        Debug.DrawLine(transform.position, v1);
        Debug.DrawLine(transform.position, v2);
        
        Debug.DrawLine(transform.position, sv1, Color.red);
        Debug.DrawLine(transform.position, sv2, Color.red);
        
        
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_PosA", sv1);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_PosB", sv2);
    }
}
