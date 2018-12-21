using System;
using System.Collections.Generic;
using SFML;
using SFML.Graphics;
using SFML.Window;
using System.Text;
using SFML.System;

namespace Struggle
{
    class Core
    {
        //Client _client;
        List<Entity> entities;
        RenderWindow app;
        public Core()
        {
            app = new RenderWindow(new VideoMode(640, 480), "Struggle");
            entities = new List<Entity>();

            entities.Add(new Entity(new Vector2i(100, 100), 0, 32));
            entities.Add(new Entity(new Vector2i(200, 200), 1, 32));

            // Console.Write("IP: ");
            // string ip = Console.ReadLine();
            // Console.Write("Port: ");
            // int port = int.Parse(Console.ReadLine());
            // _client = new Client(this);
            // _client.Connect(ip, port);
            while (app.IsOpen)
            {
                app.Clear();
                Draw();
                app.Display();
 
            }
        }

        public void Draw()
        {
            foreach (Entity e in entities)
                e.Draw(app);
        }
    }
}
