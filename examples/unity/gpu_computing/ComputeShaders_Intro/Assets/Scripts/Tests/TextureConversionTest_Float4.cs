using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class TextureConversionTest_Float4 : TextureConversionTestBase<float4>
{
    protected override float4 ExtractPixelValue(Color pixel)
    {
        return new float4(pixel.r, pixel.g, pixel.b, pixel.a);
    }
}
