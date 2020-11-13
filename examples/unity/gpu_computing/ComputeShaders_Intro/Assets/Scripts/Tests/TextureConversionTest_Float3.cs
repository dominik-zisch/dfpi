using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class TextureConversionTest_Float3 : TextureConversionTestBase<float3>
{
    protected override float3 ExtractPixelValue(Color pixel)
    {
        return new float3(pixel.r, pixel.g, pixel.b);
    }
}
