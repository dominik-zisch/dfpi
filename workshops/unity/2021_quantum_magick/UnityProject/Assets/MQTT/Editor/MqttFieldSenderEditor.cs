using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(MqttFieldSender))]
public class MqttFieldSenderEditor : Editor
{
    SerializedProperty _mqtt;
    SerializedProperty _framerate;
    SerializedProperty _topic;
    SerializedProperty _dataSource;
    SerializedProperty _fieldName;
    SerializedProperty _keepSending;
    
    // Component list cache and its parent game object
    string[] _componentList;
    GameObject _cachedGameObject;

    // Property list cache and its parent type
    string[] _fieldList;
    Type _cachedType;
    
    static class Labels
    {
        public static readonly GUIContent Mqtt = new GUIContent("Mqtt Handler");
        public static readonly GUIContent Framerate = new GUIContent("Framerate");
        public static readonly GUIContent Topic = new GUIContent("Topic");
    }
    
    void OnEnable()
    {
        _mqtt = serializedObject.FindProperty("_mqtt");
        _framerate = serializedObject.FindProperty("_framerate");
        _topic = serializedObject.FindProperty("_topic");
        _dataSource = serializedObject.FindProperty("_dataSource");
        _fieldName = serializedObject.FindProperty("_fieldName");
        _keepSending  = serializedObject.FindProperty("_keepSending");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.ObjectField(_mqtt, Labels.Mqtt);
        EditorGUILayout.DelayedIntField(_framerate, Labels.Framerate);
        EditorGUILayout.PropertyField(_topic, Labels.Topic);
        EditorGUILayout.PropertyField(_dataSource);

        if (!_dataSource.hasMultipleDifferentValues &&
            !_fieldName.hasMultipleDifferentValues)
        {
            // Custom drop-down UI
            ShowComponentSelector();
            ShowFieldSelector();
        }
        else
        {
            // Normal text field
            EditorGUILayout.PropertyField(_fieldName);
        }

        EditorGUILayout.PropertyField(_keepSending);

        serializedObject.ApplyModifiedProperties();
    }
    
    void ShowComponentSelector()
    {
        var component = (Component)_dataSource.objectReferenceValue;
        if (component == null) return;

        // Cache the component list in the parent game object.
        CacheComponentList(component.gameObject);

        // Show the component selection drop-down list.
        var index = Array.IndexOf(_componentList, component.GetType().Name);
        var newIndex = EditorGUILayout.Popup("Component", index, _componentList);

        // Update the component if the selection was changed.
        if (index != newIndex)
        {
            component = component.GetComponent(_componentList[newIndex]);
            _dataSource.objectReferenceValue = component;
        }
    }

    void ShowFieldSelector()
    {
        var component = (Component)_dataSource.objectReferenceValue;
        if (component == null) return;

        // Cache the field list of the component.
        CacheFieldList(component.GetType());

        if (_fieldList.Length > 0)
        {
            // Show the field selection drop-down list.
            var index = Array.IndexOf(_fieldList, _fieldName.stringValue);
            var newIndex = EditorGUILayout.Popup("Field", Mathf.Max(0, index), _fieldList);

            // Update the field if the selection was changed.
            if (index != newIndex)
                _fieldName.stringValue = _fieldList[newIndex];
        }
        else
        {
            // No matched field found: Clear the previous selection
            _fieldName.stringValue = "";
        }
    }

    // Hashed set of targetable types
    HashSet<Type> _targetableTypes = new HashSet<Type>(new [] {
        typeof(byte[]),
        typeof(bool),
        typeof(int), 
        typeof(float), 
        typeof(string),
        typeof(Vector2), 
        typeof(Vector3), 
        typeof(Vector4),
        typeof(Vector2Int), 
        typeof(Vector3Int)
    });

    // Cache components attached to the given game object if it's different
    // from the previously given one.
    void CacheComponentList(GameObject gameObject)
    {
        if (_cachedGameObject == gameObject) return;

        _componentList = gameObject.GetComponents<Component>().
            Select(x => x.GetType().Name).ToArray();

        _cachedGameObject = gameObject;
    }

    // Check the type of the given field.
    bool IsTargetable(FieldInfo info)
    {
        return _targetableTypes.Contains(info.FieldType);
    }

    // Cache properties from the given class if it's different from the
    // previously given one.
    void CacheFieldList(Type type)
    {
        if (_cachedType == type) return;

        _fieldList = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(x => IsTargetable(x))
            .Select(x => x.Name).ToArray();

        _cachedType = type;
    }
}
