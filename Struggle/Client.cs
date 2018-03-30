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
            _thread = new Thread(o => ReceiveData((TcpClient)o));

            _thread.Start(_client);
        }

        void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            StreamReader sr = new StreamReader(ns);

            // byte[] receivedBytes = new byte[1024];
            // int byte_count;


            //    while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            //      Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            while (!sr.EndOfStream)
            {
                char data = (char)sr.Read();
                switch (data)
                {
                    case 'f':
                        _core.Fraction = (uint)sr.Read();
                        Console.WriteLine(_core.Fraction);


                        _core.SpawnEntity(100, 100, _core.Fraction, 100);
                        break;
                }
                
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
