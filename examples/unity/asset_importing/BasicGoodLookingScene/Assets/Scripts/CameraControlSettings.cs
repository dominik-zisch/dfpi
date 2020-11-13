using UnityEngine;

namespace UnchartedLimbo.Core.Runtime.CameraControls
{
    [CreateAssetMenu(menuName = "Uncharted Limbo/Settings/Camera Control Settings", fileName = "Camera Control Settings")]
    public class CameraControlSettings : ScriptableObject
    {
        
        public bool fixedTarget;
        
        /// <summary>
        /// Current desired distance from target
        /// </summary>
        [Range(1, 3000)]
        public float desiredDistance;

        /// <summary>
        /// The camera cannot come closer to the target than this.
        /// </summary>
        [Range(0,3000)]
        public float minDistance = 0;

        /// <summary>
        /// The camera cannot move further from the target than this
        /// </summary>
        [Range(0,10000)]
        public float maxDistance = 3000;
        
        /// <summary>
        /// How fast the camera rotates
        /// </summary>
        [Range(0, 500)]
        public float orbitSpeed = 2;

        /// <summary>
        /// How fast the camera moves to its target position
        /// </summary>
        [Range(0, 200)] 
        public float zoomSpeed = 2;

        /// <summary>
        /// How fast the target will move to its new position
        /// </summary>
        [Range(0, 200)] 
        public float targetSlideSpeed = 1;

        /// <summary>
        /// Layers that will be raycast
        /// </summary>
        public LayerMask raycastableLayers;
        
        /// <summary>
        /// This curve controls the rate of zoom speed change.
        /// </summary>
        public AnimationCurve zoomDistanceCurve;
        
        /// <summary>
        /// Distances above this will remap to maximum Zoom speed.
        /// </summary>
        public float zoomDistanceThreshold = 5;


        public float zoomExtentsFactor = 1.2f;
    }
}
