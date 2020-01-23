using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OffsetFollow : MonoBehaviour
{
    public Transform target;
#if OCULUS
[FormerlySerializedAs("controller")] public OVRInput.Controller offsetAdjustableByController;
#endif
    [Header("Uniform movement")]
    public Camera cam;
    public Vector2 offset;
    
    private bool self;
    
    [Range(-3,10)]
    public float forwardOffset;
    
    [Range(-3,10)]
    public float rightOffset;
    
    [Range(-3,10)]
    public float upOffset;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        #if OCULUS
        if (offsetAdjustableByController == OVRInput.Controller.LTouch || offsetAdjustableByController == OVRInput.Controller.RTouch)
            forwardOffset += OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, offsetAdjustableByController).y * Time.deltaTime;
        #endif
        
        self = target == null;
        if (self) target = transform;
        
        transform.position = target.position +
            transform.forward * forwardOffset + transform.right * rightOffset + transform.up * upOffset;
        transform.rotation = target.rotation;
        
        
        
        var frwrd = cam.transform.forward;
        frwrd.y = 0;

        var right = cam.transform.right;
        right.y = 0;

        #if OCULUS
        offset = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller) * Time.deltaTime;
        
        #endif
        
#if UNITY_EDITOR
        offset = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
#endif
        transform.position += frwrd * offset.y + right * offset.x;
    }
}
