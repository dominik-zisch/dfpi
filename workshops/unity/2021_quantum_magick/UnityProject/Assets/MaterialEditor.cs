using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEditor : MonoBehaviour
{
    public MeshRenderer mr;
    
    public void SetMaterialColor(Vector4 color)
    {
        mr.material.color =  color;
    }
    
    public void SetMaterialColorFromPosition(Vector3 position)
    {
        mr.material.color = new Color(position.x, position.y, position.z);
    }
}
