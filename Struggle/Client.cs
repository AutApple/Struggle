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
        int bufR= -1;
        int bufX = -1;
        int bufY = -1;
        int bufId = -1;
        int bufF = -1;

         
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
            try
            {
                while (!sr.EndOfStream)
                {


                    char data = (char)sr.Read();
 
                    switch (data)
                    {
                        case 'f':
                            _core.Fraction = (uint)sr.Read();
                            Console.WriteLine(_core.Fraction);
                            Console.WriteLine("Fraction changed to " + _core.Fraction + "!");

                            
                            break;
                        case 'r':
                            string rstr = sr.ReadLine();
                            int r = BitConverter.ToInt32(Encoding.ASCII.GetBytes(rstr), 0);
                            bufR = r;

                            Console.WriteLine("Radius buffer changed to " + bufR.ToString() + "!");

                            break;
                        case 'x':
                            string xstr = sr.ReadLine();
                            int x = BitConverter.ToInt32(Encoding.ASCII.GetBytes(xstr), 0);
                            bufX = x;

                            Console.WriteLine("X buffer changed to " + bufR.ToString() + "!");

                            break;
                        case 'y':
                            string ystr = sr.ReadLine();
                            int y = BitConverter.ToInt32(Encoding.ASCII.GetBytes(ystr), 0);
                            bufY = y;

                            Console.WriteLine("Y buffer changed to " + bufR.ToString() + "!");

                            break;
                        case 'i':
                            string istr = sr.ReadLine();
                            int i = BitConverter.ToInt32(Encoding.ASCII.GetBytes(istr), 0);
                            bufId = i;

                            Console.WriteLine("ID buffer changed to " + bufId.ToString() + "!");
                            break;
                        case '0':
                            string fstr = sr.ReadLine();
                            int f = BitConverter.ToInt32(Encoding.ASCII.GetBytes(fstr), 0);
                            bufF = f;

                            Console.WriteLine("Fraction buffer changed to " + bufF.ToString() + "!");
                            break;
                        case 's':
                            _core.SpawnEntity(bufX, bufY, (uint)bufF, (uint)bufR, bufId);
                            Console.WriteLine("Entity spawned at X = " + bufX.ToString() + ", Y = " + bufY.ToString() + ". Radius = " + bufR.ToString() + ", ID = " + bufId.ToString() + ", Fraction = " + bufF.ToString());
                            bufX = -1;
                            bufY = -1;
                            bufR = -1;
                            bufId = -1;
                            bufF = -1;
                            break;
                        case '1':
                            int fdel = sr.Read();
                            _core.RemoveFractionEntities(fdel);
                            Console.WriteLine("Entities of fraction " + fdel + " removed!");
                            break;
                    }

                }
            }catch(Exception e)
            {
                Console.WriteLine("Connection with server lost :(");
                sr.Close();
                _core.Stop();
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
