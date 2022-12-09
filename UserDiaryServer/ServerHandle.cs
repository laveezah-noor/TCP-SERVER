using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        class LoginRequest
        {
            public string username;
            public string password;

            public LoginRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
            //public LoginRequest() { }
        }

        public static void Login(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string stringData = _packet.ReadString();
            
            Console.WriteLine(stringData);
            //try { 
            dynamic result =
                JsonConvert.DeserializeObject(stringData);
            //var result = JsonConvert.DeserializeObject<Request>(stringData);
            //LoginRequest results = JToken.Parse(stringData).ToObject<LoginRequest>();
            //Console.WriteLine($"{result}, {result.GetType()}");

            string _username = result.username;
            string _password = result.password;
            //string _username, _password;
            //}
            //catch(Exception err)
            //{
            //    Console.WriteLine("JTOKEN PARSE");
            //    Console.WriteLine(err.Message);
            //}

            Console.WriteLine($"{_username}, {_password}");

            //Console.WriteLine($" player {_fromClient} is here to login with username = {_username}.");
            //In fulure, it will send dictionary
            ServerSend.LoginReceived(_fromClient, "Logged In Successfull");
            if (_fromClient != _clientIdCheck)
            {
                //Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
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
