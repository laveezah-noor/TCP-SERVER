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

        //class LoginRequest
        //{
        //    public string username;
        //    public string password;

        //    public LoginRequest(string username, string password)
        //    {
        //        this.username = username;
        //        this.password = password;
        //    }
        //    //public LoginRequest() { }
        //}

        public static void Login(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string response = _packet.ReadString();
            
            Console.WriteLine(response);

            JMessage jdata = JMessage.Deserialize(response);
            utils.LoginRequest result = jdata.Value.ToObject<utils.LoginRequest>();
            //Console.WriteLine(result);
            
            string _username = result.username;
            string _password = result.password;
            
            Console.WriteLine($"Username: {_username}, Password: {_password}");


            dynamic res = UserDiary.Cache.getCache().UserLog(_username,_password);
            if ((int)res["Status"] == 200)
            { Console.WriteLine("Login Successful"); }
                dynamic clientRes  = new Dictionary<string, object>(){
                            { "Status", (int)res["Status"] },
                            { "Response", res["Response"]} };
                ServerSend.LoginReceived(_fromClient, JMessage.Serialize(JMessage.FromValue(clientRes)));
            
            Console.WriteLine($" player {_fromClient} is here to login with username = {_username}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

        public static void Register(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string response = _packet.ReadString();

            JMessage jdata = JMessage.Deserialize(response);
            utils.RegisterRequest result = jdata.Value.ToObject<utils.RegisterRequest>();
            //Console.WriteLine(result);


            //Console.WriteLine($"Username: {_username}, Password: {_password}");

            dynamic res = UserDiary.Cache.getCache().Register(result.name, result.username, result.password, result.email, result.phone);

            Console.WriteLine($" player {_fromClient} is here to register with username = {result.username}.");
            //In fulure, it will send dictionary
            ServerSend.LoginReceived(_fromClient, "Registered Successfull");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{result.username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

    }
}
