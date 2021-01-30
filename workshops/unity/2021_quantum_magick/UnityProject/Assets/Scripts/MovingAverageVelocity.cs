using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace UnchartedLimbo.Tools.VoxelUtils.Runtime.Tracking
{
    public class MovingAverageVelocity : MonoBehaviour
    {
        [Serializable]
        public class VectorChangedEvent:UnityEvent<Vector3>
        {
            
        }
        
        public Transform followPoint;
        
        [Range(3,100)]
        public int                     bufferSize = 20;

        public float3 averagePosition;
        public float3 averageVelocity;
        public float3 averageAcceleration;
        public float3 averageJerk;
        
        public VectorChangedEvent OnPositionChanged;
        public VectorChangedEvent OnVelocityChanged;
        public VectorChangedEvent OnAccelerationChanged;
        public VectorChangedEvent OnJerkChanged;
        
        private CircularBuffer<float3> positionBuffer;
        private CircularBuffer<float3> velocityBuffer;
        private CircularBuffer<float3> accelerationBuffer;
        private CircularBuffer<float3> jerkBuffer;
        
        private float3 prevPos;
        private float3 prevVel;
        private float3 prevAccel;
        
        private void Start()
        {
            positionBuffer     = new CircularBuffer<float3>(bufferSize);
            velocityBuffer = new CircularBuffer<float3>(bufferSize);
            accelerationBuffer = new CircularBuffer<float3>(bufferSize);
            jerkBuffer = new CircularBuffer<float3>(bufferSize);
        }

        private void FixedUpdate()
        {
            if (followPoint == null)
                followPoint = transform;
            
            var position = (float3) followPoint.position;
            var velocity = (position - prevPos) / Time.fixedDeltaTime;
            
            if (math.lengthsq(velocity) < math.EPSILON * 10)
                return;
            
            var acceleration = (velocity - prevVel)   / Time.fixedDeltaTime;
            var jerk         = (acceleration - prevAccel) / Time.fixedDeltaTime;
            
            positionBuffer.Put(position);
            velocityBuffer.Put(velocity);
            accelerationBuffer.Put(acceleration);
            jerkBuffer.Put(jerk);
            
            (averagePosition, averageVelocity, averageAcceleration, averageJerk) = CalculateMovingAverage();

            prevPos   = position;
            prevVel   = velocity;
            prevAccel = acceleration;
            
            OnPositionChanged.Invoke(averagePosition);
            OnVelocityChanged.Invoke(averageVelocity);
            OnAccelerationChanged.Invoke(averageAcceleration);
            OnJerkChanged.Invoke(averageJerk);
        }

        private (float3, float3, float3, float3) CalculateMovingAverage()
        {
            var _velocities    = new NativeArray<float3>(velocityBuffer.Peek(bufferSize),     Allocator.TempJob);
            var _positions     = new NativeArray<float3>(positionBuffer.Peek(bufferSize),     Allocator.TempJob);
            var _accelerations = new NativeArray<float3>(accelerationBuffer.Peek(bufferSize), Allocator.TempJob);
            var _jerks         = new NativeArray<float3>(jerkBuffer.Peek(bufferSize),         Allocator.TempJob);
            
            var _averageVelocity =  new NativeArray<float3>(1, Allocator.TempJob);
            var _averagePoint    =  new NativeArray<float3>(1, Allocator.TempJob);
            var _averageAccel    =  new NativeArray<float3>(1, Allocator.TempJob);
            var _averageJerk     =  new NativeArray<float3>(1, Allocator.TempJob);
            
            new MovingAverageJob
            {
                    positions = _positions,
                    velocities = _velocities,
                    accelerations = _accelerations,
                    jerks = _jerks,
                    averageVelocity = _averageVelocity,
                    averagePosition = _averagePoint,
                    averageAccel = _averageAccel,
                    averageJerk = _averageJerk
            }.Schedule().Complete();

            var avgP = _averagePoint[0];
            var avgV = _averageVelocity[0];
            var avgA = _averageAccel[0];
            var avgJ = _averageJerk[0];

            _velocities.Dispose();
            _positions.Dispose();
            _accelerations.Dispose();
            _jerks.Dispose();

            _averageVelocity.Dispose();
            _averagePoint.Dispose();
            _averageAccel.Dispose();
            _averageJerk.Dispose();
            
            return (avgP,avgV, avgA, avgJ);
        }

        [BurstCompile]
        private struct MovingAverageJob : IJob
        {
            [ReadOnly]
            public NativeArray<float3> positions;
            [ReadOnly]
            public NativeArray<float3> velocities;
            [ReadOnly]
            public NativeArray<float3> accelerations;
            [ReadOnly]
            public NativeArray<float3> jerks;

            public NativeArray<float3> averageVelocity;
            public NativeArray<float3> averagePosition;
            public NativeArray<float3> averageAccel;
            public NativeArray<float3> averageJerk;
           
            public void Execute()
            {
                var cnt = velocities.Length;
                
                for (var i = 0; i < cnt; i++)
                {
                    averageVelocity[0] += velocities[i];
                    averagePosition[0] += positions[i];
                    averageAccel[0]    += accelerations[i];
                    averageJerk[0]     += jerks[i];
                }

                averageVelocity[0] /= cnt;
                averagePosition[0] /= cnt;
                averageAccel[0]    /= cnt;
                averageJerk[0]     /= cnt;
            }
        }
    }
}
