using System;
using System.Collections.Generic;
using utils;

namespace UserDiaryClient
{
    public class ClientHandle
    {
        public static dynamic Welcome(Packet _packet)
        {
            string response = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {response}, id: {_myId}");
            //MessageBox.Show($"Message from server: {response}");
            Client.instance.myId = _myId;
            //Add Client Type Packet Which you want to send now
            ClientSend.WelcomeReceived();
            return new Dictionary<string, object>
            {{"Status", 200},
                {"Response", response },
                {"RequestId", (int)ClientPackets.welcomeReceived }
            };
        }
        public static Dictionary<string, object> LoginReceived(Packet _packet)
        {
            string response = _packet.ReadString();
            int _myId = _packet.ReadInt();

            JMessage jdata = JMessage.Deserialize(response);
            Dictionary<string, object> res = jdata.Value.ToObject<Dictionary<string, object>>();

            Console.WriteLine(res["Status"]);

            Console.WriteLine($"Message from server: {response}, id: {_myId}");
            //MessageBox.Show($"Message from server: {response}");
            Client.instance.myId = _myId;
            //ClientSend.WelcomeReceived();
            return res;
        }

        public static dynamic RegisteredReceived(Packet _packet)
        {
            string response = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {response}, id: {_myId}");
            //MessageBox.Show($"Message from server: {response}");
            Client.instance.myId = _myId;
            //ClientSend.WelcomeReceived();
            return null;
        }
    }
}
