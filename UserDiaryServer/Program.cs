using System;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using UserDiary;
using Newtonsoft.Json.Linq;
using System.Numerics;

namespace UserDiaryServer // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        public class TcpClientTests
        {
        //    public Task Read()
        //    {
        //        IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234); // Address  
        //        TcpListener listener = new TcpListener(ep); // Instantiate the object  
        //        listener.Start(); // Start listening... 

        //        Console.WriteLine(@"  
        //    ===================================================  
        //           Started listening requests at: {0}:{1}  
        //    ===================================================",
        //        ep.Address, ep.Port);

        //        _ = Task.Run(() => { 
                
        //         while (true)
        //        {
        //            const int bytesize = 1024 * 1024;

        //            string message = null;
        //            byte[] buffer = new byte[bytesize];

        //            var sender = listener.AcceptTcpClient();
        //            sender.GetStream().Read(buffer, 0, bytesize);
        //            this.HandleTcpClient(sender);


        //            // Read the message and perform different actions  
        //            //message = cleanMessage(buffer);

        //            //// Save the data sent by the client;  
        //            //Person person = JsonConvert.DeserializeObject<Person>(message); // Deserialize  

        //            //byte[] bytes = System.Text.Encoding.Unicode.GetBytes("Thank you for your message, " + person.Name);
        //            //sender.GetStream().Write(bytes, 0, bytes.Length); // Send the response  

        //            //sendEmail(person);
        //        }
                
        //        });
                
        //        using TcpClient client = new TcpClient(ep);

        //        using NetworkStream ns = client.GetStream();
        //        using StreamReader sr = new StreamReader(ns);

        //        string message = sr.ReadToEnd();
        //        Console.Write(message);
        //}
            private void HandleTcpClient(TcpClient client)
            {
                //When the client connects, it sends an message
                using NetworkStream ns = client.GetStream();
                using StreamWriter sw = new StreamWriter(ns);

                sw.WriteLine("Connected Successfully!");
                ns.Flush();
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "UserDiary Server";
            Server.Start(2, 1234);

            //IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
            //TcpListener listener = new TcpListener(ep);
            //listener.Start();

            //Console.WriteLine(@"  
            //===================================================  
            //       Started listening requests at: {0}:{1}  
            //===================================================",
            //ep.Address, ep.Port);

            //// Run the loop continuously; this is the server.  
            //while (true)
            //{
            //    const int bytesize = 1024 * 1024;

            //    string message = "";
            //    byte[] buffer = new byte[bytesize];

            //    var sender = listener.AcceptTcpClient();
            //    sender.GetStream().Read(buffer, 0, bytesize);

            //    // Read the message and perform different actions  
            //    message = cleanMessage(buffer);

            //    Cache cache = Cache.getCache();
            //    UserDiary.DefaultUserList list = cache.UserList;

            //    string data = JMessage.Serialize(JMessage.FromValue(list));

            //    // Save the data sent by the client;  
            //    //Person person = JsonConvert.DeserializeObject<Person>(message); // Deserialize  


            //    byte[] bytes = System.Text.Encoding.ASCII.GetBytes("Thank you for your message, " + data);
            //    Console.WriteLine(data);
                //    if (message == "Hello Server")
                //    {
                //        greet(sender);
                //    }
                //    if (message == "Login")
                //    {
                //        login(sender);
                //    }
            //}
        //Thread t = new Thread(delegate ()
        //{
        //    // replace the IP with your system IP Address...
        //    IPAddress ip = IPAddress.Loopback;

        //    Server myserver = new(Convert.ToString(ip), 1234);
        //});
        //t.Start();

                Console.WriteLine("Server Started...!");
                //sender.GetStream().Write(bytes, 0, bytes.Length); // Send the response  

                //sendEmail(person);
                }

        private static void greet(TcpClient client)
        {
            string message = "Hello Client";
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            sendMessage(bytes, client);
            //return 
        }

        private static void login(TcpClient client)
        {

        }


        private static void sendEmail(Person p)
        {
            try
            {
                // Send an email to the user also to notify him of the delivery.  
                using (SmtpClient client = new SmtpClient("<your-smtp>", 25))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("<email>", "<pass>");

                    client.Send(
                        new MailMessage("<email-from>", p.Email,
                            "Thank you for using the Web Service",
                            string.Format(
    @"Thank you for using our Web Service, {0}.We have recieved your message, '{1}'.", p.Name, p.Message
                            )
                        )
                    );
                }

                Console.WriteLine("Email sent to " + p.Email); // Email sent successfully  
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        // Sends the message string using the bytes provided.  
        private static void sendMessage(byte[] bytes, TcpClient client)
        {
            client.GetStream()
                .Write(bytes, 0,
                bytes.Length); // Send the stream  
        }
    }

    class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    //class JMessage
    //{
    //    public Type Type { get; set; }
    //    public JToken Value { get; set; }

    //    public static JMessage FromValue<T>(T value)
    //    {
    //        return new JMessage { Type = typeof(T), Value = JToken.FromObject(value) };
    //    }

    //    public static string Serialize(JMessage message)
    //    {
    //        return JToken.FromObject(message).ToString();
    //    }

    //    public static JMessage Deserialize(string data)
    //    {
    //        return JToken.Parse(data).ToObject<JMessage>();
    //    }
    //}

}