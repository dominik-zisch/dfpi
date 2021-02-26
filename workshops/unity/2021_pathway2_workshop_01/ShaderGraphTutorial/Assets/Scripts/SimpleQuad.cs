using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    ForwardFacing,
    BackwardFacing
};

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteAlways]
public class SimpleQuad : MonoBehaviour
{
    
    #region Public Variables

    public float width = 1;
    public float height = 1;
    public Direction direction = Direction.ForwardFacing;
    
    public Mesh GeneratedMesh;

    #endregion
    
    #region Private Variables
    
    private MeshFilter mf;
    private bool changed = false;
    
    #endregion

    #region MonoBehavior Methods
    
    void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load("Material/Standard", typeof(Material)) as Material;
        Generate();
    }
    
    private void OnValidate()
    {
        Generate();
    }
    
    private void Update()
    {
        if (mf == null) mf = GetComponent<MeshFilter>();

        if (changed)
        {
            mf.sharedMesh = new Mesh();
            mf.sharedMesh.vertices = GeneratedMesh.vertices;
            mf.sharedMesh.triangles = GeneratedMesh.triangles;
            mf.sharedMesh.uv = GeneratedMesh.uv;
            mf.sharedMesh.normals = GeneratedMesh.normals;
            mf.sharedMesh.name = GeneratedMesh.name;
            changed = false;
        }
    }

    #endregion

    #region Custom methods

    private void Generate()
    {
        Mesh mesh = new Mesh();
        
        // Vertex array
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;
        
        // Triangles
        if (direction == Direction.ForwardFacing)
        {
            int[] triangles = new int[]
            {
                0, 2, 1,
                2, 3, 1
            };
            mesh.triangles = triangles; 
        }
        else
        {
            int[] triangles = new int[]
            {
                0, 1, 2,
                2, 1, 3
            };
            mesh.triangles = triangles; 
        }
        
        Vector3[] normals = new Vector3[]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;
        
        // Texture coordinates
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        mesh.name = "SimpleQuad";

        GeneratedMesh = mesh;
        changed = true;
    }
    
    #endregion
}
