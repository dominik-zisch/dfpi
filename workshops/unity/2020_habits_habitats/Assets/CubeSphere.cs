using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSphere : MonoBehaviour
{
    public int gridSize;
    [Range(0,10)]
    public float radius = 1;
    
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color32[] cubeUV;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Generate () 
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Sphere";
        CreateVertices();
        CreateTriangles();
        CreateColliders();
    }
    
    private void SetVertex (int i, int x, int y, int z) {
        Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
        normals[i] = v.normalized;
        vertices[i] = normals[i] * radius;
        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    private void CreateColliders()
    {
        gameObject.AddComponent<SphereCollider>();
    }

    private void CreateTriangles()
    {
        throw new System.NotImplementedException();
    }

    private void CreateVertices()
    {
        throw new System.NotImplementedException();
    }
}
