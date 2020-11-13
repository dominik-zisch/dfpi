using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class TextureConversionTest_Float2 : TextureConversionTestBase<float2>
{
    protected override float2 ExtractPixelValue(Color pixel)
    {
        return new float2(pixel.r, pixel.g);
    }
}
