using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]

public class ProceduralGrid : MonoBehaviour
{

    #region Public Variables

    [BoxGroup("Dimensions")] [Range(1, 100)] public int XCount;
    [BoxGroup("Dimensions")] [Range(1, 100)] public int YCount;
    [Space]
    [BoxGroup("Dimensions")] [Range(1, 100)] public float XSize;
    [BoxGroup("Dimensions")] [Range(1, 100)] public float YSize;

    [Space] 
    [BoxGroup("Settings")] [Dropdown("PlaneValues")] public string plane;
    [BoxGroup("Settings")] public bool reverse;
    [BoxGroup("Settings")] public bool StretchTexture;
    [BoxGroup("Settings")] public bool VisualizeVertices;
    [BoxGroup("Settings")] public bool CreateFaces;
    [BoxGroup("Settings")] public bool zNoise;

    [Space] 
    [BoxGroup("Noise")] [ShowIf("zNoise")] [MinValue(0)] public float MaxNoiseSize = 1;
    [BoxGroup("Noise")] [ShowIf("zNoise")] public float noiseXOffset = 0;
    [BoxGroup("Noise")] [ShowIf("zNoise")] public float noiseYOffset = 0;
    [BoxGroup("Noise")] [ShowIf("zNoise")] public float noiseXScale = 1;
    [BoxGroup("Noise")] [ShowIf("zNoise")] public float noiseYScale = 1;

    [BoxGroup("Noise Animator")] public bool animateNoise = false;
    [BoxGroup("Noise Animator")] [ShowIf("animateNoise")] public float xFactor = 0;
    [BoxGroup("Noise Animator")] [ShowIf("animateNoise")] public float yFactor = 0;

     public Mesh GeneratedMesh;

    #endregion

    #region Private Variables
    
    private Vector3[]    vertices;
    private Vector2[]    uvs;
    private int[]        triangles;
    private bool         Changed = false;
    private MeshFilter   mf;
    private MeshRenderer mr;
    private Mesh         mfMesh;
    private List<string> PlaneValues { get { return new List<string>() { "XY", "XZ", "YZ" }; } }

    #endregion

    #region MonoBehavior methods
    
    private void Awake()
    {
        // LOAD ALL THE THINGS THAT SHOULDN'T CHANGE WHEN THE GAME BEGINS
        // THIS IS CALLED CACHING
        mf            = GetComponent<MeshFilter>();
        mr            = GetComponent<MeshRenderer>();
        mr.material   = Resources.Load("Material/Standard", typeof(Material)) as Material;
        mf.sharedMesh = new Mesh();
        mfMesh        = mf.sharedMesh;
        mfMesh.MarkDynamic();
                   
        Generate();
    }

    private void OnValidate()
    {
        Generate();
    }

    private void Update()
    {
        if (Changed)
        {
            // COPY MESH DATA TO THE MESH FILTER
            MeshUtils.CopyMeshSlow(GeneratedMesh, mfMesh);
            
            Changed       = false;
        }

        // CONTINUOUSLY GENERATE A NEW MESH
        if (animateNoise)
        {
            noiseXOffset = noiseXOffset + xFactor * Time.deltaTime;
            noiseYOffset = noiseYOffset + yFactor * Time.deltaTime;
            Generate();
        }
    }

    private void OnDrawGizmos()
    {
        if (VisualizeVertices)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.01f);
            }
        }
    }

    
    // ALWAYS DEALLOCATE THE MESH WHEN IT'S NO LONGER RELEVANT
    private void OnApplicationQuit()
    {
        Destroy(GeneratedMesh);
        GeneratedMesh = null;
    }
    
    private void OnDisable()
    {
        Destroy(GeneratedMesh);
        GeneratedMesh = null;
    }
    
    private void OnDestroy()
    {
        Destroy(GeneratedMesh);
        GeneratedMesh = null;
    }
   
    #endregion


    #region Custom methods
  
    public void Generate()
    {
        if (GeneratedMesh != null)
        {
            // IF A MESH IS ALREADY GENERATED, WE JUST CLEAR IT !
            GeneratedMesh.Clear();
        }   
        else
        {
            GeneratedMesh = new Mesh();
        }

        GeneratedMesh.name = string.Format("Procedural Grid {0}x{1}", XCount, YCount);

        int XCount_Vertices = XCount + 1;
        int YCount_Vertices = YCount + 1;

        vertices = new Vector3[(XCount_Vertices) * (YCount_Vertices)];
        uvs = new Vector2[vertices.Length];
        
        // WE HAVE X * Y QUADS
        // EACH QUAD HAS 2 TRIANGLES, SO WE HAVE X * Y * 2 TRIANGLES
        // EACH TRIANGLE HAS 3 INDICES, SO WE HAVE X * Y * 2 * 3 INDICES
        triangles = new int[XCount * YCount * 6];

        for (int y = 0; y < YCount_Vertices; y++)
        {
            for (int x = 0; x < XCount_Vertices; x++)
            {
                // DETERMINE THE VERTEX INDEX THAT WE SHOULD WRITE TO (IN THE BIG VERTEX ARRAY)
                int currentVertexIndex   = y * XCount_Vertices + x;
               
                // DETERMINE THE TRIANGLE INDEX THAT WE SHOULD WRITE TO (IN THE BIG TRIANGLE ARRAY)
                int currentTriangleIndex = (y * XCount + x) * 6;

                // VERTEX COORDINATES (GRID)
                float vx = x * (XSize / XCount);
                float vy = y * (YSize / YCount);
                
                // OPTIONAL PERLIN NOISE
                float vz = zNoise
                    ? MaxNoiseSize * Mathf.PerlinNoise(noiseXOffset + noiseXScale * vx, noiseYOffset + noiseYScale * vy)
                    : 0;

                // SELECT A PLANE
                if (plane == "XY") 
                    vertices[currentVertexIndex] = new Vector3(vx, vy, vz);
                else if (plane == "XZ")
                    vertices[currentVertexIndex] = new Vector3(vx, vz, vy);
                else
                    vertices[currentVertexIndex] = new Vector3(vz, vx, vy);


                if (StretchTexture)
                {
                    uvs[currentVertexIndex] = new Vector2(x / (float)XCount, y / (float)YCount);
                }
                else
                {
                    uvs[currentVertexIndex] = new Vector2(x * (XSize / XCount), y * (YSize / YCount));
                }

                if (CreateFaces)
                {
                    if (y < YCount && x < XCount)
                    {
                        if (reverse)
                        {
                            triangles[currentTriangleIndex + 0] = currentVertexIndex;
                            triangles[currentTriangleIndex + 1] = currentVertexIndex + 1;
                            triangles[currentTriangleIndex + 2] = currentVertexIndex + XCount_Vertices;

                            triangles[currentTriangleIndex + 3] = currentVertexIndex + 1;
                            triangles[currentTriangleIndex + 4] = currentVertexIndex + 1 + XCount_Vertices;
                            triangles[currentTriangleIndex + 5] = currentVertexIndex + XCount_Vertices;
                        }
                        else
                        {
                            triangles[currentTriangleIndex + 2] = currentVertexIndex;
                            triangles[currentTriangleIndex + 1] = currentVertexIndex + 1;
                            triangles[currentTriangleIndex + 0] = currentVertexIndex + XCount_Vertices;

                            triangles[currentTriangleIndex + 5] = currentVertexIndex + 1;
                            triangles[currentTriangleIndex + 4] = currentVertexIndex + 1 + XCount_Vertices;
                            triangles[currentTriangleIndex + 3] = currentVertexIndex + XCount_Vertices;
                        }
                    }
                }
            }
        }

        GeneratedMesh.SetVertices(vertices);
        GeneratedMesh.SetTriangles(triangles,0);
        GeneratedMesh.RecalculateNormals();
        GeneratedMesh.SetUVs(0,uvs);

        Changed = true;
    }


    
    #endregion
}
