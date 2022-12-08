using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace UserDiaryServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }
        public static void Login(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            //string _password = _packet.ReadString();

            Console.WriteLine($" player {_fromClient} is here to login with username = {_username}.");
            //In fulure, it will send dictionary
            ServerSend.LoginReceived(_fromClient, "Logged In Successfull");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

        public static void Register(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            //string _password = _packet.ReadString();

            Console.WriteLine($" player {_fromClient} is here to register with username = {_username}.");
            //In fulure, it will send dictionary
            ServerSend.LoginReceived(_fromClient, "Registered Successfull");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

    }
}
