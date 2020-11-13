using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public abstract class TextureConversionTestBase <T>:MonoBehaviour where T:struct, IEquatable<T>
    {
        public bool fromStartOfArray;
    
        [Range(1, 1000)]
        public int count = 10;

        [Header("TEXTURE3D TEST")]
        public Texture3D originalTexture3D;
        public  T[]       originalValues_Texture3D;
        public  T[]       outputValues_Texture3D;
        protected ComputeBuffer outputBuffer_Texture3D;

        [Header("RENDER TEXTURE (2D) TEST")]
        public RenderTexture originalRenderTexture2D;
        public    T[]           originalValues_RenderTexture;
        public    T[]           outputValues_RenderTexture;
        protected ComputeBuffer outputBuffer_RenderTexture;
        
        private void Start()
        {
            InitializeTestArrays();
        }
    
        private void OnValidate()
        {
            InitializeTestArrays();
            Test();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) 
                return;
        
            Test();
        }

        
        protected abstract T ExtractPixelValue(Color pixel);
       
        
        protected void InitializeTestArrays()
        {
            if (originalTexture3D != null)
            {
                originalValues_Texture3D = new T[count];
                outputValues_Texture3D   = new T[count];
                // Get original pixel values from T3D
                var pixels     = originalTexture3D.GetPixels();
                var cnt        = math.min(count, pixels.Length);
                var firstValue = fromStartOfArray ? 0 : pixels.Length - cnt;

                for (int i = 0; i < count; i++)
                    originalValues_Texture3D[i] = ExtractPixelValue(pixels[firstValue + i]);
            }

            if (originalRenderTexture2D)
            {
                originalValues_RenderTexture = new T[count];
                outputValues_RenderTexture   = new T[count];
                // Get original pixel values from RT
                var pixels2 = originalRenderTexture2D.GetPixels();
                var cnt        = math.min(count, pixels2.Length);
                var firstValue = fromStartOfArray ? 0 :pixels2.Length - cnt;

                for (int i = 0; i < count; i++)
                    originalValues_RenderTexture[i] = ExtractPixelValue(pixels2[firstValue + i]);  
            }

        }
        
        protected void Test()
        {
            Clear();
     
            ExecuteTestCore();
        
            CompareTestResults(originalValues_Texture3D, outputValues_Texture3D);
       
            CompareTestResults(originalValues_RenderTexture, outputValues_RenderTexture);
        }
        
        protected void ExecuteTestCore()
        {
            if (originalTexture3D != null)
            {
                // MAIN CONVERSION
                outputBuffer_Texture3D = originalTexture3D.ToComputeBuffer();
            
                // DATA READ-BACK
                var cnt = math.min(count, outputBuffer_Texture3D.count);

                var firstValue = fromStartOfArray ? 0 : outputBuffer_Texture3D.count - cnt;
                outputBuffer_Texture3D?.GetData(outputValues_Texture3D, 0, firstValue, cnt);
            }

            if (originalRenderTexture2D)
            {
                // MAIN CONVERSION
                outputBuffer_RenderTexture = originalRenderTexture2D.ToComputeBuffer();
                
                // DATA READ-BACK
                var cnt        = math.min(count, outputBuffer_RenderTexture.count);
                var firstValue = fromStartOfArray ? 0 : outputBuffer_RenderTexture.count -cnt;
                outputBuffer_RenderTexture?.GetData(outputValues_RenderTexture, 0, firstValue, cnt);  
            }
        }
        
        protected static void CompareTestResults<T>(T[] A, T[] B)where T:IEquatable<T>
        {
            if (A == null || B == null)
                return;
            
            Assert.AreEqual(A.Length, B.Length);
      
            for (var i = 0; i < A.Length; i++)
            {
                Assert.AreEqual(A[i], B[i]);
            }
        }
        
        protected void Clear()
        {
            outputBuffer_Texture3D?.Dispose();
            outputBuffer_RenderTexture?.Dispose();
        }

    }
}