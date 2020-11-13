using System;
using DefaultNamespace;
using UnityEngine;

public class TextureConversionTest_Float : TextureConversionTestBase<float>
{
    protected override float ExtractPixelValue(Color pixel)
    {
        return pixel.r;
    }
}
