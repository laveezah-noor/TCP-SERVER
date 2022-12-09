using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using utils;

namespace UserDiaryServer
{
    public class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }

        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;
            string ip;

            public TCP( int id)
            {
                this.id = id;
            }

            public void Connect(TcpClient _socket)
            {

                Console.WriteLine("================================");
                Console.WriteLine($"=   Connected to the client {id}    =");
                Console.WriteLine("================================");

                socket = _socket;
                ip = _socket.Client.RemoteEndPoint.ToString();
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                //string imei = String.Empty;

                //string data = null;
                //Byte[] bytes = new Byte[256];
                //int i;
                //try
                //{
                stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
                ServerSend.Welcome(id, "Welcome to the Server");
                //    //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //    //{
                //    string hex = BitConverter.ToString(bytes);
                //    data = Encoding.ASCII.GetString(bytes, 0, i);
                //    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                //    //string str = "Hey Device!";
                //    ////UserDiary.DefaultUserList str = UserDiary.Cache.getCache().UserList;
                //    Cache cache = Cache.getCache();
                //    UserDiary.DefaultUserList list = cache.UserList;

                //    string jData = JMessage.Serialize(JMessage.FromValue(list));
                //    jData = jData.Length + jData;
                //    Byte[] reply = System.Text.Encoding.ASCII.GetBytes(jData);
                //    Console.WriteLine($"{reply.Length}, {jData.Length}");
                //    stream.Write(reply, 0, reply.Length);
                //    Console.WriteLine("{1}: Sent: {0}{1}", jData, Thread.CurrentThread.ManagedThreadId);

                //}
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Exception: {0}", e.ToString());
                //client.Close();
                //}
                //while (stream.DataAvailable)
                //{

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

                    HandleData(_data);
                    receivedData = new Packet();
                    receiveBuffer = new byte[dataBufferSize];

                    stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.Write(_packet.ToArray(), 0, _packet.Length());
                        Console.WriteLine("Data Sent");
                        //stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    Console.WriteLine($"This is the packetLength:{_packetLength}");
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    //ThreadManager.ExecuteOnMainThread(() =>
                    //{
                    Console.WriteLine($"PacketBytes: {_packetBytes}");
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Console.WriteLine($"Packet ID: {_packetId}");
                            Server.packetHandlers[_packetId](id, _packet);
                        }
                    //});

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
