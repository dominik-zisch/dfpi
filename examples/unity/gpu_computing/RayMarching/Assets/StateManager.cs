using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    public UnityEvent OnSphereShouldAppear;
    public UnityEvent OnSphereShouldDisappear;
 
    public UnityEvent OnVFXShouldAppear;
    public UnityEvent OnVFXShouldDisappear;

    public float sphereTemporaryTimer;
    public float sphereTotalTimer;

    public bool  sphereTriggered;
    
    void Start()
    {
        sphereTemporaryTimer = 0;
        sphereTotalTimer     = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
         OnSphereShouldAppear.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
          OnSphereShouldDisappear.Invoke();
        }
       
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnVFXShouldAppear.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnVFXShouldDisappear.Invoke();
        }

        if (sphereTriggered)
        {
            sphereTemporaryTimer += Time.deltaTime;
            sphereTotalTimer     += Time.deltaTime;
        }
    }


    public void SphereGotTinkered(bool state)
    {
        sphereTriggered = state;
     
        if (state)
            sphereTemporaryTimer = 0;
    }
    
    public void VFXGotTinkered(bool state)
    {
      //  vfxTriggered = state;
     
     //   if (state)
     //       vfxTemporaryTimer = 0;
     
    }
}
