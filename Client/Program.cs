using SFML.Graphics;
using SFML.Window;
using System.Threading;

namespace Struggle
{
    class Program
    {
        static void Main()
        { 
            RenderWindow app = new RenderWindow(new VideoMode(640, 480), "Struggle");

            Client client = new Client("127.0.0.1", 7777, app);
            Thread networkThread = new Thread(client.NetworkLoop);
            networkThread.Start();


           // Game game = new Game();
           // game.Run(app);
        }
    }
}
