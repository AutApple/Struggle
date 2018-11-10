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
        private Dictionary<int, Entity> _entities;
   
        Client _client;
        uint _fraction;

         

        bool spectator = false;


        public uint Fraction
        {
            get { return _fraction; }
            set { _fraction = value; }
        }

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


            _entities = new Dictionary<int, Entity>();

            string buffer = "";
            buffer = "i";
            _client.SendData(Encoding.ASCII.GetBytes(buffer));
             
 
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

        public void SpawnEntity(int x, int y, uint fraction, uint radius, int id)
        {
            Entity e = new Entity(new Vector2i(x, y), fraction, radius);
            _entities.Add(id, e);
        }

        public void RemoveEntity(int id)
        {
            _entities.Remove(id);
        }

        public void RemoveFractionEntities(int f)
        {
            foreach(KeyValuePair<int, Entity> e in _entities)
            {
                if (e.Value.Fraction == f)
                    _entities.Remove(e.Key);
            }
        }


        private void Tick()
        {
            _window.DispatchEvents();


           
        }
        private void Draw()
        {
            _window.Clear(new Color(20, 20, 20, 255));
            //drawing stuff
            foreach (KeyValuePair<int, Entity> e in _entities)
                e.Value.Draw(_window);
            _window.Display();
        }
        public void Stop()
        {
          
            _window.Close();
        }
        private void OnKeyP(object sender, KeyEventArgs e)
        {
                
            
        }
 
    }
}
