using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Struggle
{
    class Client
    {
        Socket socket;
        
        Timer timeoutTimer;
        bool timeoutFlag;

        public Client(string ip, int port)
        {
            try
            {
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(iep);
            }
            catch(Exception)
            {
                Console.WriteLine("Unable to connect to the server");
            }
            timeoutFlag = true;

            timeoutTimer = new Timer(4000);
            timeoutTimer.AutoReset = true;

            timeoutTimer.Elapsed += timeoutEvent;
            timeoutTimer.Start();

            NetworkLoop(ref socket);
        }

        private void NetworkLoop(ref Socket socket)
        {
            byte[] data = new byte[256];
            int bytes = 1;

            do
            {
                bytes = socket.Receive(data, data.Length, 0);

                switch (data[0])
                {
                    case 1:
                        timeoutFlag = true;
                        Console.WriteLine("timeout flag");
                        break;
                }
            }
            while (bytes > 0);
        }
        private void timeoutEvent(object sender, ElapsedEventArgs e)
        {
            if(timeoutFlag == true)
            {
                socket.Send(new byte[] { 0 });
                timeoutFlag = false;
            }
            else
            {
                Console.WriteLine("No response from server, closing connection");
                Close();
            }
         
        }

        public void Close()
        {
            socket.Shutdown(0);
        }
    }
}