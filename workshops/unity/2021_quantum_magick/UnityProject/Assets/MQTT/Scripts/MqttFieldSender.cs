using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using M2MqttUnity;
using UnityEngine;

[AddComponentMenu("MQTT/Field Sender")]
public class MqttFieldSender : MonoBehaviour
{
    #region Inspector
        
    [SerializeField] Mqtt _mqtt;
    [SerializeField] int _framerate = 60;
    [SerializeField] string _topic = "mqtt/topic";
    [SerializeField] Component _dataSource = null;
    [SerializeField] string _fieldName = "";
    [SerializeField] bool _keepSending = false;
    
    #endregion
        
    #region Public Variables
        
    #endregion
        
    #region Private Variables
    
    FieldInfo _fieldInfo;
    private float _frameDuration;
    private float _frameTimer;

    private byte[] _bytesValue;
    private bool _boolValue = false;
    private int _intValue = Int32.MaxValue;
    private float _floatValue = Single.MaxValue;
    private string _stringValue = string.Empty;
    private Vector2Int _vector2IntValue = new Vector2Int(Int32.MaxValue, 0);
    private Vector3Int _vector3IntValue = new Vector3Int(Int32.MaxValue, 0, 0);
    private Vector2 _vector2Value = new Vector2(Single.MaxValue, 0);
    private Vector3 _vector3Value = new Vector3(Single.MaxValue, 0, 0);
    private Vector4 _vector4Value = new Vector4(Single.MaxValue, 0, 0, 0);
    
    #endregion
        
    #region Private Functions
    
    void UpdateSettings()
    {
        if (_dataSource != null && !string.IsNullOrEmpty(_fieldName))
            _fieldInfo = _dataSource.GetType().GetField(_fieldName);
        else
            _fieldInfo = null;

        _frameDuration = 1f / _framerate;
    }

    #endregion
    
    #region MonoBehaviour implementation
    
    void Start()
    {
        if (_mqtt == null)
            _mqtt = GameObject.Find("MQTT Client").GetComponent<Mqtt>();
        
        UpdateSettings();
    }

    void OnValidate()
    {
        if (Application.isPlaying) UpdateSettings();
    }

    void Update()
    {
        if (_fieldInfo == null || !_mqtt.isConnected) return;

        _frameTimer -= Time.deltaTime;
        if (_frameTimer > 0) return;
        _frameTimer = _frameDuration;

        var type = _fieldInfo.FieldType;
        var value = _fieldInfo.GetValue(_dataSource); // boxing!!

        if (type == typeof(byte[]))
        {
            byte[] data = (byte[]) value;
            if (!_keepSending && data == _bytesValue) return;
            _mqtt.Publish(_topic, data);
            _bytesValue = data;
        }
        else if (type == typeof(bool))
        {
            bool data = (bool) value;
            if (!_keepSending && data == _boolValue) return;
            _mqtt.Publish(_topic, data);
            _boolValue = data;
        }
        else if (type == typeof(int))
        {
            int data = (int) value;
            if (!_keepSending && data == _intValue) return;
            _mqtt.Publish(_topic, data);
            _intValue = data;
        }
        else if (type == typeof(float))
        {
            float data = (float) value;
            if (!_keepSending && data == _floatValue) return;
            _mqtt.Publish(_topic, data);
            _floatValue = data;
        }
        else if (type == typeof(string))
        {
            string data = (string) value;
            if (!_keepSending && data == _stringValue) return;
            _mqtt.Publish(_topic, data);
            _stringValue = data;
        }
        else if (type == typeof(Vector2Int))
        {
            Vector2Int data = (Vector2Int) value;
            if (!_keepSending && data == _vector2IntValue) return;
            _mqtt.Publish(_topic, data);
            _vector2IntValue = data;
        }
        else if (type == typeof(Vector3Int))
        {
            Vector3Int data = (Vector3Int) value;
            if (!_keepSending && data == _vector3IntValue) return;
            _mqtt.Publish(_topic, data);
            _vector3IntValue = data;
        }
        else if (type == typeof(Vector2))
        {
            Vector2 data = (Vector2) value;
            if (!_keepSending && data == _vector2Value) return;
            _mqtt.Publish(_topic, data);
            _vector2Value = data;
        }
        else if (type == typeof(Vector3))
        {
            Vector3 data = (Vector3) value;
            if (!_keepSending && data == _vector3Value) return;
            _mqtt.Publish(_topic, data);
            _vector3Value = data;
        }
        else if (type == typeof(Vector4))
        {
            Vector4 data = (Vector4) value;
            if (!_keepSending && data == _vector4Value) return;
            _mqtt.Publish(_topic, data);
            _vector4Value = data;
        }
    }
    
    #endregion
}
