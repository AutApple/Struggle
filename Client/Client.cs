using SFML.Graphics;
using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Struggle
{
    class Client
    {
        TcpClient tcpclient;
        NetworkStream stream;

        public Client(string ip, int port, RenderWindow app)
        {
            try
            {
                tcpclient = new TcpClient(ip, port);
                stream = tcpclient.GetStream();
            }
            catch(Exception)
            {
                Console.WriteLine("Unable to connect to the server");
                app.Close();
                return;
            }
        }

        public void NetworkLoop()
        {
            int size = 0;
            byte[] buffer = new byte[256];
            while (tcpclient.Connected)
            {
                do
                {
                    size = stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine(size);
                    foreach (byte b in buffer)
                        Console.Write(b + " ");
                    switch(buffer[0])
                    {
                        default:
                            break;
                    }
                }
                while (stream.DataAvailable);
            }
        }

        public void Close()
        {
            stream.Close();
            tcpclient.Close();
        }
    }
}