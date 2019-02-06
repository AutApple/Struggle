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
            GameCommon gc = new GameCommon();
            Console.WriteLine(gc.Show());

            byte[] buffer = new byte[255];
            Stream str = new FileStream("gamecommon.dat", FileMode.CreateNew);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(str, gc);

            str.Close();
            Stream str2 = new FileStream("gamecommon.dat", FileMode.Open);
                      
            GameCommon gc2 = (GameCommon) bf.Deserialize(str2);
            Console.WriteLine(gc2.Show());
            
            str2.Close();
            /*
            RenderWindow app = new RenderWindow(new VideoMode(640, 480), "Struggle");

            Client client = new Client("127.0.0.1", 7777, app);
            Thread networkThread = new Thread(client.NetworkLoop);
            networkThread.Start();
            */

            // Game game = new Game();
            // game.Run(app);
        }
    }
}
