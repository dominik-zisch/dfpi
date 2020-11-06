using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleDistanceField : MonoBehaviour
{
    public  ComputeShader computeShader;
    public  RenderTexture sdfTex;
    public  Transform     Point;
    public  Transform     Plane;
    
    private int           kernelIndex;
    private Vector3Int    threadsPerGroup;
    private RenderTexture tempTexture2D;
    private RenderTexture tempTexture3D;

    public Texture3D VF;
    
    //public ComputeBuffer buffer;
    
    private void Start()
    {
        if (computeShader == null)
            return;
        
       Init(); 
    }

    private void Update()
    {
        if (computeShader == null)
            return;
        
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
    
    
    
    
    private void Init()
    {
        //kernelIndex = computeShader.FindKernel("DistanceToPoint");
        kernelIndex = computeShader.FindKernel("ReadSDF"); 
        
        computeShader.GetKernelThreadGroupSizes(kernelIndex, out var x ,out var y, out var z);
        threadsPerGroup                 = new Vector3Int((int)x, (int)y, (int)z);
        
        tempTexture2D                   = new RenderTexture(sdfTex.width, sdfTex.height, 0, sdfTex.graphicsFormat);
        // IMPORTANT!
        tempTexture2D.enableRandomWrite = true;
        tempTexture2D.Create();

        tempTexture3D   = new RenderTexture(VF.width, VF.height, 0, VF.graphicsFormat);
        // NECESSARY FOR 3D TEXTURES
        tempTexture3D.volumeDepth       = VF.depth;
        tempTexture3D.dimension         = TextureDimension.Tex3D;
        tempTexture3D.enableRandomWrite = true;
        tempTexture3D.Create();
        
        Graphics.CopyTexture(VF,tempTexture3D);
       // Graphics.Blit(VF, tempTexture3D);

    }

    private void Schedule()
    {
       computeShader.SetTexture(kernelIndex, "SDF", tempTexture2D);
       computeShader.SetTexture(kernelIndex, "VF", tempTexture3D);

       computeShader.SetVector("Point", Point.position);
       computeShader.SetVector("TexDims", new Vector4(sdfTex.width,sdfTex.height));
       computeShader.SetMatrix("DomainTransformation", Plane.localToWorldMatrix);
    }

    private void Execute()
    {
        int width  = sdfTex.width;
        int height = sdfTex.height;
        int depth  = 1; //sdfTex.volumeDepth;

        int xThreads = threadsPerGroup.x;
        int yThreads = threadsPerGroup.y;
        int zThreads = threadsPerGroup.z;

        int xGroups = width  / xThreads;
        int yGroups = height / yThreads;
        int zGroups = depth  / zThreads;
        
        computeShader.Dispatch(kernelIndex,xGroups,yGroups,zGroups); 
        
        Graphics.Blit(tempTexture2D, sdfTex);
    }

    private void Clear()
    {
        tempTexture2D.Release();      
        tempTexture3D.Release();
        //buffer.Dispose();
    }
    
}

