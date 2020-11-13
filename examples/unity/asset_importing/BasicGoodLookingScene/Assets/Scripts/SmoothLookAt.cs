using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnchartedLimbo.Core.Runtime.CameraControls
{
    public class SmoothLookAt : MonoBehaviour
    {
        [Header("References")]
        public CameraControlSettings settings;
        public BoxCollider      targetObject;
       
        [HideInInspector]
        public GraphicRaycaster raycaster;
        [HideInInspector]
        public EventSystem      eventSystem;

        
        [Header("Runtime Info")]
        public float            currentDistance;
        public Vector3 fixedTarget;
        public Vector3 looseTarget;
        public Vector3 tempTarget;

        
        public Vector3          newPosition;
        private Camera           cam;
        private Vector3          velocity       = new float3(0, 0, 0);
        private Vector3          targetVelocity = new float3(0, 0, 0);
        private bool             shouldZoomExtents;
        private PointerEventData pointerEventData;
       
        // WORKAROUND FOR REMOTE DESKTOP CONNECTION (GoToMyPC etc)
        private Vector2 lastAxis;
        
        
        private void Start()
        {
            cam               = GetComponent<Camera>();
            fixedTarget       = targetObject.center;
            shouldZoomExtents = true;
        }

        
        private void Update()
        {
            //----------------------------------------------------------------------------------------------------------
            // FIXED TARGET FINDING
            //----------------------------------------------------------------------------------------------------------
            fixedTarget = targetObject.transform.TransformPoint(targetObject.center);
         
            var currentTarget = settings.fixedTarget ? fixedTarget : looseTarget;
            
            if (Input.GetKey(KeyCode.Space))
            {
                shouldZoomExtents = true;
            }
            
            if (shouldZoomExtents)
            {
                ZoomExtents(ref newPosition);
                shouldZoomExtents = false;
            }
            else
            {
                //------------------------------------------------------------------------------------------------------
                // ORBIT AROUND TARGET
                //------------------------------------------------------------------------------------------------------
                if (Input.GetMouseButton(1))
                {
                    OrbitAroundTarget(in currentTarget, ref newPosition);
                }

                
                //------------------------------------------------------------------------------------------------------
                // DISTANCE ADAPTIVE ZOOMING
                //------------------------------------------------------------------------------------------------------
                TryScrollZoom(in currentTarget, ref newPosition);

                
                //------------------------------------------------------------------------------------------------------
                // DRAG
                //------------------------------------------------------------------------------------------------------
                if (Input.GetMouseButton(2))
                {
                    Drag(ref looseTarget, ref newPosition);
                }
            }

            ApplyMotion();

            // WORKAROUND FOR REMOTE DESKTOP CONNECTION (GoToMyPC etc)
            lastAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        
        
        private void OrbitAroundTarget(in Vector3 target, ref Vector3 newPosition)
        {
            // ORIGINAL BEHAVIOUR
            //float axisX = Input.GetAxis("Mouse X");
            //float axisY = Input.GetAxis("Mouse Y");
            
            // WORKAROUND FOR REMOTE DESKTOP CONNECTION (GoToMyPC etc)
            Vector2 axis = new Vector3(-(lastAxis.x - Input.mousePosition.x)*0.01F, -(lastAxis.y - Input.mousePosition.y)*0.01F);
            var axisX = axis.x;
            var axisY = axis.y;
            
            
            var upwards    = cam.transform.up;
            var right      = cam.transform.right;
            var sideways   = new Vector3(right.x, 0, right.z);

            // Adjust new position by moving sideways or upwards/downwards 
             newPosition = transform.position
                          + -axisX * settings.orbitSpeed * 4*(currentDistance / settings.maxDistance) * sideways
                          + -axisY * settings.orbitSpeed * 4*(currentDistance / settings.maxDistance) * upwards;

            // Move towards our target to maintain the desired distance
            var newDir = target - newPosition;
            var newDist = newDir.magnitude;
            var error = newDist - settings.desiredDistance;
            var zForce = newDir.normalized * error;

            newPosition += zForce;
        }

        
        private void Drag(ref Vector3 newTarget, ref Vector3 newPosition )
        {
            if (settings.fixedTarget)
                return;
            
            var forward  =Vector3.up;
            var sideways = cam.transform.right;
            sideways.y = 0;
                
            newTarget +=
                    sideways * (+ -Input.GetAxis("Mouse X") * settings.orbitSpeed * (currentDistance /settings.maxDistance) )
                    + forward * (-Input.GetAxis("Mouse Y") * settings.orbitSpeed * (currentDistance /settings.maxDistance) );
                
            newPosition +=
                    sideways * (+ -Input.GetAxis("Mouse X") * settings.orbitSpeed * (currentDistance /settings.maxDistance) )
                    + forward * (-Input.GetAxis("Mouse Y") * settings.orbitSpeed * (currentDistance /settings.maxDistance) ); // = UpdateTargetPosition(lookAtTarget, settings.desiredDistance);
        }
       
        
        private void TryScrollZoom(in Vector3 target, ref Vector3 newPosition)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
          
            
            // In case of scroll
            if (!(Mathf.Abs(scroll) > 0.01f)) 
                return;

            // Ignore zooming if mouse is over UI elements
            if (eventSystem != null && raycaster != null)
            {
                //Set up the new Pointer Event
                //Set the Pointer Event Position to that of the mouse position
                pointerEventData = new PointerEventData(eventSystem) {position = Input.mousePosition};
                
                //Create a list of Raycast Results
                var results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                raycaster.Raycast(pointerEventData, results);

                if (results.Count != 0) return;
            }

            cam.AdaptiveZoom(scroll, 
                             ref settings.desiredDistance,
                             settings.zoomSpeed,
                             settings.zoomDistanceThreshold,
                             settings.minDistance,
                             settings.maxDistance,
                             settings.raycastableLayers,
                             settings.zoomDistanceCurve);

            newPosition = UpdateTargetPosition(looseTarget, settings.desiredDistance);
        }


        public void ZoomExtents(ref Vector3 newPosition)
        {
            looseTarget = fixedTarget;
            cam.ZoomExtents(targetObject.bounds, ref settings.desiredDistance, settings.zoomExtentsFactor);
            newPosition = UpdateTargetPosition(fixedTarget, settings.desiredDistance);
        }

        
        private Vector3 UpdateTargetPosition(Vector3 lookAt, float desiredDistance)
        {
            // Zoom Direction
            var newDir = lookAt - transform.position;
                    
            // Current Distance from target
            var newDist = newDir.magnitude;
                   
            // Difference between current and desired distance
            var error = newDist - desiredDistance;
                   
            // Movement force
            var zForce = newDir.normalized * error;

            // Target Positon
           return transform.position + zForce;
        }
        
      
        private void ApplyMotion()
        {
            // Slowly move
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.1f);

            // Slowly change the target
            tempTarget = settings.fixedTarget
                                 ? fixedTarget
                                 : Vector3.SmoothDamp(tempTarget, looseTarget, ref targetVelocity, 1 /settings.targetSlideSpeed);
            
            // Look at the target
            transform.LookAt(tempTarget);

            // Report current distance
            currentDistance = (transform.position - (settings.fixedTarget ? fixedTarget: looseTarget)).magnitude; 
        }
        

    }
}