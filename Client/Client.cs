using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Struggle
{
    class Client
    {
        TcpClient _client;
        Thread _thread;
        NetworkStream _ns;
        Core _core;

        public Client(Core core)
        {
            _core = core;
        }

        public void Connect(string ipaddr, int port)
        {
            IPAddress ip = IPAddress.Parse(ipaddr);

            _client = new TcpClient();
            _client.Connect(ip, port);

            Console.WriteLine("Client connected!");
            _ns = _client.GetStream();

            StreamReader sr = new StreamReader(_ns);
            try
            {
                while (!sr.EndOfStream)
                {
                    byte data = (byte)sr.Read();
                    Console.WriteLine(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection with server lost :(");
                Console.WriteLine(e);
                sr.Close();
                return;
            }
            sr.Close();
        }

        public void SendData(byte[] data)
        {
            _ns.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {
            _client.Client.Shutdown(SocketShutdown.Send);
            _thread.Join();
            _ns.Close();
            _client.Close();

            Console.WriteLine("Client disconnected!");
        }
    }
}
