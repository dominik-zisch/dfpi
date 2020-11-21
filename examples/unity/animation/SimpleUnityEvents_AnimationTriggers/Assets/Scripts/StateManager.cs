using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    [Header("Sphere Events Setup")]
    public KeyCode    sphereAppearKey;
    public UnityEvent OnSphereShouldAppear;
  
    public KeyCode    sphereDisappearKey;
    public UnityEvent OnSphereShouldDisappear;
 
    [Header("VFX Events Setup")]
    public KeyCode    vfxAppearKey;
    public UnityEvent OnVFXShouldAppear;
  
    public KeyCode    vfxDisappearKey;
    public UnityEvent OnVFXShouldDisappear;

    [Header("Sphere State")]
    public float sphereTemporaryTimer;
    public float sphereTotalTimer;
    public bool  sphereTriggered;


    private void Start()
    {
        sphereTemporaryTimer = 0;
        sphereTotalTimer     = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(sphereAppearKey))
            OnSphereShouldAppear.Invoke();
        
        if (Input.GetKeyDown(sphereDisappearKey))
            OnSphereShouldDisappear.Invoke();
        
        
        if (Input.GetKeyDown(vfxAppearKey))
            OnVFXShouldAppear.Invoke();
        
        if (Input.GetKeyDown(vfxDisappearKey))
            OnVFXShouldDisappear.Invoke();
  

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
