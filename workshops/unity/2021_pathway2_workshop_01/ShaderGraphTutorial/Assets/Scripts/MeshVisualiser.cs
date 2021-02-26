using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class MeshVisualiser : MonoBehaviour
{
    #region Public Variables
    
    [BoxGroup("Vertices")] public bool showVertices = false;
    [BoxGroup("Vertices")] [ShowIf("showVertices")] public bool showVertexNumbers = false;
    [BoxGroup("Vertices")] [ShowIf("showVertices")] public bool useVertexColors = false;
    [FormerlySerializedAs("indicesColor")] [BoxGroup("Vertices")] [ShowIf(EConditionOperator.And, "showVertices", "notUsingVertexColors")] public Color vertexColor = Color.red;
    [FormerlySerializedAs("indicesSize")] [BoxGroup("Vertices")] [ShowIf("showVertices")] [Range(0, 0.1f)] public float vertexSize = 0.02f;
    
    [Space]
    
    [BoxGroup("Triangles")] public bool showTriangles = false;
    [BoxGroup("Triangles")] [ShowIf("showTriangles")] public bool showTriangleIcons = false;
    [BoxGroup("Triangles")] [ShowIf("showTriangles")] public bool useTriangleVertexColors = false;
    [BoxGroup("Triangles")] [ShowIf(EConditionOperator.And, "showTriangles", "showTriangleIcons")] [MinValue(0)] public int selectedTriangleIcon = 0;
    [BoxGroup("Triangles")] [ShowIf(EConditionOperator.And, "showTriangles", "notUsingTriangleVertexColors")] public Color triangleColor = Color.green;
    
    [Space]
    
    [BoxGroup("Normals")] public bool showNormals = false;
    [BoxGroup("Normals")] [ShowIf("showNormals")] public Color normalsColor = Color.blue;
    [BoxGroup("Normals")] [ShowIf("showNormals")] [Range(0, 0.5f)] public float normalsSize = 0.1f;
    
    [Space]
    
    [BoxGroup("UVs")] public bool showUvMap = false;
    
    [Space]
    
    [BoxGroup("Mesh")] public bool showMesh = true;

    [Space]
    
    [BoxGroup("Mesh Values")] public Vector3[] vertices;
    [BoxGroup("Mesh Values")] public int[] triangles;
    [BoxGroup("Mesh Values")] public Vector3[] normals;
    [BoxGroup("Mesh Values")] public Vector2[] uv;
    [BoxGroup("Mesh Values")] public Color[] colors;

    #endregion
    
    #region Private Variables

    private MeshRenderer mr;
    private Mesh mesh;
    private Material orgMat;
    private bool showingUvMap = false;
    private bool showingMesh = true;
    private bool changed = false;
    private int[] verticesIndex;
    private bool notUsingVertexColors => !useVertexColors;
    private bool notUsingTriangleVertexColors => !useTriangleVertexColors;

    #endregion

    #region MonoBehavior Methods
    
    void Awake()
    {
        GetMeshData();
        showingMesh = mr.enabled;
    }
    
    private void OnValidate()
    {
        GetMeshData();
    }

    void LateUpdate()
    {
        GetMeshData();

        if (!showingUvMap && showUvMap)
        {
            orgMat = mr.materials[0];
            mr.material = Resources.Load("Material/uvMaterial", typeof(Material)) as Material;
            showingUvMap = showUvMap;
        } 
        if (showingUvMap && !showUvMap)
        {
            mr.material = orgMat;
            showingUvMap = showUvMap;
        } 
        
        if (!showingMesh && showMesh)
        {
            mr.enabled = true;
            showingMesh = showMesh;
        } 
        if (showingMesh && !showMesh)
        {
            mr.enabled = false;
            showingMesh = showMesh;
        } 
    }

    
    void OnDrawGizmos()
    {
        if (showVertices)
        {
            Gizmos.color = vertexColor;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (useVertexColors && i < colors.Length)
                    Gizmos.color = new Color(colors[i].r, colors[i].g, colors[i].b, 1);
                DrawVertex(i);
                if (showVertexNumbers)
                    DrawVertexIndex(i);
            }
        }
        
        if (showNormals)
        {
            Gizmos.color = normalsColor;
            for (int i = 0; i < normals.Length; i++)
            {
                DrawNormal(transform.TransformPoint(vertices[i]), normals[i]);
            }
        }
        
        if (showTriangles)
        {
            Gizmos.color = triangleColor;
            if (showTriangleIcons)
            {
                selectedTriangleIcon = Mathf.Clamp(selectedTriangleIcon, 0, triangles.Length / 3 - 1);
                int i = selectedTriangleIcon * 3;
                DrawVertexIndex(triangles[i]);
                DrawVertexIndex(triangles[i + 1]);
                DrawVertexIndex(triangles[i + 2]);
                DrawTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
            }
            else
            {
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    DrawTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
                }
            }
        }
    }
    
    #endregion

    #region Custom methods

    
    void GetMeshData()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null) return;
        
        mesh = meshFilter.sharedMesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
        triangles = mesh.triangles;
        normals = mesh.normals;
        uv = mesh.uv;

        verticesIndex = new int[vertices.Length];
        Dictionary<Vector3, int> dict = new Dictionary<Vector3, int>();
        for (int i = 0; i < vertices.Length; i++)
        {
            if (dict.ContainsKey(vertices[i]))
                dict[vertices[i]]++;
            else
                dict[vertices[i]] = 0;
            verticesIndex[i] = dict[vertices[i]];
        }

        changed = true;
    }

    void DrawVertex(int index)
    {
        Gizmos.DrawSphere(transform.TransformPoint(vertices[index]), vertexSize);
    }

    void DrawVertexIndex(int index)
    {
        Vector3 pos = transform.TransformPoint(vertices[index]);
        var digits = index.ToString().ToCharArray();
        Vector3 p = pos + 0.1f * SceneView.lastActiveSceneView.camera.transform.right +
                    new Vector3(0, verticesIndex[index] * 0.1f, 0);
        for (int i = 0; i < digits.Length; i++)
        {
            Gizmos.DrawIcon(p + 0.07f * i * SceneView.lastActiveSceneView.camera.transform.right,
                string.Format("Number_{0}.tif", digits[i]),
                true);
        }
    }
    
    void DrawTriangle(int i0, int i1, int i2)
    {
        DrawLine(i0, i1);
        DrawLine(i1, i2);
        DrawLine(i2, i0);
    }

    void DrawLine(int i0, int i1)
    {
        if (useTriangleVertexColors && i0 < colors.Length)
            Gizmos.color = new Color(colors[i0].r, colors[i0].g, colors[i0].b, 1);
        Gizmos.DrawLine(transform.TransformPoint(vertices[i0]), transform.TransformPoint(vertices[i1]));
    }

    private void DrawNormal(Vector3 pos, Vector3 n)
    {
        Vector3 start = pos;
        Vector3 end = pos + n.normalized * normalsSize;
        Gizmos.DrawLine(start, end);
    }
    
    #endregion
}
