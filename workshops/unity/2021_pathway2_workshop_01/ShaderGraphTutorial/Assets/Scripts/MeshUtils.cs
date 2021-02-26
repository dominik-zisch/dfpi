using UnityEngine;

namespace DefaultNamespace
{
    public static class MeshUtils
    {

        public static void AllocateNewMesh(ref Mesh m)
        {
            if (m != null)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(m);
                }
                else
                {
                    GameObject.DestroyImmediate(m);
                }
            }

            m = new Mesh();
        }

        public static void CopyMeshSlow(Mesh from, Mesh to)
        {
            if (from == null || to == null)
                return;
        
            // Reset Mesh Memory
            to.Clear();
        
            to.SetVertices(from.vertices);
            to.SetTriangles(from.triangles, 0);
            to.SetUVs(0, from.uv);
            to.SetNormals(from.normals);
            to.name = from.name;

        }
    }
}