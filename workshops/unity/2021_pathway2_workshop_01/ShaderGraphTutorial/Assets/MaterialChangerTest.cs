using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangerTest : MonoBehaviour
{
        void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Material mat =  GetComponent<MeshRenderer>().material;
      
            mat.SetColor("_ColorA", Random.ColorHSV()); 
            mat.SetColor("_ColorB", Random.ColorHSV());

        }
    }
}
