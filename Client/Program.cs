using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using UserDiaryClient;

namespace UserDiaryClient // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            //            byte[] messageBytes = System.Text.Encoding.Unicode.GetBytes("Hello Server");

            //            const int bytesize = 1024 * 1024;
            //            try // Try connecting and send the message bytes  
            //            {
            //                TcpClient client = new TcpClient("127.0.0.1", 1234); // Create a new connection  
            //                NetworkStream stream = client.GetStream();

            //                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  

            //                if (stream.DataAvailable)
            //                {
            //                messageBytes = new byte[bytesize]; // Clear the message   

            //                // Receive the stream of bytes  
            //                stream.Read(messageBytes, 0, messageBytes.Length);
            //                    //Console.WriteLine(messageBytes);
            //                Console.WriteLine(Encoding.Unicode.GetString(messageBytes));

            //                // Clean up  

            //                }

            //                //stream.Dispose();
            //                //client.Close();
            //            }
            //            catch (Exception e) // Catch exceptions  
            //            {
            //                Console.WriteLine(e.Message);
            //            }
            //        }

            //    }
            //}
            //IPAddress ip = IPAddress.Loopback;

            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    Connect(Convert.ToString(ip), "Hello I'm Device 1...");
            //}).Start();

            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    Connect(Convert.ToString(ip), "Hello I'm Device 2...");
            //}).Start();
            //Client.instance.ConnectToServer();
            Client.instance.ConnectToServer(1);
            //Client.instance.tcp.Login();
            //Console.ReadLine();
            Client.instance.Disconnect();
        }

        static void Connect(String server, String message)
        {
            try
            {
                Int32 port = 1234;
                TcpClient client = new TcpClient(server, port);
                Console.WriteLine("================================");
                Console.WriteLine("=   Connected to the server    =");
                Console.WriteLine("================================");
                Console.WriteLine("Waiting for response...");

                NetworkStream stream = client.GetStream();

                //int count = 0;
                while (client.Connected)
                {
                    if (stream.DataAvailable)
                {
                        

                    Byte[] buffer = new Byte[4024];
                    Int32 bytes = stream.Read(buffer, 0, buffer.Length);
                    string response = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                    //JMessage jdata = JMessage.Deserialize(response);
                    //UserDiary.DefaultUserList list = jdata.Value.ToObject<UserDiary.DefaultUserList>();
                    Console.WriteLine("Received: {0}", response);

                stream.Close();
                }

                //    Console.WriteLine("Write");
                //string input = Console.ReadLine();
                //    Console.WriteLine(input);

                //if (input == "get") {
                //    // Translate the Message into ASCII.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                //// Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);
                //Console.WriteLine("Sent: {0}", message);

                //String response = String.Empty;

                //// Bytes Array to receive Server Response.
                ////Byte[] dataLength = new Byte[4];
                ////// Read the Tcp Server Response Bytes.
                ////Int32 bytesLength = stream.Read(dataLength, 0, dataLength.Length);

                ////int responseLength = Convert.ToInt16(System.Text.Encoding.ASCII.GetString(dataLength, 0, bytesLength));
                //data = new Byte[4096];
                //// Read the Tcp Server Response Bytes.
                //Int32 bytes = stream.Read(data, 0, data.Length);

                //response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                ////JMessage jdata = JMessage.Deserialize(response);
                ////UserDiary.DefaultUserList list = jdata.Value.ToObject<UserDiary.DefaultUserList>();
                //Console.WriteLine("Received: {0}", response);

                //    Thread.Sleep(2000);
                //}

                //}
                //if (Console.ReadKey().KeyChar == 'c')
                //{

                //}
                //if (Console.ReadKey().KeyChar == 'd')
                //{

                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }

            Console.Read();
        }
        class JMessage
        {
            public Type Type { get; set; }
            public JToken Value { get; set; }

            public static JMessage FromValue<T>(T value)
            {
                return new JMessage { Type = typeof(T), Value = JToken.FromObject(value) };
            }

            public static string Serialize(JMessage message)
            {
                return JToken.FromObject(message).ToString();
            }

            public static JMessage Deserialize(string data)
            {
                return JToken.Parse(data).ToObject<JMessage>();
            }
        }
    }
}