using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Struggle
{
    class Game
    {
        List<Fraction> fractions;
        EventHandler ec;
        Client client;
        List<Color> fractionColors;
        List<uint> selectedEntities;

        public Client Client
        {
            get
            {
                return client;
            }
        }

        public List<uint> SelectedEntities
        {
            get
            {
                return selectedEntities;
            }
        }

        public void Run(RenderWindow app)
        {
            
            ec = new EventHandler();

            app.Closed += ec.Window_Closed;
            app.MouseButtonPressed += ec.Mouse_Pressed;
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
           
            List<Fraction> nf = new List<Fraction>();

            int count = 0;
            foreach(FractionCommon fc in gc.Fractions)
            {
                Fraction f = new Fraction(fractionColors[count], this);
                count++;

                foreach(EntityCommon ec in fc.entities)
                {
                    Entity e = new Entity(new Vector2f(ec.x, ec.y), ec.id, ec.mass);
                    e.SetFraction(f);
                  /*  if (fractions.Count > 0)
                        foreach (Entity es in fractions[gc.FractionId].entities)
                            if (es.Id == e.Id)
                            {
                                if (es.Selected)
                                     
                                if (es.DrawMoving)
                                    e.DrawMoving = true;
                            }*/

                    f.AddEntity(e);
                }
                nf.Add(f);
            }
            fractions = nf;

            Fraction pf = fractions[gc.FractionId];
            ec.SetPlayersFraction(ref pf);

         }

        public Game()
        {
            fractions = new List<Fraction>();
            fractionColors = new List<Color>();
            selectedEntities = new List<uint>();

            fractionColors.Add(Color.Blue);
            fractionColors.Add(Color.Red);
            fractionColors.Add(Color.Green);
            fractionColors.Add(Color.Cyan);
        }

 

        public void SetClient(ref Client client)
        {
            this.client = client;
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
