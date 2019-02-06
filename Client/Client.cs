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
        RenderWindow app;
        public Client(string ip, int port, RenderWindow app)
        {
            this.app = app;
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
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.AssemblyFormat = FormatterAssemblyStyle.Simple;
                    GameCommon gc = bf.Deserialize(stream) as GameCommon;

                    Game gm = new Game();
                    gm.Run(app);

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