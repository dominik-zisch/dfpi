using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestBehaviour : MonoBehaviour
{
    public Animator animator;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        //  animator.SetFloat("Fuzzy", transform.position.y);
        
       //  SendValueToAnimator(NormalizedXAxisAngle());
    }

    public float NormalizedXAxisAngle()
    {
        float angle = Vector3.Angle(transform.right, Vector3.right);
        angle /= 90;
        angle =  Mathf.Clamp01(angle);
        return angle;
    }
    
    public void SendValueToAnimator(float value)
    {
        animator.SetFloat("Fuzzy", value);
    }

}
