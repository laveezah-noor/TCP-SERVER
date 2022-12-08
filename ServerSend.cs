using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;


namespace UserDiaryServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.Max; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.Max; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                Console.WriteLine($"Sending: {_msg} ");
                SendTCPData(_toClient, _packet);
            }
        }
        public static void LoginReceived(int _toClient, string _msg)
        {
            //In fulure, it will send dictionary as a packet
            using (Packet _packet = new((int)ServerPackets.loginReceived))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                Console.WriteLine($"Sending: {_msg} ");
                SendTCPData(_toClient, _packet);
            }
        }
        public static void RegisterReceived(int _toClient, string _msg)
        {
            //In fulure, it will send dictionary as a packet
            using (Packet _packet = new((int)ServerPackets.registerReceived))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                Console.WriteLine($"Sending: {_msg} ");
                SendTCPData(_toClient, _packet);
            }
        }
    }
}
