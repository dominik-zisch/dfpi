using System;
using System.Collections;
using System.Collections.Generic;
using M2MqttUnity;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("MQTT/Event Receiver")]
public class MqttEventReceiver : MonoBehaviour
{

    #region Receiver event classes
    
    [System.Serializable] class BytesReceived : UnityEvent<byte[]> {}
    [System.Serializable] class BoolReceived : UnityEvent<bool> {}
    [System.Serializable] class IntReceived : UnityEvent<int> {}
    [System.Serializable] class FloatReceived : UnityEvent<float> {}
    [System.Serializable] class StringReceived : UnityEvent<string> {}
    [System.Serializable] class Vector2IntReceived : UnityEvent<Vector2Int> {}
    [System.Serializable] class Vector3IntReceived : UnityEvent<Vector3Int> {}
    [System.Serializable] class Vector2Received : UnityEvent<Vector2> {}
    [System.Serializable] class Vector3Received : UnityEvent<Vector3> {}
    [System.Serializable] class Vector4Received : UnityEvent<Vector4> {}
    
    #endregion
    
    #region Inspector
    
    [SerializeField] Mqtt _mqtt;
    [SerializeField] string _topic = "mqtt/topic";
    [SerializeField] MqttDataType _dataType = MqttDataType.Bytearray;

    [SerializeField] BytesReceived _bytesReceived = null;
    [SerializeField] BoolReceived _boolReceived = null;
    [SerializeField] IntReceived _intReceived = null;
    [SerializeField] FloatReceived _floatReceived = null;
    [SerializeField] StringReceived _stringReceived = null;
    [SerializeField] Vector2IntReceived _vector2IntReceived = null;
    [SerializeField] Vector3IntReceived _vector3IntReceived = null;
    [SerializeField] Vector2Received _vector2Received = null;
    [SerializeField] Vector3Received _vector3Received = null;
    [SerializeField] Vector4Received _vector4Received = null;
    
    #endregion
    
    #region MonoBehaviour implementation
    
    // Start is called before the first frame update
    void Start()
    {
        if (_mqtt == null)
            _mqtt = GameObject.Find("MQTT Client").GetComponent<Mqtt>();
        
        _mqtt.RegisterListener(OnDataReceive);
        _mqtt.Subscribe(_topic);
    }

    // Update is called once per frame
    void Update()
    {
        if (_mqtt == null || !_mqtt.isConnected) return;
    }
    
    #endregion
    
    #region Mqtt event callback

    void OnDataReceive(MqttBuffer data)
    {
        if (_topic != data.topic) return;
        if (_dataType != data.dataType) return;
        
        if (_dataType == MqttDataType.Bytearray)
        {
            byte[] ba = MqttUtils.HexStringToByteArray(data.msg);
            _bytesReceived.Invoke(ba);
        }
        else if (_dataType == MqttDataType.Bool)
        {
            bool b;
            if (bool.TryParse(data.msg, out b))
            {
                _boolReceived.Invoke(b);
            }
        }
        else if (_dataType == MqttDataType.Int)
        {
            int i;
            if (int.TryParse(data.msg, out i))
            {
                _intReceived.Invoke(i);
            }
        }
        else if (_dataType == MqttDataType.Float)
        {
            float f;
            if (float.TryParse(data.msg, out f))
            {
                _floatReceived.Invoke(f);
            }
        }
        else if (_dataType == MqttDataType.String)
        {
            _stringReceived.Invoke(data.msg);
        }
        else if (_dataType == MqttDataType.Vector2Int)
        {
            var tokens = data.msg.Split(',');
            if (tokens.Length != 2) return;
            int i1, i2;
            if (int.TryParse(tokens[0], out i1) && 
                int.TryParse(tokens[1], out i2))
            {
                _vector2IntReceived.Invoke(new Vector2Int(i1, i2));
            }
        }
        else if (_dataType == MqttDataType.Vector3Int)
        {
            var tokens = data.msg.Split(',');
            if (tokens.Length != 3) return;
            int i1, i2, i3;
            if (int.TryParse(tokens[0], out i1) && 
                int.TryParse(tokens[1], out i2) && 
                int.TryParse(tokens[2], out i3))
            {
                _vector3IntReceived.Invoke(new Vector3Int(i1, i2, i3));
            }
        }
        else if (_dataType == MqttDataType.Vector2)
        {
            var tokens = data.msg.Split(',');
            if (tokens.Length != 2) return;
            float f1, f2;
            if (float.TryParse(tokens[0], out f1) && 
                float.TryParse(tokens[1], out f2))
            {
                _vector2Received.Invoke(new Vector2(f1, f2));
            }
        }
        else if (_dataType == MqttDataType.Vector3)
        {
            var tokens = data.msg.Split(',');
            if (tokens.Length != 3) return;
            float f1, f2, f3;
            if (float.TryParse(tokens[0], out f1) && 
                float.TryParse(tokens[1], out f2) && 
                float.TryParse(tokens[2], out f3))
            {
                _vector3Received.Invoke(new Vector3(f1, f2, f3));
            }
        }
        else if (_dataType == MqttDataType.Vector4)
        {
            var tokens = data.msg.Split(',');
            if (tokens.Length != 4) return;
            float f1, f2, f3, f4;
            if (float.TryParse(tokens[0], out f1) && 
                float.TryParse(tokens[1], out f2) && 
                float.TryParse(tokens[2], out f3) && 
                float.TryParse(tokens[3], out f4))
            {
                _vector4Received.Invoke(new Vector4(f1, f2, f3, f4));
            }
        }
    }
    
    #endregion
}
