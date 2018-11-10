using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Eternal_Struggle_Server
{
    static class Configuration
    {
        static XmlTextReader xmlReader;
        public static Dictionary<string, string> paramList;

   /*     public static int maxEntities
        {
            get;
            private set;
        }//1000
        public static int maxPlayers
        {
            get;
            private set;
        }//25
        public static int port
        {
            get;
            private set;
        }//7777
        
    */


        private static void DisplayConfig()
        {
            foreach(string par in paramList.Keys)
            {
                string s;
                paramList.TryGetValue(par, out s);
                Console.WriteLine("\t| {0} = {1}", par, s);
            }
  
        }
        public static void Setup()
        {
            paramList = new Dictionary<string, string>();
        
            try
            {
                xmlReader = new XmlTextReader("config.xml");
            }
            catch(Exception e)
            {
                Console.WriteLine("[Configuration File] unable to find configuration file. Using default config");

                paramList.Add("port", "7777");
                paramList.Add("maxPlayers", "25");
                paramList.Add("maxEntities", "1000");

                DisplayConfig();
                return;
            }

            Console.WriteLine("[Configuration File] Configuration file loaded!");
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if(xmlReader.Name == "config")
                        {
                            while (xmlReader.MoveToNextAttribute())
                                paramList.Add(xmlReader.Name, xmlReader.Value);
                        }
                        break;
                }
           }
            DisplayConfig(); 
        }




    }
}
