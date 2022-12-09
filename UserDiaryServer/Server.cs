using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using utils;

namespace UserDiaryServer
{
    class Server
    {
        public static int Port { get; private set; }
        public static int Max { get; private set; }
        private static TcpListener tcpListener;
        public static Dictionary<int, Client> clients = new();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        public static ManualResetEvent tcpClientConnected =
    new ManualResetEvent(false);

        public static void Start (int _max ,int _port)
        {
            Max = _max;
            Port = _port;
            Console.WriteLine("Starting Server.........");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Loopback, Port);
            tcpListener.Start();

            Console.WriteLine("Server Running!");
            StartListener(tcpListener);
            //tcpClientConnected.Reset();
            //Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
            //t.Start(client);
            //TCPConnectCallback();
            //while (true)
            //{
            //if (!tcpListener.Pending())
            //{
            //tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            //}

            //}

            //Console.WriteLine($"Server started on {Port}");
            //tcpClientConnected.WaitOne();
        }

        private static void TCPConnectCallback()
        //{
        //private static void TCPConnectCallback(IAsyncResult _result)
        {
            while (true)
            {
                TcpClient _client = tcpListener.AcceptTcpClient();
                //Console.WriteLine("Im");
                //TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
                //tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

                Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}");

            for (int i = 1; i <= Max; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                        Console.WriteLine(clients[i]);
                        //Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
            //t.Start(client);
                    clients[i].tcp.Connect(_client);
                        break;
                }
            }
            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full");
            }

        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= Max; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.login, ServerHandle.Login },
                { (int)ClientPackets.register, ServerHandle.Register }
            };
            Console.WriteLine("Initialized packets.");
        }
        //TcpListener server = null;
        //public Server(string ip, int port)
        //{
        //    IPAddress localAddr = IPAddress.Parse(ip);
        //    server = new TcpListener(localAddr, port);
        //    server.Start();
        //    StartListener();
        //}

        public static void StartListener(TcpListener server)
        {
            try
            {
                while (!server.Pending())
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                //Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}");

                //for (int i = 1; i <= Max; i++)
                //{
                //    if (clients[i].tcp.socket == null)
                //    {
                //        Console.WriteLine(clients[i]);
                //        //Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                //        //t.Start(client);
                //        clients[i].tcp.Connect(client);
                //        break;
                //    }
                //}
                //Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full");
            }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }



        public static void HandleDeivce(Object obj)
        {
            
            TcpClient client = (TcpClient)obj;
            if (client.Connected) clients[1].tcp.Connect(client);
        
        //var stream = client.GetStream();
        //    string imei = String.Empty;

        //    string data = null;
        //    Byte[] bytes = new Byte[256];
        //    int i;
        //    try
        //    {
                
                //ServerSend.Welcome(1, "Welcome to the Server");
                //string jData = JMessage.Serialize(JMessage.FromValue("Welcome to the Server"));
                //jData = jData.Length + jData;
                //Byte[] reply = System.Text.Encoding.ASCII.GetBytes(jData);
                //Console.WriteLine($"{reply.Length}, {jData.Length}");
                //stream.Write(reply, 0, reply.Length);
                //Console.WriteLine("{1}: Sent: {0}{1}", jData, Thread.CurrentThread.ManagedThreadId);

                //if (stream.DataAvailable)
                //{
                //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    string hex = BitConverter.ToString(bytes);
                //    data = Encoding.ASCII.GetString(bytes, 0, i);
                //    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                    //string str = "Hey Device!";
                    ////UserDiary.DefaultUserList str = UserDiary.Cache.getCache().UserList;
                    //Cache cache = Cache.getCache();
                    //UserDiary.DefaultUserList list = cache.UserList;

                    //string jData = JMessage.Serialize(JMessage.FromValue(list));
                    //jData = jData.Length + jData;
                    //byte[] reply = System.Text.Encoding.ASCII.GetBytes(jData);
                    //Console.WriteLine($"{reply.Length}, {jData.Length}");
                    //stream.Write(reply, 0, reply.Length);
                    //Console.WriteLine("{1}: Sent: {0}{1}", jData, Thread.CurrentThread.ManagedThreadId);

                //}

                //}
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception: {0}", e.ToString());
            //    client.Close();
            //}
             }
    }
}