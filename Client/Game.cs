using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Struggle
{
    class Game
    {
        List<Fraction> fractions;
        EventHandler ec;
        public void Run(RenderWindow app)
        {
            Fraction playersFraction = fractions[0];
            ec = new EventHandler(ref playersFraction);

            app.Closed += ec.Window_Closed;
            app.MouseButtonPressed += ec.Mouse_Pressed;
            app.MouseMoved += ec.Mouse_Moved;
            app.MouseButtonReleased += ec.Mouse_Released;

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

            fractions.Add(new Fraction(Color.Blue, 0, this));
            fractions.Add(new Fraction(Color.Red, 1, this));
            fractions.Add(new Fraction(Color.Green, 2, this));
            fractions.Add(new Fraction(Color.Magenta, 3, this));
            
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

        public void HandleCollisions()
        {
            for (int i = fractions.Count-1; i >= 0; --i)
                for (int j = fractions[i].entities.Count - 1; j >= 0; --j)
                    for (int k = i - 1; k >= 0; --k)
                        for (int l = fractions[k].entities.Count - 1; l >= 0; --l)
                        {
                            Unit e = (Unit)fractions[i].entities[j];
                            Unit e2 = (Unit)fractions[k].entities[l];

                            if(Math.Sqrt(Math.Pow(e.Position.X-e2.Position.X, 2)  + Math.Pow(e.Position.Y - e2.Position.Y, 2)) <= Math.Min(e2.Mass, e.Mass))
                            {
                                if (e.Mass > e2.Mass)
                                    e.Eat(e2);
                                else if(e.Mass < e2.Mass)
                                    e2.Eat(e);
                                return;
                            }
                        }
        }

        public void Draw(RenderWindow app)
        {
            foreach (Fraction f in fractions)
                f.Draw(app);
            if (ec.LeftMouseButton_Hold)
                DrawSelection(app, ec.LeftMouseButton_Hold_Coords);
        }

        public void DrawSelection(RenderWindow app, Vector2f coords)
        {
            RectangleShape rect = new RectangleShape();
            rect.Position = coords;
            rect.Size = new Vector2f(Mouse.GetPosition(app).X - coords.X, Mouse.GetPosition(app).Y - coords.Y);
            rect.FillColor = new Color(0, 250, 0, 100);
            
            app.Draw(rect);
        }
    }
}
