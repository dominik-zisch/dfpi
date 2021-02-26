using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(ProceduralGrid))]
[ExecuteAlways]
public class VertexAttraction : MonoBehaviour
{
    #region Public Variables
    public Transform Attractor;
    [Range(0, 100)] public float MinimumDistance;
    [Range(0, 100)] public float MaximumDistance;
    public bool Inverse;
    [Space]

    [Range(0, 1)] public float AttractionStrength;
    public AnimationCurve ResponseCurve;
    [Range(0, 100)] public float MaximumDisplacement;

    [Space]
    public bool DisplaceVerices;
    #endregion

    #region Private variables
    MeshFilter mf;
    ProceduralGrid grid;
    Mesh m;

    List<Vector3> defaultVertices;
    List<Vector3> defaultNormals;

    Vector3[] realtimeVertices;
    Vector3[] realtimeNormals;

    float[] distances;
    float[] normalizedDistances;
    float[] interpolatedDistances;
    float[] remappedDistances;

    int prevVertexCount;
    Bounds prevBounds;

    Vector3 prevAttractorPosition;

    #endregion


    void Start()
    {
        defaultVertices = new List<Vector3>();
        defaultNormals = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {

        // Always maintain a reference to the mesh filter component
        if (mf == null)
        {
            mf = GetComponent<MeshFilter>();
        }

        // Always maintain a reference to the procedural grid component
        if (grid == null)
        {
            grid = GetComponent<ProceduralGrid>();
        }

        // Get the latest mesh
        m = grid.GeneratedMesh;
        realtimeVertices = new Vector3[m.vertexCount];
        realtimeNormals = new Vector3[m.vertexCount];

        // If there was a change in the grid, update everything
        if (prevVertexCount != m.vertexCount || prevBounds != m.bounds)
        {
            defaultVertices.Clear();
            defaultNormals.Clear();

            m.GetVertices(defaultVertices);
            m.GetNormals(defaultNormals);

            distances = new float[m.vertexCount];
            normalizedDistances = new float[m.vertexCount];
            interpolatedDistances = new float[m.vertexCount];
            remappedDistances = new float[m.vertexCount];

            prevVertexCount = m.vertexCount;
            prevBounds = m.bounds;
        }

        // Get the normals and the vertices of the rest state
        defaultNormals.CopyTo(realtimeNormals);
        defaultVertices.CopyTo(realtimeVertices);

        // Calculate distance to the attractor
        for (int i = 0; i < m.vertexCount; i++)
        {

            // Raw distance
            distances[i] = (Attractor.position - transform.TransformPoint(defaultVertices[i])).magnitude;

            //Normalized to 0...1 range
            normalizedDistances[i] = Mathf.InverseLerp(MinimumDistance, MaximumDistance, distances[i]);

            //Interpolate through the curve
            interpolatedDistances[i] = Inverse ? ResponseCurve.Evaluate(1 - normalizedDistances[i]) : ResponseCurve.Evaluate(normalizedDistances[i]);

            // Remap to the final displacement range
            remappedDistances[i] = interpolatedDistances[i] * MaximumDisplacement * AttractionStrength;

            // Update normals and vertices
            realtimeNormals[i] *= remappedDistances[i];
            realtimeVertices[i] += realtimeNormals[i];

        }

        List<Vector3> temp = new List<Vector3>(realtimeVertices);
        // Update the mesh filter
        mf.sharedMesh.SetVertices(temp);
        mf.sharedMesh.RecalculateBounds();
        mf.sharedMesh.RecalculateNormals();

        // Draw the new normals
        /*
        for (int i = 0; i < mf.sharedMesh.vertexCount; i++)
        {
            Debug.DrawRay(transform.TransformPoint(mf.sharedMesh.vertices[i]), mf.sharedMesh.normals[i]*0.1f, Color.red);
        }
        */

    }

}
