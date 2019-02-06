using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


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
            app.KeyPressed += ec.Key_Pressed;

            while (app.IsOpen)
            {
                app.Clear();
                app.DispatchEvents();
                Update();
                Draw(app);
                app.Display();
            }
        }

        public void SetInfo(GameCommon gc)
        {
            foreach(FractionCommon fc in gc.fractions)
            {
                Fraction f = new Fraction(Color.Blue, 0, this);
                foreach(EntityCommon ec in fc.GetEntities())
                {
                    Entity e = new Entity(new Vector2f(ec.x, ec.y), ec.mass);
                    f.AddEntity(e);
                }
                fractions.Add(f);
            }
        }
        public Game()
        {
            fractions = new List<Fraction>();
          /*fractions.Add(new Fraction(Color.Blue, 0, this));
            fractions.Add(new Fraction(Color.Red, 1, this));
            fractions.Add(new Fraction(Color.Green, 2, this));
            fractions.Add(new Fraction(Color.Magenta, 3, this));

            fractions.Add(new Fraction(new Color(128, 128, 128, 255), 4, this)); //neutral

            fractions[0].AddEntity(new Warrior(new Vector2f(64, 64), 16));
            fractions[0].AddEntity(new Warrior(new Vector2f(128, 128), 32));

            fractions[0].AddEntity(new Builder(new Vector2f(255, 255), 8));

            fractions[0].AddEntity(new Builder(new Vector2f(255, 305), 32));
            fractions[0].AddEntity(new Builder(new Vector2f(400, 400), 8));
            fractions[1].AddEntity(new Warrior(new Vector2f(576, 416), 16));
            fractions[1].AddEntity(new Warrior(new Vector2f(512, 352), 32));
            fractions[4].AddEntity(new BuildPlace("Warrior", 3, new Vector2f(100, 100), 32));*/
        }
        
        public void Update()
        {
            foreach (Fraction f in fractions)
                f.UpdateEntities();
        }

        public void HandleCollisions()
        {
            for (int i = fractions.Count - 1; i >= 0; --i)
                for (int j = fractions[i].entities.Count - 1; j >= 0; --j)
                    for (int k = i - 1; k >= 0; --k)
                        for (int l = fractions[k].entities.Count - 1; l >= 0; --l)
                        {
                            Entity e = fractions[i].entities[j];
                            Entity e2 = fractions[k].entities[l];

                            if (Utils.Distance(e.Position, e2.Position) <= Math.Min(e2.Mass, e.Mass))
                            {
                                if (e is Unit && e2 is Unit)
                                {
                                    Unit u = (Unit)e;
                                    Unit u2 = (Unit)e2;

                                    if (u.Mass > u2.Mass)
                                        u.Eat(u2);
                                    else if (u.Mass < u2.Mass)
                                        u2.Eat(u);
                                    return;
                                }
                                if (e is Builder && e2 is BuildPlace)
                                {
                                    Builder b = (Builder)e;
                                    BuildPlace bp = (BuildPlace)e2;

                                    bp.Increase(b.Fraction);
                                }
                                if (e is BuildPlace && e2 is Builder)
                                {
                                    Builder b = (Builder)e2;
                                    BuildPlace bp = (BuildPlace)e;

                                    bp.Increase(b.Fraction);
                                    b.Fraction.RemoveEntity(b);
                                }
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
