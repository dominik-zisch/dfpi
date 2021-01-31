using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MqttBuffer
{
    public string topic;
    public MqttDataType dataType;
    public string msg;

    public MqttBuffer(string _topic, MqttDataType _dataType, string _msg)
    {
        topic = _topic;
        dataType = _dataType;
        msg = _msg;
    }
}
