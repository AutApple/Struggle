using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Struggle
{
    class Game
    {
        List<Fraction> fractions;

        public void Run(RenderWindow app)
        {
            Fraction playersFraction = fractions[0];
            EventHandler ec = new EventHandler(ref playersFraction);

            app.Closed += ec.Window_Closed;
            app.MouseButtonPressed += ec.Mouse_Pressed;
            app.MouseMoved += ec.Mouse_Moved;

            while (app.IsOpen)
            {
                app.Clear();
                app.DispatchEvents();
                Update();
                Draw(app);
                app.Display();
            }
        }

        public Game()
        {      
            fractions = new List<Fraction>();

            fractions.Add(new Fraction(Color.Blue, 0));
            fractions.Add(new Fraction(Color.Red, 1));
            fractions.Add(new Fraction(Color.Green, 2));
            fractions.Add(new Fraction(Color.Magenta, 3));
            
            fractions[0].AddEntity(new Warrior(new Vector2f(64, 64), 16));
            fractions[0].AddEntity(new Warrior(new Vector2f(128, 128), 32));
            fractions[1].AddEntity(new Warrior(new Vector2f(576, 416), 16));
            fractions[1].AddEntity(new Warrior(new Vector2f(512, 352), 32));
        }

        public void Update()
        {
            foreach (Fraction f in fractions)
                f.UpdateEntities();
        }

        public void Draw(RenderWindow app)
        {
            foreach (Fraction f in fractions)
                f.Draw(app);
        }
    }
}
