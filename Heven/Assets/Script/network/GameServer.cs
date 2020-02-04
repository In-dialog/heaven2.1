using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
public class GameServer : MonoBehaviour, INetEventListener, INetLogger
{
    private NetManager _netServer;
    private List <NetPeer> _ourPeer = new List<NetPeer>();
    //private NetDataWriter _dataWriter;
    public bool Star;

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
        if (_ourPeer != null)
        {
            if (Star)
            {
             
            }
   
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
        if(!_ourPeer.Contains(peer))
        _ourPeer.Add(peer);
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
            _ourPeer.Remove(peer);
    }
  
    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        string req = reader.GetString();
        if (req == "IWantPoints")
        {
            List<Vector3> vectors = new List<Vector3>();
            //for (int i = 0; i < 200; i++)
            //    vectors[i] = new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
            ///////---->list of point
            ////////_---->pathFinding.bestObstions
            ///
            SendPoint(vectors, peer);
        }
        else if (req == "Finish")
        {
            //// count ++ ; if count == pear.count count =0;
            /// RecalculateDrawing;
        }


    }
    public void SendPoint(List<Vector3> marker, NetPeer peer)
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put("PathIncomig");
        writer.Put(marker.Count);
        for (int i = 0; i < marker.Count; i++)
        {
            Vector3Packet.Serialize(writer, marker[i]);
        }
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
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
        res.x = (reader.GetFloat()-15);
        res.z = reader.GetFloat();
        res.y = (reader.GetFloat()+30);
        return res *7;
    }
}