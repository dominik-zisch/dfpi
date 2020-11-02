using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GraphObject))]
    public class GraphObjectEditor : UnityEditor.Editor
    {
        private GraphObject go;
        private bool        foldNodes;
        private bool        foldEdges;

        private void OnEnable()
        {
            go = (GraphObject) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var style = new GUIStyle(EditorStyles.foldoutHeader)
                    {fixedWidth = EditorStyles.foldoutHeader.CalcSize(new GUIContent("Nodes")).x};
          
            foldNodes = EditorGUILayout.BeginFoldoutHeaderGroup(foldNodes, "Nodes", style);
          
            if (foldNodes)
            {
                var nodes = go.graph.nodes;

                foreach (var node in nodes)
                {
                    EditorGUILayout.IntField(node.Index, new GUIStyle(GUI.skin.box){fixedWidth = 32, alignment = TextAnchor.MiddleCenter});
                }  

                
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            foldEdges = EditorGUILayout.BeginFoldoutHeaderGroup(foldEdges, "Edges", style);
            if (foldEdges)
            {

                var edges = go.graph.edges;

                foreach (var edge in edges)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(edge.From.Index.ToString(), new GUIStyle(GUI.skin.box){fixedWidth = 32, alignment = TextAnchor.MiddleCenter}, GUILayout.ExpandWidth(false));
                    EditorGUILayout.LabelField("->",  new GUIStyle(GUI.skin.box){fixedWidth = 32, alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState(){background = Texture2D.blackTexture}},
                                               GUILayout.ExpandWidth(false));
                    EditorGUILayout.LabelField(edge.To.Index.ToString(), new GUIStyle(GUI.skin.box){fixedWidth = 32, alignment = TextAnchor.MiddleCenter}, GUILayout.ExpandWidth(false));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
           
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}