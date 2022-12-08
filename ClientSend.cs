using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace UserDiaryClient
{
    public class ClientSend
    {
        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write("HardCoded User");

                SendTCPData(_packet);
                Console.WriteLine("Welcome Received Packet Send!");
            }
        }
        #endregion

        public static void Login()
        {
            using (Packet _packet = new Packet((int)ClientPackets.login))
            {   
                _packet.Write(Client.instance.myId);
                _packet.Write("HardCoded User");

                SendTCPData(_packet);
                Console.WriteLine("Login Packet Send!");
            }
        }

        public static void Register()
        {
            using (Packet _packet = new Packet((int)ClientPackets.register))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write("HardCoded User");

                SendTCPData(_packet);
                Console.WriteLine("Register Packet Send!");
            }
        }
    }
}
