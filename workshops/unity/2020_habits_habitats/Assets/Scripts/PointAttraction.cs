using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAttraction : MonoBehaviour
{
    public Transform attractor;
    [Range(0,100)]
    public float attractionStrength;
    
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(( attractor.position-transform.position) *attractionStrength, ForceMode.Force);
    }
}
