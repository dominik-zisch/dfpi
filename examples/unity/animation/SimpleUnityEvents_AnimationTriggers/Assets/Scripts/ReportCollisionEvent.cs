using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReportCollisionEvent : MonoBehaviour
{
    [Serializable]
    public class Collision_Event:UnityEvent<bool>{}
    
    public Collision_Event OnCollisionStateChanged;
    
    private void OnTriggerEnter(Collider other)
    {
        OnCollisionStateChanged.Invoke(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        OnCollisionStateChanged.Invoke(false);
    }
}
