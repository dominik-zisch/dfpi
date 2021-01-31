using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2MqttUnity
{
    public class Mqtt : M2MqttUnityClient
    {
        #region Inspector
        
        public MessageReceived OnMessageReceived;                          // Callback for Message received
        
        [Serializable] public class MessageReceived : UnityEvent<MqttBuffer> {}
        
        #endregion
        
        #region Public Variables

        public bool isConnected = false;
        
        #endregion
        
        #region Private Variables

        private List<string> topics;
        
        #endregion
        
        #region MonoBehaviour implementation

        protected override void Awake()
        {
            base.Awake();
            topics = new List<string>();
        }
        
        #endregion

        #region Public Functions

        public void Subscribe(string topic)
        {
            topics.Add(topic);
        }
        
        public void RegisterListener(UnityAction<MqttBuffer> call)
        {
            OnMessageReceived.AddListener(call);
        }

        public void DeregisterListener(UnityAction<MqttBuffer> call)
        {
            OnMessageReceived.RemoveListener(call);
        }

        public void Publish(string topic, byte[] data)
        {
            if (data == null || data.Length == 0) return;
            string msg = (int)MqttDataType.Bytearray + "-" + MqttUtils.ByteArrayToHexString(data);
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, bool data)
        {
            string msg = (int)MqttDataType.Bool + "-" + data;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, int data)
        {
            string msg = (int)MqttDataType.Int + "-" + data;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, float data)
        {
            string msg = (int)MqttDataType.Float + "-" + data;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, string data)
        {
            string msg = (int)MqttDataType.String + "-" + data;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, Vector2Int data)
        {
            string msg = (int)MqttDataType.Vector2Int + "-" + data.x + "," + data.y;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, Vector3Int data)
        {
            string msg = (int)MqttDataType.Vector3Int + "-" + data.x + "," + data.y + "," + data.z;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, Vector2 data)
        {
            string msg = (int)MqttDataType.Vector2 + "-" + data.x + "," + data.y;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, Vector3 data)
        {
            string msg = (int)MqttDataType.Vector3 + "-" + data.x + "," + data.y + "," + data.z;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Publish(string topic, Vector4 data)
        {
            string msg = (int)MqttDataType.Vector4 + "-" + data.x + "," + data.y + "," + data.z + "," + data.w;
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        
        #endregion
        
        #region M2Mqtt Override Functions
        
        protected override void OnConnected()
        {
            base.OnConnected();
            isConnected = true;
        }
        
        protected override void OnConnectionFailed(string errorMessage)
        {
            base.OnConnectionFailed(errorMessage);
            isConnected = true;
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            isConnected = true;
        }

        protected override void OnConnectionLost()
        {
            base.OnConnectionLost();
            isConnected = true;
        }
        
        protected override void SubscribeTopics()
        {
            foreach (var t in topics)
            {
                
                client.Subscribe(new string[] {t}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            }
            
        }

        protected override void UnsubscribeTopics()
        {
            foreach (var t in topics)
            {
                client.Unsubscribe(new string[] {t});
            }
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            var tokens = System.Text.Encoding.UTF8.GetString(message).Split(new[] {'-'}, 2);
            if (tokens.Length != 2) return;
            int res;
            if (int.TryParse(tokens[0], out res))
            {
                MqttBuffer buf = new MqttBuffer(topic, (MqttDataType)res, tokens[1]);
                OnMessageReceived.Invoke(buf);
            }
        }
        
        #endregion
    }
}