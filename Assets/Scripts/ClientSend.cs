using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);
            SendTCPData(_packet);
        }
    }
    public static void RoomConect(bool isDealer, string code)
    {
        using (Packet _packet = new Packet((int)ClientPackets.conectToRoom))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(code);
            _packet.Write(isDealer);
            SendTCPData(_packet);
        }
        Debug.Log("Sent room conect");
    }
    public static void CardSend(int id,string card,string code,Vector3 position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.cardSend))
        {
            _packet.Write(id);
            _packet.Write(card);
            _packet.Write(code);
            _packet.Write(position);
            print("Sending card");
            SendUDPData(_packet);
        }
    }
    public static void CardKeep(int id, string card, string code, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.cardKeep))
        {
            _packet.Write(id);
            _packet.Write(card);
            _packet.Write(code);
            SendUDPData(_packet);
        }
    }
    #endregion
}
