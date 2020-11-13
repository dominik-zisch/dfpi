using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class SimpleDistanceField : MonoBehaviour
{
    public enum Algorithm
    {
        PointSDF,
        VolumeSlice
    }

    [Header("References")]
    public ComputeShader computeShader;
   
    [Header("Inputs")]
    public Algorithm algorithm;
    public BoxCollider bounds;
    public Texture3D   volume;
    public Transform   point;
    public Transform   plane;
    
    [Header("Outputs")]
    public RenderTexture slice;
  
    
    private int           _kernelIndex;
    private Vector3Int    _threadsPerGroup;
    private RenderTexture _tempTexture2D;
    private RenderTexture _tempTexture3D;
    private Algorithm     _previousAlgorithm;

    private const string PointSDFKernelName     = "DistanceToPoint";
    private const string VolumeSlicerKernelName = "ReadSDF";


    //public ComputeBuffer buffer;
    
    private void Start()
    {
        if (computeShader == null)
            return;
        
        Init(CurrentKernel);
    }

    private void Update()
    {
        if (computeShader == null)
            return;

        if (_previousAlgorithm != algorithm)
        {
           Init(CurrentKernel);
           _previousAlgorithm = algorithm;
        }   
        
        Schedule();

        Execute();
    }

    private void OnDestroy()
    {
        Clear();
    }

    private void OnDisable()
    {
        Clear();
    }

    private void OnApplicationQuit()
    {
        Clear();
    }


    private string CurrentKernel
    {
        get
        {
            switch (algorithm)
            {
                case Algorithm.PointSDF:
                    return PointSDFKernelName;
                case Algorithm.VolumeSlice:
                    return VolumeSlicerKernelName;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void Init(string kernelName)
    {
        _kernelIndex = computeShader.FindKernel(kernelName);
        computeShader.GetKernelThreadGroupSizes(_kernelIndex, out var x, out var y, out var z);
        _threadsPerGroup = new Vector3Int((int) x, (int) y, (int) z);

        Clear();

        _tempTexture2D = new RenderTexture(slice.width, slice.height, 0, slice.graphicsFormat)
        {
                enableRandomWrite = true
        };
        _tempTexture2D.Create();

        _tempTexture3D = new RenderTexture(volume.width, volume.height, 0, volume.graphicsFormat)
        {
                volumeDepth       = volume.depth,
                dimension         = TextureDimension.Tex3D,
                enableRandomWrite = true
        };
        _tempTexture3D.Create();

        Graphics.CopyTexture(volume, _tempTexture3D);
    }

    private void Schedule()
    {
       computeShader.SetTexture(_kernelIndex, "SDF", _tempTexture2D);
       computeShader.SetTexture(_kernelIndex, "VF", _tempTexture3D);

       computeShader.SetVector("Point", point.position);
       computeShader.SetVector("TexDims", new Vector4(slice.width,slice.height));
       computeShader.SetMatrix("DomainTransformation", plane.localToWorldMatrix);
    }

    private void Execute()
    {
        int width  = slice.width;
        int height = slice.height;
        int depth  = 1; //slice.volumeDepth;

        int xThreads = _threadsPerGroup.x;
        int yThreads = _threadsPerGroup.y;
        int zThreads = _threadsPerGroup.z;

        int xGroups = width  / xThreads;
        int yGroups = height / yThreads;
        int zGroups = depth  / zThreads;
        
        computeShader.Dispatch(_kernelIndex,xGroups,yGroups,zGroups); 
        
        Graphics.Blit(_tempTexture2D, slice);
    }

    private void Clear()
    {
        if (_tempTexture2D!=null)
            _tempTexture2D.Release();      
       
        if (_tempTexture3D != null)
            _tempTexture3D.Release();
    }
    
}

