using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Setup();

            string strmp, strport, strentmax,
                   strdeffrac;

            Configuration.paramList.TryGetValue("port", out strport);
            Configuration.paramList.TryGetValue("maxPlayers", out strmp);
            Configuration.paramList.TryGetValue("maxEntities", out strentmax);
            Configuration.paramList.TryGetValue("defaultFraction", out strdeffrac);

            short config_port;
            int config_maxPlayers, config_maxEntities, config_defaultFraction;

            try
            {
                config_port = short.Parse(strport);
                config_maxPlayers = int.Parse(strmp);
                config_maxEntities = int.Parse(strentmax);
                config_defaultFraction = int.Parse(strdeffrac);
            }
            catch (Exception)
            {
                Console.WriteLine("[Server] Oops, something is wrong with config. ");
                return;
            }

            try
            {
                Server server = new Server(config_port, config_maxPlayers, config_maxEntities, config_defaultFraction);
                server.AcceptAsync();

                Console.WriteLine("[Server] Server is running at port {0}!", config_port);
                while (true)
                {
                    Console.ReadLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[Server] Something went wrong  while initializing server.");
            }
        }
    }
}
