using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
public class GameClient : MonoBehaviour, INetEventListener
{
    private NetManager _netClient;

    [SerializeField] private GameObject _clientBall;
    [SerializeField] private GameObject _clientBallInterpolated;

    private float _newBallPosX;
    private float _oldBallPosX;
    private float _lerpTime;
    public bool start;
   public bool finish;
    void Start()
    {
        _netClient = new NetManager(this);
        _netClient.UnconnectedMessagesEnabled = true;
        _netClient.UpdateTime = 15;
        _netClient.Start();
    }

    void Update()
    {
        if (_netClient == null)
            return;
        _netClient.PollEvents();
    
        var peer = _netClient.FirstPeer;
        if (peer != null && peer.ConnectionState == ConnectionState.Connected)
        {
            if (start)
            {
                NetDataWriter writer = new NetDataWriter();
                writer.Put("IWantPoints");
                print("Points");
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
                start = false;
            }

        }
        else
        {
            _netClient.SendBroadcast(new byte[] {1}, 5000);
        }
    }

    void OnDestroy()
    {
        if (_netClient != null)
            _netClient.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[CLIENT] We connected to " + peer.EndPoint);
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
    {
        Debug.Log("[CLIENT] We received error " + socketErrorCode);
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        ControlSystem cs = FindObjectOfType<ControlSystem>();
        string req = reader.GetString();
        int lenth = reader.GetInt();
        if (req == "PathIncomig")
        {
            cs.pointsRecived.Clear();
            for (int i = 0; i < lenth; i++)
                {
                    Vector3 pos = Vector3Packet.Deserialize(reader);
                    cs.pointsRecived.Add(pos);
                }
        }
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.BasicMessage && _netClient.ConnectedPeersCount == 0 && reader.GetInt() == 1)
        {
            Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
            _netClient.Connect(remoteEndPoint, "sample_app");
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {

    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
    }

    //public void SendMoveType(Actor actor, MoveType type)
    //{
    //    NetPeer peer = netClient.FirstPeer;
    //    if (peer == null)
    //        return;
    //    NetDataWriter writer = new NetDataWriter();
    //    writer.Put("MOVETYPE");
    //    writer.Put(actorManager.listActors.IndexOf(actor));
    //    writer.Put((int)type);
    //    peer.Send(writer, DeliveryMethod.ReliableOrdered);
    //    Debug.Log((int)type + " type index");
    //}
}