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
            Server server = new Server(7777, 4, 100, 0);
            server.AcceptAsync();
            
            while (true)
                Console.ReadLine();   
        }
    }
}
