using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
public class GameServer : MonoBehaviour, INetEventListener, INetLogger
{
    private NetManager     _netServer;
    private List <NetPeer> _ourPeer = new List<NetPeer>();
    //private NetDataWriter _dataWriter;
    public bool Conection;
    public int        Count_Peers = 0;

    void Start()
    {
        NetDebug.Logger = this;
        _netServer = new NetManager(this);
        _netServer.Start(5000);
        _netServer.BroadcastReceiveEnabled = true;
        _netServer.UpdateTime = 15;
    }

    void Update()
    {
        _netServer.PollEvents();
    }

    void FixedUpdate()
    {
        if (_ourPeer.Count >= 1)
        {
            Conection = true;
            //if (Start_conection)
            //{
             
            //}
        }
        else
        {
            Conection = false;
        }
    }

    void OnDestroy()
    {
        NetDebug.Logger = null;
        if (_netServer != null)
            _netServer.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[SERVER] We have new peer " + peer.EndPoint);
        if (!_ourPeer.Contains(peer))
        {
            _ourPeer.Add(peer);
            Count_Peers++;
        }
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
    {
        Debug.Log("[SERVER] error " + socketErrorCode);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader,
        UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.Broadcast)
        {
            Debug.Log("[SERVER] Received discovery request. Send discovery response");
            NetDataWriter resp = new NetDataWriter();
            resp.Put(1);
            _netServer.SendUnconnectedMessage(resp, remoteEndPoint);
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        request.AcceptIfKey("sample_app");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[SERVER] peer disconnected " + peer.EndPoint + ", info: " + disconnectInfo.Reason);

        if (_ourPeer.Contains(peer))
        {
            _ourPeer.Remove(peer);
            Count_Peers--;
        }
    }
  
    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        string req = reader.GetString();
        if (req == "IWantPoints")
        {
            Debug.Log("The slave wants points");
            StateOfMachine.Instance.SetSate = true;
        }
    }


    public void SendPoint(Vector3[] marker)
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put("PathIncomig");
        writer.Put(marker.Length);
        for (int i = 0; i < marker.Length - 1; i++)
        {
            Vector3Packet.Serialize(writer, marker[i]);
        }
        foreach (var item in _ourPeer)
        {
            item.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }

    public void Send_Center(Vector3 input)
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put("Center");
        Debug.Log("----------------------Sending the center coordinates-----------------------");
        Vector3Packet.Serialize(writer, input);
        foreach (var item in _ourPeer)
        {
            item.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }


    public void WriteNet(NetLogLevel level, string str, params object[] args)
    {
        Debug.LogFormat(str, args);
    }

}
struct Vector3Packet
{
    public static void Serialize(NetDataWriter writer, Vector3 v)
    {
        writer.Put(v.x);
        writer.Put(v.y);
        writer.Put(v.z);
    }

    public static Vector3 Deserialize(NetDataReader reader)
    {
        Vector3 res = new Vector3();
        res.x = reader.GetFloat();
        res.y = reader.GetFloat();
        res.z = reader.GetFloat();
        return res;
    }
}