using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Struggle_Server
{
    class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        static int count;
        static void Main(string[] args)
        {
            count = 1;
            
            Console.WriteLine("\t /Struggle Server/");
            /* Console.Write("IP: ");
             IPAddress ip = IPAddress.Parse(Console.ReadLine());*/
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());


            TcpListener socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            Console.WriteLine("[Server] Listening on port " + port.ToString() + "...");
            while (true)
            {
                
                TcpClient client = socket.AcceptTcpClient();
                lock (_lock) clients.Add(count, client);
                Console.WriteLine("[Server] Client connected");

                Thread t = new Thread(handler);
                t.Start(count);

                count++;
            }
        }


        public static void handler(object o)
        {
            int id = (int)o;
            TcpClient client;
            lock (_lock) client = clients[id];

            while (true)
            {
                    NetworkStream nstr = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = nstr.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                        break;

                    string data = Encoding.ASCII.GetString(response(buffer), 0, byte_count);
                    Execute(data, nstr);
                   
                    
            }

            lock (_lock) clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
            count--;
            Console.WriteLine("[Server] Client disconnected");
        }
        public static void Execute(string data, NetworkStream nstr)
        {
             
            switch (data[0])
            {
                case 'f':
                    string buf = "f";
                    if (count-1 < 5) {    //TODO: max players defined by map
                        buf += (char)(count-1);
                        send(buf, nstr);
                        Console.WriteLine("[Server] Client joins fraction " + (count-1).ToString() + "!");
                    }
                    else
                    {
                        buf += (char)5;
                        send(buf, nstr);
                        Console.WriteLine("[Server] Client joins spectators fraction!");
                    }
                    break;
               
            }
        }
        static void send(string data, NetworkStream nstr)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);
            nstr.Write(buffer, 0, buffer.Length);
        }

        static void broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach(TcpClient c in clients.Values)
                {
                    NetworkStream stream = c.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public static byte[] response(byte[] data)
        {
            return data;
        }
    }
}
   