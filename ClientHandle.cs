using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using utils;

namespace UserDiaryClient
{
    public class ClientHandle
    {
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {_msg}, id: {_myId}");
            //MessageBox.Show($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            //Add Client Type Packet Which you want to send now
            ClientSend.WelcomeReceived();
        }
        public static void LoginReceived(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {_msg}, id: {_myId}");
            //MessageBox.Show($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            //ClientSend.WelcomeReceived();
        }

        public static void RegisteredReceived(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {_msg}, id: {_myId}");
            //MessageBox.Show($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            //ClientSend.WelcomeReceived();
        }
    }
}
