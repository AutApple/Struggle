using SFML.Graphics;
using SFML.Window;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;

namespace Struggle
{
    class Client
    {
        TcpClient tcpclient;
        NetworkStream stream;

        Game gm;

        public Client(string ip, int port, ref Game gm)
        {
            this.gm = gm;
            try
            {
                tcpclient = new TcpClient(ip, port);
                stream = tcpclient.GetStream();
            }
            catch(Exception)
            {
                Console.WriteLine("Unable to connect to the server");
                return;
            }
        }

        public void Send(byte[] data)
        {
            tcpclient.Client.Send(data);
        }

        public void NetworkLoop()
        {
            byte[] buffer = new byte[256];
            while (tcpclient.Connected)
            {
                do
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    GameCommon gc = bf.Deserialize(stream) as GameCommon;
                    gm.SetInfo(gc);
                    //size = stream.Read(buffer, 0, buffer.Length);
                    //Console.WriteLine(size);
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