using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void CardReceive(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string card = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        int fromId = _packet.ReadInt();
        print("Card received from server: " + card);
        GameManager.instance.MoveCard(fromId,card, _position);
    }
    public static void CardGive(Packet _packet)
    {
        string card = _packet.ReadString();
        GameManager.instance.RemoveCardPetition(card);
    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        GameManager.instance.SpawnPlayer(_id, _username);
    }
    public static void RoomError(Packet _packet)
    {
        bool error = _packet.ReadBool();
        if (error)
        {
            UIManager.instance.ShowError();
        }
        else
        {
            UIManager.instance.Success();
        }
        

    }


}
