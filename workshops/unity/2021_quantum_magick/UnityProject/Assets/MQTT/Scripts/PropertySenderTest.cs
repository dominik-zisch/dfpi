using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertySenderTest : MonoBehaviour
{
    [Header("Receivers")]
    public byte[] baRecv;
    public bool bRecv;
    public int iRecv;
    public float fRecv;
    public string sRecv;
    public Vector2Int v2iRecv;
    public Vector3Int v3iRecv;
    public Vector2 v2Recv;
    public Vector3 v3Recv;
    public Vector4 v4Recv;
    
    [Header("Field Senders")]
    public byte[] baSend;
    public bool bSend;
    public int iSend;
    public float fSend;
    public string sSend;
    public Vector2Int v2iSend;
    public Vector3Int v3iSend;
    public Vector2 v2Send;
    public Vector3 v3Send;
    public Vector4 v4Send;
    
    [Header("Property Senders")]
    [SerializeField] private byte[] _ba;
    [SerializeField] private bool _b;
    [SerializeField] private int _i;
    [SerializeField] private float _f;
    [SerializeField] private string _s;
    [SerializeField] private Vector2Int _v2i;
    [SerializeField] private Vector3Int _v3i;
    [SerializeField] private Vector2 _v2;
    [SerializeField] private Vector3 _v3;
    [SerializeField] private Vector4 _v4;

    public byte[] BA
    {
        get { return _ba; }
        set { _ba = value; }
    }

    public bool B
    {
        get { return _b; }
        set { _b = value; }
    }

    public int I
    {
        get { return _i; }
        set { _i = value; }
    }

    public float F
    {
        get { return _f; }
        set { _f = value; }
    }

    public string S
    {
        get { return _s; }
        set { _s = value; }
    }

    public Vector2Int V2I
    {
        get { return _v2i; }
        set { _v2i = value; }
    }

    public Vector3Int V3I
    {
        get { return _v3i; }
        set { _v3i = value; }
    }

    public Vector2 V2
    {
        get { return _v2; }
        set { _v2 = value; }
    }

    public Vector3 V3
    {
        get { return _v3; }
        set { _v3 = value; }
    }

    public Vector4 V4
    {
        get { return _v4; }
        set { _v4 = value; }
    }

    public void BytesReceived(byte[] ba)
    {
        baRecv = ba;
    }

    public void BoolReceived(bool b)
    {
        bRecv = b;
    }

    public void IntReceived(int i)
    {
        iRecv = i;
    }

    public void FloatReceived(float f)
    {
        fRecv = f;
    }

    public void StringReceived(string s)
    {
        sRecv = s;
    }

    public void Vector2IntReceived(Vector2Int v2i)
    {
        v2iRecv = v2i;
    }

    public void Vector3IntReceived(Vector3Int v3i)
    {
        v3iRecv = v3i;
    }

    public void Vector2Received(Vector2 v2)
    {
        v2Recv = v2;
    }

    public void Vector3Received(Vector3 v3)
    {
        v3Recv = v3;
    }

    public void Vector4Received(Vector4 v4)
    {
        v4Recv = v4;
    }
}
