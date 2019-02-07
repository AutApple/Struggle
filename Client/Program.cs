using SFML.Graphics;
using SFML.Window;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Struggle
{
    class Program
    {
        static void Main()
        {
            
            RenderWindow app = new RenderWindow(new VideoMode(640, 480), "Struggle");


            Game gm = new Game();
            Client client = new Client("127.0.0.1", 7777, ref gm);
            Thread networkThread = new Thread(client.NetworkLoop);
            
            networkThread.Start();

            gm.SetClient(ref client);
            gm.Run(app);
        }
    }
}
