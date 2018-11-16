using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    static class CommandParser
    {
        public static void Parse(string command, ref ConcurrentDictionary<int, Connection> connections, ref WorldMap worldMap, ref Buffer buffer, ref Connection c)
        {
            string[] commandParts = command.ToLower().Split(' ');
            Console.WriteLine("[Command Execution Info] {0} executed {1}", c.nick, commandParts[0]);
            switch (commandParts[0])
            {
                case "!spawnentity":

                    if (commandParts.Length != 6)
                    {
                        buffer.SeekStart();
                        buffer.WriteInt8(9);
                        buffer.WriteString("usage: !spawnentity [x] [y] [size] [type] [fraction]");

                        c.socket.Send(buffer.GetBytes());
                    }
                    else
                    {
                     
                        try
                        {

                            int eid = worldMap.entityContainer.addEntity(ref connections, int.Parse(commandParts[1]), int.Parse(commandParts[2]), int.Parse(commandParts[3]), commandParts[4], int.Parse(commandParts[5]));
                           
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("entity spawned!");
                            c.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);

                            Console.WriteLine("[Command Execution Info] {0} spawned entity with id={1}, type={2}", c.nick, eid, commandParts[4]);
                        }
                        catch (Exception e)
                        {
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("usage: !spawnentity [x] [y] [size] [type] [fraction]");

                            c.socket.Send(buffer.GetBytes());
                        }
                    }
                    break;
                case "!removeentity":
                    if (commandParts.Length != 2)
                    {
                        buffer.SeekStart();
                        buffer.WriteInt8(9);
                        buffer.WriteString("usage: !removeentity [id]");

                        c.socket.Send(buffer.GetBytes());
                    } else
                    {
                        try
                        {
                            worldMap.entityContainer.removeEntity(ref c, ref connections, short.Parse(commandParts[1]));
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("entity removed!");

                            c.socket.Send(buffer.GetBytes());
                            Console.WriteLine("[Command Execution Info] {0} removed entity with id={1}", c.nick, short.Parse(commandParts[1]));
                        }catch(Exception e)
                        {
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("usage: !removeentity [id]");

                            c.socket.Send(buffer.GetBytes());
                        }
                    }
                    break;
                case "!fill":
                    if (commandParts.Length < 5)
                    {
                         
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("usage: !fill [n] [width] [height] [type] {[size] [fraction]}");

                            c.socket.Send(buffer.GetBytes());

                    }else
                    {
                        try
                        {
                            int number = int.Parse(commandParts[1]);
                            int w = int.Parse(commandParts[2]);
                            int h = int.Parse(commandParts[3]);
                            string type = commandParts[4];
                            int size = 1;
                            int fraction = 0;
                            if (type != "part")
                                if (commandParts.Length != 7)
                                {
                                    buffer.SeekStart();
                                    buffer.WriteInt8(9);
                                    buffer.WriteString("usage: !fill [n] [width] [height] [type] {[size] [fraction]}");

                                    c.socket.Send(buffer.GetBytes());
                                    return;
                                }
                                else
                                {
                                    size = int.Parse(commandParts[5]);
                                    fraction = int.Parse(commandParts[6]); 
                                }


                            Random r = new Random();
                            for (int i = 0; i < number; ++i)
                            {
                                int x = r.Next() % w;
                                int y = r.Next() % h;

                                worldMap.entityContainer.addEntity(ref connections, x, y, size, type, fraction);
               
                            }
                            buffer.WriteString("entities filled!");
                            Console.WriteLine("[Command Execution Info] {0} filled entities, type={1}", c.id, type);
                        }
                        catch(Exception e)
                        {
                            buffer.SeekStart();
                            buffer.WriteInt8(9);
                            buffer.WriteString("usage: !fill [n] [width] [height] [type] {[size] [fraction]}");

                            c.socket.Send(buffer.GetBytes());
                        }
                        
                    }
                    break;
                case "!motd":
                    string[] motd = File.ReadAllLines("motd.txt");

                    foreach (string str in motd)
                    {
                        buffer.SeekStart();
                        buffer.WriteInt8(10);
                        buffer.WriteString(str);

                        c.socket.Send(buffer.GetBytes());
                    }
                    break;
                case "!clearentities":
                    worldMap.entityContainer.Clear(ref c, ref connections);

                    buffer.SeekStart();
                    buffer.WriteInt8(9);
                    buffer.WriteString("entities cleared!");

                    c.socket.Send(buffer.GetBytes());
                    Console.WriteLine("[Command Execution Info] {0} cleared entities",  c.id);
                    break;

            }
        }
    }
}
