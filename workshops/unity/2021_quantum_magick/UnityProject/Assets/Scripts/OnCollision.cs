using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void Start()
    {
    }

    // WHEN A COLLIDER ENTERS OUR ZONE
    void OnTriggerEnter()
    {
        OnEnter.Invoke();
    }

    // WHEN A COLLIDER EXITS OUR ZONE
    void OnTriggerExit()
    {
        OnExit.Invoke();
    }
}
