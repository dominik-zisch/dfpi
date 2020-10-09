using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[ExecuteAlways]
public class JsonParseText : MonoBehaviour
{
    public TextAsset jsonFile;
    public SceneCollection      scenes;
    
    void OnEnable()
    {
        scenes = JsonConvert.DeserializeObject<SceneCollection>(jsonFile.text); 
    }

}

[Serializable]
public class Scene
{
    public string    name;
    public int       index;
    public List<int> neighbors;
}

[Serializable]
public class SceneCollection
{
    public List<Scene> scenes;
}

