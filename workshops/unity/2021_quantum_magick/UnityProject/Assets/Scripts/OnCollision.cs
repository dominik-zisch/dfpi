using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Time = UnityEngine.Time;

public class OnCollision : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public bool isColliding;
    
    private void Start()
    {
    }

    // WHEN A COLLIDER ENTERS OUR ZONE
    void OnTriggerEnter()
    {
        OnEnter.Invoke();
        isColliding = true;
    }

    // WHEN A COLLIDER EXITS OUR ZONE
    void OnTriggerExit()
    {
        OnExit.Invoke();
        isColliding = false;
    }
    
    
}


