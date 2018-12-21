using System.Collections.Generic;
using SFML.Graphics;

namespace Struggle
{
    class Game
    {
        List<Fraction> fractions;

        public void Run(RenderWindow app)
        {
            while (app.IsOpen)
            {
                app.Clear();
                app.DispatchEvents();
                Draw(app);
                app.Display();
            }
        }

        public Game()
        {      
            fractions = new List<Fraction>();
            fractions.Add(new Fraction(Color.Blue, 0));
            fractions[0].AddEntity(new Warrior(new SFML.System.Vector2f(64, 64), fractions[0], 16));
        }

        public void Draw(RenderWindow app)
        {
            foreach (Fraction f in fractions)
                f.Draw(app);
        }
    }
}
