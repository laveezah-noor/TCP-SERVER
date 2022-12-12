using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Markup;
using System.Diagnostics;
using utils;
using System.Net.Http;
using System.Windows;

namespace UserDiaryClient
{
    public class Client
    {
        public static Client instance = new Client();
        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";
        public int port = 1234;
        public int myId = 0;
        public TCP tcp;

        private delegate dynamic PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.WriteLine("Instance already exists, destroying the object ");
                instance = null;
            }

        }
        //private void Start()
        private void Start(int requestId)
        {
            tcp = new TCP();
            tcp.requestId = requestId;
        }
        //public void ConnectToServer()
        public void ConnectToServer(int requestId)
        {
            if (tcp != null) {
                InitializeClientData();
                //    new Thread(() =>
                //                               {
                //                                   Thread.CurrentThread.IsBackground = true;
                //    tcp.Connect();
                //}).Start();
                tcp.Connect();
            }
            else
            {
                //Start();
                //ConnectToServer();
                Start(requestId);
                ConnectToServer(requestId);
            }
        }
        public void Disconnect()
        {
            tcp.Disconnect();
        }

        //    public Client()
        //    {
        //        IPAddress ip = IPAddress.Loopback;


        //        new Thread(() =>
        //            {
        //                Thread.CurrentThread.IsBackground = true;
        //                Connect(Convert.ToString(ip), "Hello I'm Device 1...");
        //            }).Start();

        //    }

        //    static void Connect(String server, String message)
        //    {
        //        try
        //        {
        //            Int32 port = 1234;
        //            TcpClient client = new TcpClient(server, port);

        //            NetworkStream stream = client.GetStream();

        //            sendData(stream, "Hello Server");
        //            getData(stream);

        //            stream.Close();
        //            client.Close();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Exception: {0}", e);
        //        }

        //        Console.Read();
        //    }
        public class TCP
        {
            public TcpClient socket;
            private Packet receivedData;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            public int requestId;
            public bool requestFinished = false;
            private static ManualResetEvent connectDone = new ManualResetEvent(false);
            Dictionary<string, object> response;

            //public TCP(int id)
            //{
            //    this.id = id;
            //}
            public void Disconnect()
            {
                //stream.Close();
                socket.Close();
            }
            public void Connect()
            {
        
                try
                {
                socket = new TcpClient();
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;



                receiveBuffer = new byte[dataBufferSize];

                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, null);
                    ////bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                    //connectDone.WaitOne();
                    Console.Read();
                    //MessageBox.Show(socket.Connected.ToString());
                    //socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
                    //socket.Connect();
                    //sendData(stream, "Hello Server");
                    //getData(stream);
                    //    while (socket.Connected)
                    //{

                    //stream = socket.GetStream();
                    //if (stream.DataAvailable)
                    //{
                    //    getData(stream);

                    //}
                    //    sendData(stream, "Hello Server");

                    //    stream.Close();
                    //    socket.Close();

                    //receivedData = new Packet();
                    //Console.WriteLine("Im here to read");
                    //stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    //if (stream.DataAvailable)
                    //{
                    //stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ConnectCallback, null);


                    //}
                    //}


                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
                //Console.ReadKey();
            
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                Console.WriteLine("================================");
                Console.WriteLine("=   Connected to the server    =");
                Console.WriteLine("================================");
                Console.WriteLine("Waiting for response...");
                
                socket.EndConnect(_result);
                connectDone.Set();
                if (!socket.Connected)
                {
                    return;
                }
                stream = socket.GetStream();
                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                //    if (stream.DataAvailable)
                //{
                //Console.WriteLine(receiveBuffer.ToString(),receiveBuffer.Length);
                //HandleData(receiveBuffer);

                //receiveBuffer = new byte[dataBufferSize];
                //stream = socket.GetStream();
                //while (true)
                //{

                //}

                //}

            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        return;
                    }
                    byte[] _data = new byte[_byteLength];
                   Array.Copy(receiveBuffer, 0, _data, 0, _byteLength);
                    dynamic res = HandleData(_data);
                    if (requestFinished)
                    {
                        if (requestId == (int)ClientPackets.login) { ClientSend.Login(); }
                        else if (requestId == (int)ClientPackets.register) { ClientSend.Register(); }
                        requestFinished = false;
                    }
                    response = res["Data"];
                    receivedData.Reset(res["Status"]);
                    stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    Disconnect();
                }
            }

            public Dictionary<string, object> Result()
            {
                if (socket.Connected)
                {
                    return response;
                }
                else
                {
                    Console.WriteLine("Connect again");
                    return null;
                }
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to server via TCP: {_ex}");
                }
            }

            private dynamic HandleData(byte[] _data)
            {
                Dictionary<string, object> data = null;
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return new Dictionary<string, dynamic>()
                            {
                                {"Status", true },
                                {"Data", data }
                            }; 
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    //ThreadManager.ExecuteOnMainThread(() =>
                    //{
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Console.WriteLine($"Packet ID: {_packetId}");
                            data = packetHandlers[_packetId](_packet);
                        }
                    //});

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return new Dictionary<string, dynamic>()
                            {
                                {"Status", true },
                                {"Data", data }
                            };
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return new Dictionary<string, dynamic>()
                            {
                                {"Status", true },
                                {"Data", data }
                            };
                }

                return new Dictionary<string, dynamic>()
                            {
                                {"Status", false },
                                {"Data", data }
                            };
            }
        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.loginReceived, ClientHandle.LoginReceived },
            { (int)ServerPackets.registerReceived, ClientHandle.RegisteredReceived },
        };
            Console.WriteLine("Initialized packets.");
        }

        static void getData(NetworkStream stream)
        {
            string response = string.Empty;

            // Bytes Array to receive Server Response.
            byte[] dataLength = new byte[4];

            // Read the Tcp Server Response Bytes.
            int bytesLength = stream.Read(dataLength, 0, dataLength.Length);

            int responseLength = Convert.ToInt16(Encoding.ASCII.GetString(dataLength, 0, bytesLength));
            Console.WriteLine(Encoding.ASCII.GetString(dataLength, 0, bytesLength));
            byte[] data = new byte[responseLength];

            // Read the Tcp Server Response Bytes.
            int bytes = stream.Read(data, 0, data.Length);

            response = Encoding.ASCII.GetString(data, 0, bytes);

            Console.WriteLine("Received: {0}", response);
        }

        static void sendData(NetworkStream stream, object message)
        {

            string strMessage = JMessage.Serialize(JMessage.FromValue(message));
            strMessage = strMessage.Length + strMessage;

            // Translate the Message into ASCII.
            byte[] data = Encoding.ASCII.GetBytes(strMessage);

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent: {0}", message);

        }
        
    }
}