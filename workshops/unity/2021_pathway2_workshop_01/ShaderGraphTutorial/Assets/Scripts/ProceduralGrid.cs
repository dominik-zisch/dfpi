using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteAlways]

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

    [HideInInspector] public Mesh GeneratedMesh;

    #endregion

    #region Private Variables
    
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;
    private bool Changed = false;
    private MeshFilter mf;
    private MeshRenderer mr;
    private List<string> PlaneValues { get { return new List<string>() { "XY", "XZ", "YZ" }; } }

    #endregion

    #region MonoBehavior methods
    
    private void Awake()
    {
        Generate();
    }

    private void OnValidate()
    {
        Generate();
    }

    private void Update()
    {
        if (mf == null) mf = GetComponent<MeshFilter>();
        if (mr == null) mr = GetComponent<MeshRenderer>();

        if (Changed)
        {
            Debug.Log("FUCK YOU");
            mf.sharedMesh = new Mesh();
            mf.sharedMesh.vertices = GeneratedMesh.vertices;
            mf.sharedMesh.triangles = GeneratedMesh.triangles;
            mf.sharedMesh.uv = GeneratedMesh.uv;
            mf.sharedMesh.normals = GeneratedMesh.normals;
            mr.material = Resources.Load("Material/Standard", typeof(Material)) as Material;
            Changed = false;
        }

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

    #endregion


    #region Custom methods
    public void Generate()
    { 
        GeneratedMesh = new Mesh
        {
            name = string.Format("Procedural Grid {0}x{1}", XCount, YCount)
        };

        int XCount_Vertices = XCount + 1;
        int YCount_Vertices = YCount + 1;

        vertices = new Vector3[(XCount_Vertices) * (YCount_Vertices)];
        uvs = new Vector2[vertices.Length];
        triangles = new int[XCount * YCount * 6];

        for (int y = 0; y < YCount_Vertices; y++)
        {
            for (int x = 0; x < XCount_Vertices; x++)
            {
                int vi = y * XCount_Vertices + x;
                int ti = (y * XCount + x) * 6;

                float vx = x * (XSize / XCount);
                float vy = y * (YSize / YCount);
                float vz = zNoise
                    ? MaxNoiseSize * Mathf.PerlinNoise(noiseXOffset + noiseXScale * vx, noiseYOffset + noiseYScale * vy)
                    : 0;

                if (plane == "XY") 
                    vertices[vi] = new Vector3(vx, vy, vz);
                else if (plane == "XZ")
                    vertices[vi] = new Vector3(vx, vz, vy);
                else
                    vertices[vi] = new Vector3(vz, vx, vy);


                if (StretchTexture)
                {
                    uvs[vi] = new Vector2((float)x / (float)XCount, (float)y / (float)YCount);
                }
                else
                {
                    uvs[vi] = new Vector2(x * (XSize / XCount), y * (YSize / YCount));
                }

                if (CreateFaces)
                {
                    if (y < YCount && x < XCount)
                    {
                        if (reverse)
                        {
                            triangles[ti + 0] = vi;
                            triangles[ti + 1] = vi + 1;
                            triangles[ti + 2] = vi + XCount_Vertices;

                            triangles[ti + 3] = vi + 1;
                            triangles[ti + 4] = vi + 1 + XCount_Vertices;
                            triangles[ti + 5] = vi + XCount_Vertices;
                        }
                        else
                        {
                            triangles[ti + 2] = vi;
                            triangles[ti + 1] = vi + 1;
                            triangles[ti + 0] = vi + XCount_Vertices;

                            triangles[ti + 5] = vi + 1;
                            triangles[ti + 4] = vi + 1 + XCount_Vertices;
                            triangles[ti + 3] = vi + XCount_Vertices;
                        }
                    }
                }
            }
        }

        GeneratedMesh.vertices = vertices;
        GeneratedMesh.triangles = triangles;
        GeneratedMesh.RecalculateNormals();
        GeneratedMesh.uv = uvs;

        Changed = true;
    }

    #endregion
}
