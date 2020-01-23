using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ProceduralGrid : MonoBehaviour
{
    private static readonly float3 WORLD_UP = new float3(0, 1, 0);
    
    [Range(1,20)]
    public int resolutionX;
    [Range(1,20)]
    public int resolutionY;
    [Range(0,10)]
    public float tileSize;
    
    
    public List<Vector3> vertices;
    public List<int> indices;
    public List<int2> occupancyMap;
    
    public int tileCount;
    
    // PROPERTIES
    private int nextIndex => tileCount * 4;
    private Vector3 forward => transform.forward;
    private Vector3 right => transform.right;
    private Vector3 up => transform.up;
    private float3 bottomCorner(int2 ij) => (right * ij.x + up * ij.y) * tileSize;

    
    private Mesh mesh;
    private MeshFilter mf;
    
    
    private void OnEnable()
    {
        vertices = new List<Vector3>();
        indices = new List<int>();
        occupancyMap = new List<int2>();
        
        mf = GetComponent<MeshFilter>();
        mesh = mf.sharedMesh;
        
        if (mesh != null) return;
        
        mesh = new Mesh();
        mf.sharedMesh = mesh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddQuad_HighLevel(new int2(Random.Range(0,resolutionX),Random.Range(0,resolutionY)));
        }
    }
    
    public void AddCube()
    {}
    
    public void AddGrid()
    {}
    
    /// <summary>
    /// LEVEL 4: Errors need to be handled, and the execution order needs to be preserved.
    /// </summary>
    public void AddQuad_HighLevel(int2 ij)
    {
        if (ij.x > resolutionX || ij.y > resolutionY) return;

        if (occupancyMap.Contains(ij)) return;

        AddQuad_LowLevel(ij);

        UpdateMesh();

        occupancyMap.Add(ij);

        tileCount ++;
    }

     /// <summary>
     /// LEVEL 3: The mesh needs to be informed about the changes.
     /// </summary>
    void UpdateMesh()
    {
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Quads,0);
    }
    
    /// <summary>
    /// LEVEL 2: The vertices and indices need to be put in the right place.
    /// </summary>
    void AddQuad_LowLevel(int2 ij)
    {
        var verts = GenerateQuad_Vertices(bottomCorner(ij), right, up, tileSize);
        var inds = GenerateQuad_Indices(nextIndex);
        
        vertices.AddRange(verts);
        indices.AddRange(inds);
    }
    
    /// <summary>
    /// LEVEL 1: A mesh needs indices in order to connect its vertices and form triangles
    /// </summary>
    int[] GenerateQuad_Indices(int currentIndex)
    {
        return new[] 
        {
            currentIndex + 0, 
            currentIndex + 1, 
            currentIndex + 2, 
            currentIndex + 3
        };
    }
    
    /// <summary>
    /// LEVEL 0: A mesh is nothing without its vertices
    /// </summary>
    Vector3[] GenerateQuad_Vertices(Vector3 _zero, Vector3 _right, Vector3 _up, float _size)
    {
        var points = new Vector3[4];
        
        points[0] = _zero ;
        points[1] = _zero + _right * _size;
        points[2] = _zero + (_right + _up) * _size;
        points[3] = _zero + _up * _size;
        
        return points;
    }


}

