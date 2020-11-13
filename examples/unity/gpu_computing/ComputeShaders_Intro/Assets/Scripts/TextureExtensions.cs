using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

public static class TextureExtensions
{
    private const string RENDER_TEXTURE_CONVERSION_SHADER_PATH        = "ComputeShaders/TextureToBufferConversions";
    private const string RENDER_TEXTURE_CONVERSION_SHADER_KERNEL2D_NAME = "RenderTexture2DToBuffer_Float";
    private const string RENDER_TEXTURE_CONVERSION_SHADER_KERNEL3D_NAME = "RenderTexture3DToBuffer_Float";
    private const string RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_TEXTURE2D_NAME = "Input2DTexture_Float";
    private const string RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_TEXTURE3D_NAME = "Input3DTexture_Float";
    private const string RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_BUFFER_NAME = "OutputBuffer_Float";
    
    private static ComputeShader RenderTextureConversionShader = Resources.Load<ComputeShader>(RENDER_TEXTURE_CONVERSION_SHADER_PATH);

    private static bool CheckResources()
    {
        if (RenderTextureConversionShader == null)
        {
            RenderTextureConversionShader = Resources.Load<ComputeShader>(RENDER_TEXTURE_CONVERSION_SHADER_PATH);
           
            if (RenderTextureConversionShader == null)
            {
                throw new Exception($"{nameof(RenderTextureConversionShader)} was not found." +
                                    $"Check that your ComputeShaders are in Assets/Resources/ComputeShaders");
                return false;
            }
        }

        return true;
    }

    
    public static int BytesPerVoxel(this Texture3D t3d)
    {
        int count     = t3d.width * t3d.height * t3d.depth;
        int byteCount = t3d.GetPixelData<byte>(0).Length;

        return byteCount / count;
    }
  
    public static int BytesPerPixel(this Texture2D t2d)
    {
        int count     = t2d.width * t2d.height;
        int byteCount = t2d.GetPixelData<byte>(0).Length;

        return byteCount / count;
    }
    
    public static int ChannelCount(this Texture tex)
    {
        var channels = 0;
        
        switch (tex.graphicsFormat)
        {
            case GraphicsFormat.None:
                channels = 0;
                break;
            case GraphicsFormat.R32_SFloat:
            case GraphicsFormat.R32_SInt:
            case GraphicsFormat.R32_UInt:
            case GraphicsFormat.R16_SFloat:
            case GraphicsFormat.R16_SInt:
            case GraphicsFormat.R16_SNorm:
            case GraphicsFormat.R16_UInt:
            case GraphicsFormat.R16_UNorm:
            case GraphicsFormat.R8_SInt:
            case GraphicsFormat.R8_SNorm:
            case GraphicsFormat.R8_UInt:
            case GraphicsFormat.R8_UNorm:
            case GraphicsFormat.R8_SRGB:
                channels = 1;
                break;
            case GraphicsFormat.R32G32_SFloat:
            case GraphicsFormat.R32G32_SInt:
            case GraphicsFormat.R32G32_UInt:
            case GraphicsFormat.R16G16_SFloat:
            case GraphicsFormat.R16G16_SInt:
            case GraphicsFormat.R16G16_SNorm:
            case GraphicsFormat.R16G16_UInt:
            case GraphicsFormat.R16G16_UNorm: 
            case GraphicsFormat.R8G8_SInt:
            case GraphicsFormat.R8G8_SNorm:
            case GraphicsFormat.R8G8_UInt:
            case GraphicsFormat.R8G8_UNorm:
            case GraphicsFormat.R8G8_SRGB: 
                channels = 2;
                break;
            case GraphicsFormat.R32G32B32_SFloat:
            case GraphicsFormat.R32G32B32_SInt:
            case GraphicsFormat.R32G32B32_UInt:
            case GraphicsFormat.R16G16B16_SFloat:
            case GraphicsFormat.R16G16B16_SInt:
            case GraphicsFormat.R16G16B16_SNorm:
            case GraphicsFormat.R16G16B16_UInt:
            case GraphicsFormat.R16G16B16_UNorm: 
            case GraphicsFormat.R8G8B8_SInt:
            case GraphicsFormat.R8G8B8_SNorm:
            case GraphicsFormat.R8G8B8_UInt:
            case GraphicsFormat.R8G8B8_UNorm:
            case GraphicsFormat.R8G8B8_SRGB: 
            case GraphicsFormat.B8G8R8_SRGB:
            case GraphicsFormat.B8G8R8_UNorm:
            case GraphicsFormat.B8G8R8_SNorm:
            case GraphicsFormat.B8G8R8_UInt:
            case GraphicsFormat.B8G8R8_SInt:
                channels = 3;
                break;
            case GraphicsFormat.R32G32B32A32_SFloat:
            case GraphicsFormat.R32G32B32A32_SInt:
            case GraphicsFormat.R32G32B32A32_UInt:
            case GraphicsFormat.R16G16B16A16_SFloat:
            case GraphicsFormat.R16G16B16A16_SInt:
            case GraphicsFormat.R16G16B16A16_SNorm:
            case GraphicsFormat.R16G16B16A16_UInt:
            case GraphicsFormat.R16G16B16A16_UNorm: 
            case GraphicsFormat.R8G8B8A8_SInt:
            case GraphicsFormat.R8G8B8A8_SNorm:
            case GraphicsFormat.R8G8B8A8_UInt:
            case GraphicsFormat.R8G8B8A8_UNorm:
            case GraphicsFormat.R8G8B8A8_SRGB: 
            case GraphicsFormat.B8G8R8A8_SRGB:
            case GraphicsFormat.B8G8R8A8_UNorm:
            case GraphicsFormat.B8G8R8A8_SNorm:
            case GraphicsFormat.B8G8R8A8_UInt:
            case GraphicsFormat.B8G8R8A8_SInt:
                channels = 4;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return channels;
    }
    
    public static Color[] GetPixels(this RenderTexture rt)
    {
        if (rt == null)
            return null;
        
        if (rt.dimension == TextureDimension.Tex2D)
        {
            var t2d    = rt.ToTexture2D();
            
            var pixels = t2d.GetPixels();
          
            if (Application.isPlaying)
            {
                Object.Destroy(t2d);
            }
            else
            {
                Object.DestroyImmediate(t2d);
            }
            return pixels;
        }
        else
        {
            throw new NotImplementedException("Sorry Can't read pixels from something that isn't a Texture2D");
        }
    }

    
    public static Texture2D ToTexture2D(this RenderTexture rt)
    {
        if (rt.dimension != TextureDimension.Tex2D) 
            return null;
       
        var t2d  = new Texture2D(rt.width, rt.height, rt.graphicsFormat, TextureCreationFlags.None);
        var temp = RenderTexture.active;
        
        RenderTexture.active = rt;
        t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        t2d.Apply();
        RenderTexture.active = temp;
        
        return t2d;
    }
    
    
    public static RenderTexture ToRenderTexture(this Texture3D t3d)
    {
        var temp = new RenderTexture(t3d.width, t3d.height, 0, t3d.graphicsFormat)
        {
                dimension = TextureDimension.Tex3D, 
                enableRandomWrite = true, 
                volumeDepth = t3d.depth
        };
        
        temp.Create();
        
        Graphics.CopyTexture(t3d, temp);
        
        return temp;
    }
    
    public static RenderTexture ToRenderTexture(this Texture2D t2d)
    {
        var temp = new RenderTexture(t2d.width, t2d.height, 0, t2d.graphicsFormat)
        {
                dimension         = TextureDimension.Tex2D, 
                enableRandomWrite = true
        };
        
        temp.Create();
        
        Graphics.CopyTexture(t2d, temp);
        
        return temp;
    }
    
    
    public static ComputeBuffer ToComputeBuffer(this Texture3D t3d)
    {
        if (t3d                == null) return null;
        if (t3d.ChannelCount() == 0) return null;
        
        var temp   = t3d.ToRenderTexture();
        var buffer = temp.ToComputeBuffer();
    
        temp.Release();
      
        return buffer;
    }
    
    public static ComputeBuffer ToComputeBuffer(this Texture2D t2d)
    {
        if (t2d                == null) 
            return null;
      
        if (t2d.ChannelCount() == 0) 
            return null;
        
        var temp   = t2d.ToRenderTexture();
        var buffer = temp.ToComputeBuffer();

        temp.Release();

        return buffer;
    }
    
    
    public static ComputeBuffer ToComputeBuffer(this RenderTexture tex, int stride = 4)
    {
        if (!CheckResources())
            return null;
        
        if (tex == null)
            return null;
        
        var shader       = RenderTextureConversionShader;
        var channelCount = tex.ChannelCount() - 1;
        var suffixes     = new[] {"", "2", "3", "4"};
        var suffix       = suffixes[channelCount];
        var bufferName   = RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_BUFFER_NAME + suffix;

        var  count       = 0;
        var  kernelIndex = 0;
        int3 threadGroups;
        var  texName    = "";
      
        if (tex.dimension == TextureDimension.Tex2D)
        {
            count        = tex.width * tex.height;
            threadGroups = new int3(tex.width /8, tex.height /8, 1);

            kernelIndex  = shader.FindKernel(RENDER_TEXTURE_CONVERSION_SHADER_KERNEL2D_NAME + suffix);
            texName      = RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_TEXTURE2D_NAME + suffix;
        }
        else if (tex.dimension == TextureDimension.Tex3D)
        {
            count        = tex.width * tex.height * tex.volumeDepth;
            threadGroups = new int3(tex.width / 8, tex.height / 8, tex.volumeDepth / 8);
          
            kernelIndex  = shader.FindKernel(RENDER_TEXTURE_CONVERSION_SHADER_KERNEL3D_NAME + suffix);
            texName      = RENDER_TEXTURE_CONVERSION_SHADER_KERNEL_TEXTURE3D_NAME + suffix;
        }
        else
        {
            throw new NotImplementedException();
        }
        
        if (count == 0)
            return null;

        var buffer = new ComputeBuffer(count, stride);
        
        shader.SetTexture(kernelIndex, texName, tex);
        shader.SetBuffer(kernelIndex, bufferName, buffer);
        shader.Dispatch(kernelIndex, threadGroups.x, threadGroups.y, threadGroups.z);

        return buffer;
    }
    
    private static ComputeBuffer ToComputeBufferOfType<T>(this NativeArray<byte> array) where T : struct
    {
        var pixelData = array.Reinterpret<T>(1);
        var buffer    = new ComputeBuffer(array.Length / UnsafeUtility.SizeOf<T>(), UnsafeUtility.SizeOf<T>());
        buffer.SetData(pixelData);
        return buffer;
    }

}
