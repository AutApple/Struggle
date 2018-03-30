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
        private RenderWindow _window;
        public Entity testEntity;
        private List<Entity> _entities;
        Client _client;
        uint _fraction;
        public uint Fraction
        {
            get { return _fraction; }
            set { _fraction = value;  }
        }
        bool spectator = false;
        public Core()
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            _client = new Client(this);
            _client.Connect(ip, port);

            _window = new RenderWindow(new VideoMode(1600, 800), "Struggle");

            _window.SetVisible(true);
            _window.Closed += new EventHandler(OnClosed);

            _window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyP);


            _entities = new List<Entity>();
           
            _client.SendData(Encoding.ASCII.GetBytes("f"));
            if (_fraction == 5)
                spectator = true;


           
            while (_window.IsOpen)
            {
                Tick();
                Draw();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _client.Disconnect();
            _window.Close();
        }

        public void SpawnEntity(int x, int y, uint fraction, uint radius)
        {
            Entity e = new Entity(new Vector2i(x, y), fraction, radius);
            _entities.Add(e);
        }
        private void Tick()
        {
            _window.DispatchEvents();
        }
        private void Draw()
        {
            _window.Clear(new Color(20, 20, 20, 255));
            //drawing stuff
            foreach(Entity e in _entities)
                e.Draw(_window);
            _window.Display();
        }

        private void OnKeyP(object sender, KeyEventArgs e)
        {
                
            
        }
    }
}
