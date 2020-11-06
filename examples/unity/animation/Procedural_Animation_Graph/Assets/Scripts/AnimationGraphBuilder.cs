using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class AnimationGraphBuilder : MonoBehaviour
{
    [FormerlySerializedAs("stateMachine")]
    public AnimatorController controller;
    
    private List<AnimatorStateMachine>    states;
    private List<AnimatorStateTransition> StateTransitions;
    
    public  string               SceneParentName = "Scenes";

    public AnimationCurve inCurve;
    public AnimationCurve outCurve;

    public List<GameObject> prefabs;
    
    private AnimatorStateMachine Root;
    private GameObject           Parent;
    private List<GameObject>     Scenes;
    private GameObject           toDestroy;

    private Type   animatedScriptType;
    private string propertyName;


    private void OnEnable()
    { }

    private void Update()
    {
        if (Application.isPlaying) 
            return;
        
        if (toDestroy == null)
            return;
        
        DestroyImmediate(toDestroy);
       
        toDestroy = null;
    }

    
    public void RebuildAnimationGraph(Graph graph)
    {
        InitializeSelf();

        InitializeController();

        GenerateSceneGameObjects(graph);

        StatesFromGraphNodes(graph);

        TransitionsFromGraphEdges(graph);

        Finalize();
    }
    
    
    
    
    private void InitializeSelf()
    { 
        // Creates the controller
        controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/StateMachineTransitions.controller");

        // Initialize lists
        states           = new List<AnimatorStateMachine>();
        StateTransitions = new List<AnimatorStateTransition>();
        Scenes           = new List<GameObject>();

        // Mark existing child for destruction
        if (transform.childCount > 0)
            toDestroy = transform.GetChild(0).gameObject;
        
        // New Parent
        Parent    = new GameObject(SceneParentName);
        Parent.transform.SetParent(transform);
        
        // Set default animation clip properties
        animatedScriptType = typeof(GenericAnimatedProperty);
        propertyName       = "value";
    }

    
    private void InitializeController()
    {
        // Remove All existing layers
        while (controller.layers.Length >0)
        {
            controller.RemoveLayer(0);
        }
        
        // Remove All existing Triggers
        while (controller.parameters.Length > 0)
        {
            controller.RemoveParameter(0);
        }
        
        // Add Default Layer
        controller.AddLayer("Layer 01");

        // Get root state machine
        Root = controller.layers[0].stateMachine;
       
        // Arrange default states
        Root.entryPosition = Vector3.zero;
        Root.anyStatePosition = new Vector3(600,600,0);
    }

    
    private void GenerateSceneGameObjects(Graph graph)
    {
        int i = 0;

        foreach (var node in graph.nodes)
        {
            GameObject go = null;

            if (prefabs == null || prefabs.Count-1 < i)
            {
                go = new GameObject(node.Index.ToString(), animatedScriptType, typeof(MeshRenderer),
                                    typeof(MeshFilter));
            }
            else
            {
                // Generate a scene using a prefab
                go      = GameObject.Instantiate(prefabs[i]);
                go.name = node.Index.ToString();
                
                // check if the scene has the animatedScriptType component already
                if (go.TryGetComponent(animatedScriptType, out _) == false)
                {
                    go.AddComponent(animatedScriptType);
                }
            }
         
            go.transform.SetParent(Parent.transform);
            Scenes.Add(go);

            i++;
        }
    }

    
    private void StatesFromGraphNodes(Graph graph)
    {
        foreach (var node in graph.nodes)
        {
            var sm = AddNode(node, CircularNodePosition(graph, node));
            states.Add(sm);
        }
    }

    
    private void TransitionsFromGraphEdges(Graph graph)
    {
        foreach (var edge in graph.edges)
        {
            StateTransitions.Add(AddEdge(edge));
        }  
    }

    
    private void Finalize()
    {
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        GetComponent<Animator>().runtimeAnimatorController = controller;
    }
    
    
    
    
    private AnimatorStateMachine AddNode(Node node, Vector3 position = default)
    {
        // Create a new sub-state machine to hold this node's states
        var subState = Root.AddStateMachine( $"Node {node.Index}", position);
         
        // Place default states at the top
        subState.anyStatePosition = new Vector3(240, -240, 0);
        subState.entryPosition    = new Vector3(20,  -120, 0);
        
        // Add entry state
        var inState = subState.AddState("In", new Vector3(0, 0, 0));
        inState.motion = AddIncomingClip(5, node, inCurve);
       
        // Add idle state
        var idleState = subState.AddState("Idle", new Vector3(0, 120, 0));
        idleState.motion = AddIdleClip(5, node);
        
        // Connect "in" to "idle"
        var inToIdleTransition = inState.AddTransition(idleState);
        
        // Should be automatically triggered
        inToIdleTransition.exitTime    = 1;
        inToIdleTransition.hasExitTime = true;
        
        // Place default states at the bottom
        subState.parentStateMachinePosition = new Vector3(0,  360, 0);
        subState.exitPosition               = new Vector3(20, 480, 0);
        
        return subState;
    }

    private AnimatorStateTransition AddEdge(Edge edge)
    {
        // Get node indices
        var    from = edge.From.Index;
        var    to   = edge.To.Index;
       
        // Get edge name
        var name    = edge.ToString();

        var currentState = states[from];
                        
        // Generate a new Trigger based on the edge's name
        controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

        // Generate a new Out State
        var xPosition = (currentState.states.Length -2) * 300;
        var outState  = currentState.AddState($"To Node {to}", new Vector3(xPosition, 240, 0));
        outState.motion = AddOutgoingClip(5, edge.From, outCurve);
        
        // Generate a new transition from the idle state to the out state
        var idleToOutTransition = currentState.states[1].state.AddTransition(outState);
        
        // Assign the trigger to the transition
        idleToOutTransition.AddCondition(AnimatorConditionMode.If, 0, name );

        // Create the exit transition to the next node
        var outToNext = outState.AddTransition(states[to]);
        outToNext.name        = name;
        outToNext.hasExitTime = true;
        outToNext.exitTime    = 1;
        
        return outToNext;
    }
    
    

    private AnimationClip AddOutgoingClip(float duration, Node node, AnimationCurve crv = null)
    {
        return AddClip(duration, 1, 0, animatedScriptType, propertyName, $"{SceneParentName}/{node.Index}", $"Exiting Node {node.Index}", crv);
    }
    
    private AnimationClip AddIdleClip(float duration, Node node, AnimationCurve crv = null)
    {
        return AddClip(duration, 1, 1, animatedScriptType, propertyName, $"{SceneParentName}/{node.Index}", $"Staying at Node {node.Index}", crv);
    }
    
    private AnimationClip AddIncomingClip(float duration, Node node, AnimationCurve crv= null)
    {
        return AddClip(duration, 0, 1, animatedScriptType, propertyName, $"{SceneParentName}/{node.Index}", $"Entering Node {node.Index}", crv);
    }
    
    
    private AnimationClip AddClip(float duration, float valueAtStart, float valueAtEnd, Type type, string propertyName, string objectPath, string clipName, AnimationCurve crv = null)
    {
        // New empty clip
        var clip = new AnimationClip();
        
        // New animation curve
        if (crv == null) 
            crv = AnimationCurve.EaseInOut(0, valueAtStart, duration, valueAtEnd);
       
        // ===============================================
        // IMPORTANT PART ! ==============================
        // Apply curve to clip
        clip.SetCurve(objectPath, type, propertyName, crv);
        // ===============================================

        // (optional) set clip's name
        clip.name = clipName;
        
        // Save clip inside AnimatorController
        AssetDatabase.AddObjectToAsset(clip, controller);
       // clip.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(clip));
      
        return clip;
    }

    
    
    private static Vector3 CircularNodePosition(Graph graph, Node node, float radius = 400)
    {
        var t = (node.Index / (float) graph.nodes.Count);
       
        var p = Mathf.Lerp(0, Mathf.PI * 2, t);
       
        return new Vector3(Mathf.Sin(p), Mathf.Cos(p), 0) * radius;
        
    }


}
