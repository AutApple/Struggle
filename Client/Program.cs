using SFML.Graphics;
using SFML.Window;

namespace Struggle
{
    class Program
    {
        static void Main()
        {
            RenderWindow app = new RenderWindow(new VideoMode(640, 480), "Struggle");
            Game game = new Game();
            game.Run(app);
        }
    }
}
