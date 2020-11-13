using UnityEngine;

namespace UnchartedLimbo.Core.Runtime.CameraControls
{
    public static class CameraExtensions 
    {
        public static Vector3 ZoomExtentsImmediate(this Camera camera, in Bounds bounds, float factor = 2)
        {
            var cameraDistance = factor; // Constant factor
            var objectSizes = bounds.max - bounds.min;
            var objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            var cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView); // Visible height 1 meter in front
            var distance =  cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object

            return bounds.center + (camera.transform.position - bounds.center).normalized * distance;

        }
        
        public static void ZoomExtents(this Camera camera, in Bounds bounds, ref float distance, float factor = 2)
        {
            var cameraDistance = factor; // Constant factor
            var objectSizes = bounds.max - bounds.min;
            var objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            var cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView); // Visible height 1 meter in front
            distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            //return Bounds.center - distance * camera.transform.forward;
        }
        
        public static void AdaptiveZoom(this Camera cam, float zoomAmount,
                                        ref float      distance,
                                        float          zoomSpeed                 = 1,
                                        float          maxSpeedDistanceThreshold = 5,
                                        float          minZoom                   = 0,
                                        float          maxZoom                   = 3000,
                                        int            layerMask                 = 0,
                                        AnimationCurve zoomSpeedRate             = null,
                                        float          maxRaycastDist            = 10000)
        {
            var adaptiveZoomSpeed = zoomSpeed;
            
            // Raycast the scene
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, maxRaycastDist, layerMask))
            {
                
                // Distance to zoom target
                var _dist = (hit.point - cam.transform.position).magnitude;

                // Normalize distance
                var normDist = Mathf.Clamp01(Mathf.InverseLerp(0, maxSpeedDistanceThreshold, _dist));

                // Adjust zoom speed based on distance. The closer the camera is to a target, the slower it will zoom
                 adaptiveZoomSpeed = Mathf.Lerp(0, zoomSpeed, zoomSpeedRate?.Evaluate(normDist) ?? normDist);
            }
            
            // Adjust the Distance (Zoom)
            distance += adaptiveZoomSpeed * -zoomAmount;

            // Clamp the Distance
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
        }
    }
} 