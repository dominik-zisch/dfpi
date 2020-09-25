using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSelectionBehaviour : MonoBehaviour
{
    [Range(0,1)]
    public float buttonValue;
    public GeorgesRandomEvent buttonClicked;
    
    public void SendValueToListeners()
    {
        buttonClicked.Invoke(buttonValue);
    }

    [Serializable]
    public class GeorgesRandomEvent : UnityEvent<float>
    { }
}
