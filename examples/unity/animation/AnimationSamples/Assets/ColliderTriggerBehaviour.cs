using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerBehaviour : MonoBehaviour
{
    public Collider colliderA;
    public Collider colliderB;
    
    public Color        defaultColor;
    private MeshRenderer mr;

    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        mr                      = GetComponent<MeshRenderer>();
        mr.sharedMaterial.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /// <summary>
    /// This function executes only when 2 rigid bodies bounce against each other.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision happened");
      
        /*
        if (other.collider == colliderA)
        {
            mr.sharedMaterial.color = Color.blue;
        }
        else if (other.collider == colliderB)
        {
            mr.sharedMaterial.color = Color.red;
        }
        */
    }
    
     /// <summary>
     /// This function executes only when 2 rigid bodies, that are set as Triggers, enter each other
     /// </summary>
     /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision happened");
        
        if (other == colliderA)
        {
            mr.sharedMaterial.color = Color.blue;

            // If an animator is not attached, this part will do nothing
            if (animator != null)
            {
                animator.SetTrigger("SphereACollided");
            }
        }
        else if (other == colliderB)
        {
            mr.sharedMaterial.color = Color.red;
            
            // If an animator is not attached, this part will do nothing
            if (animator != null)
            {
                animator.SetTrigger("SphereBCollided");
            }
        }
    }
     
     /// <summary>
     /// This function executes only when 2 rigid bodies, that are set as Triggers, exit each other
     /// </summary>
     private void OnTriggerExit(Collider other)
     {
         mr.sharedMaterial.color = defaultColor;
         
         // If an animator is not attached, this part will do nothing
         if (animator != null)
         {
             animator.SetTrigger("Nothing Collided");
         }
         
     }
}
