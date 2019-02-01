using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Struggle
{
    class WorldMap
    {
        
        public string mapName { private set; get; }
        public int width { private set; get; }
        public int height { private set; get; }
         

        public WorldMap(int maxEntities, string path)
        {
            
            XmlTextReader reader = new XmlTextReader(path);
            while (reader.Read())
            {
                if(reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "world":
                            while (reader.MoveToNextAttribute())
                                switch (reader.Name)
                                {
                                    case "width":
                                        width = int.Parse(reader.Value);
                                        break;
                                    case "height":
                                        height = int.Parse(reader.Value);
                                        break;
                                    case "name":
                                        mapName = reader.Value;
                                        break;
                                }
                            Console.WriteLine("[World] World params: width = {0}, height = {1}, name = {2}", width, height, mapName);
                            break;
                        case "entity":
                            int fraction = -1;
                            int size = -1;
                            float x = -1;
                            float y = -1;
                            string type = "undef";
                            while (reader.MoveToNextAttribute())
                                switch (reader.Name)
                                {
                                    case "size":
                                        size = int.Parse(reader.Value);
                                        break;
                                    case "x":
                                        x = float.Parse(reader.Value);
                                        break;
                                    case "y":
                                        y = float.Parse(reader.Value);
                                        break;
                                    case "fraction":
                                        fraction = int.Parse(reader.Value);
                                        break;
                                    case "type":
                                        type = reader.Value;
                                        break;
                                }
                            
                            Console.WriteLine("\t| Entity (x : {0}, y : {1}, type : {2}, size : {3}, fraction : {4})", x, y, type, size, fraction);
                            break;
                    } 
                }
 
            }
        }


 

         
    }
}
