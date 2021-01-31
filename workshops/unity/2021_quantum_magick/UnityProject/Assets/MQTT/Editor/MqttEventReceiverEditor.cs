using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(MqttEventReceiver))]
public class MqttEventReceiverEditor : Editor
{
    SerializedProperty _mqtt;
    SerializedProperty _topic;
    SerializedProperty _dataType;
    
    SerializedProperty _bytesReceived;
    SerializedProperty _boolReceived;
    SerializedProperty _intReceived;
    SerializedProperty _floatReceived;
    SerializedProperty _stringReceived;
    SerializedProperty _vector2IntReceived;
    SerializedProperty _vector3IntReceived;
    SerializedProperty _vector2Received;
    SerializedProperty _vector3Received;
    SerializedProperty _vector4Received;

    static class Labels
    {
        public static readonly GUIContent Mqtt = new GUIContent("Mqtt Handler");
        public static readonly GUIContent Topic = new GUIContent("Topic");
    }

    void OnEnable()
    {
        _mqtt = serializedObject.FindProperty("_mqtt");
        _topic = serializedObject.FindProperty("_topic");
        _dataType = serializedObject.FindProperty("_dataType");
        
        _bytesReceived = serializedObject.FindProperty("_bytesReceived");
        _boolReceived = serializedObject.FindProperty("_boolReceived");
        _intReceived = serializedObject.FindProperty("_intReceived");
        _floatReceived = serializedObject.FindProperty("_floatReceived");
        _stringReceived = serializedObject.FindProperty("_stringReceived");
        _vector2IntReceived = serializedObject.FindProperty("_vector2IntReceived");
        _vector3IntReceived = serializedObject.FindProperty("_vector3IntReceived");
        _vector2Received = serializedObject.FindProperty("_vector2Received");
        _vector3Received = serializedObject.FindProperty("_vector3Received");
        _vector4Received = serializedObject.FindProperty("_vector4Received");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.ObjectField(_mqtt, Labels.Mqtt);
        EditorGUILayout.PropertyField(_topic, Labels.Topic);
        EditorGUILayout.PropertyField(_dataType);

        if (!_dataType.hasMultipleDifferentValues)
        {
            switch ((MqttDataType)_dataType.enumValueIndex+1)
            {
                case MqttDataType.Bytearray:
                    EditorGUILayout.PropertyField(_bytesReceived);
                    break;
                case MqttDataType.Bool:
                    EditorGUILayout.PropertyField(_boolReceived);
                    break;
                case MqttDataType.Int:
                    EditorGUILayout.PropertyField(_intReceived);
                    break;
                case MqttDataType.Float:
                    EditorGUILayout.PropertyField(_floatReceived);
                    break;
                case MqttDataType.String:
                    EditorGUILayout.PropertyField(_stringReceived);
                    break;
                case MqttDataType.Vector2Int:
                    EditorGUILayout.PropertyField(_vector2IntReceived);
                    break;
                case MqttDataType.Vector3Int:
                    EditorGUILayout.PropertyField(_vector3IntReceived);
                    break;
                case MqttDataType.Vector2:
                    EditorGUILayout.PropertyField(_vector2Received);
                    break;
                case MqttDataType.Vector3:
                    EditorGUILayout.PropertyField(_vector3Received);
                    break;
                case MqttDataType.Vector4:
                    EditorGUILayout.PropertyField(_vector4Received);
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
