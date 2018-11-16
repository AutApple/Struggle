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
        Client _client;
        public Core()
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            _client = new Client(this);
            _client.Connect(ip, port);
        }
    }
}
