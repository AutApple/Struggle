using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Threading;

namespace Struggle
{
    class Server
    {
        Socket socket;
        ConcurrentDictionary<int, Connection> connections;
        IdMap connectionIds;

        Game game;

        uint entitiesCount; //for id stuff

        public Server(short port, int maxPlayers, int maxEntities, int defaultFraction)
        {
            entitiesCount = 4;
            connections = new ConcurrentDictionary<int, Connection>();
            connectionIds = new IdMap(maxPlayers);
 
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(maxPlayers);

            game = new Game(ref connections);

            /* Initialize Map */

            Fraction fr1 = new Fraction(ref connections);
            Fraction fr2 = new Fraction(ref connections);
            Fraction fr3 = new Fraction(ref connections);
            Fraction fr4 = new Fraction(ref connections);

            fr1.AddEntity(new Entity(32, 32, 16, 0));
            fr1.AddEntity(new Entity(64, 64, 32, 1));

            fr2.AddEntity(new Entity(608, 32, 16, 2));
            fr2.AddEntity(new Entity(576, 64, 32, 3));

            fr3.AddEntity(new Entity(32, 448, 16, 4));
            fr3.AddEntity(new Entity(64, 416, 32, 5));

            fr4.AddEntity(new Entity(608, 448, 16, 6));
            fr4.AddEntity(new Entity(576, 416, 32, 7));

            game.AddFraction(fr1);
            game.AddFraction(fr2);
            game.AddFraction(fr3);
            game.AddFraction(fr4);

            Thread gameThread = new Thread(game.Run);
            gameThread.Start();
        }
        public byte[] SerializeGame(int cid)
        {
            GameCommon gameClient = game.GetInfo();

            gameClient.SetFractionId(cid);

            Stream str = new MemoryStream();


            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(str, gameClient);

            str.Seek(0, SeekOrigin.Begin);

            List<byte> buffer = new List<byte>();
            while (!(str.Position == str.Length))
            {
                buffer.Add((byte)str.ReadByte());
            }

            str.Close();

            return buffer.ToArray();
        }
        public async void AcceptAsync()
        {
            if (socket != null)
            {
                Socket connection = await AsyncTask();
                if (connection != null)
                {
                    Console.WriteLine("Connection from " + (connection.RemoteEndPoint as IPEndPoint));

                    int connectionId = connectionIds.getId();
                    Connection c = new Connection(connection, connectionId, connectionId);

                    connections.TryAdd(connectionId, c);
                    c.SetupConnection(ref connections, ref connectionIds);

                    GameCommon gameClient = game.GetInfo();

                    gameClient.SetFractionId(connectionId);

                    Stream str = new MemoryStream();


                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(str, gameClient);

                    str.Seek(0, SeekOrigin.Begin);

                    List<byte> buffer = new List<byte>();
                    while (!(str.Position == str.Length))
                    {
                        buffer.Add((byte)str.ReadByte());
                    }

                    str.Close();

                    
                    c.socket.Send(buffer.ToArray());
                    RecieveDataAsync(c);
                }
                AcceptAsync();
            }
        }

        public Task<Socket> AsyncTask()
        {
            return Task.Run(() =>
            {
                Socket connection = socket.Accept();
                return connection;
            });
        }

        public async void RecieveDataAsync(Connection c)
        {
            if (c.socket != null)
            {
                int size = await RecieveDataAsyncTask(c);
       
                if (size != 0)
                {
                     
                    MemoryStream str = new MemoryStream(c.data, 0, size);
                    BinaryFormatter bf = new BinaryFormatter();
                    Command cmd = bf.Deserialize(str) as Command;
                    str.Close();

                    Console.WriteLine("Moving command, target x: " + cmd.Dx + ", target y: " + cmd.Dy + ", entity id: " + cmd.Id);
                    foreach(Entity e in game.Fractions[c.id].Entities)
                    {
                        if(e.id == cmd.Id)
                        {
                            //move it  
                            if (cmd.Dx != -1 && cmd.Dy != -1)
                            {
                                e.Stop();
                                e.Target(cmd.Dx, cmd.Dy, 5f / e.Mass);
                                e.Move();
                            }
                            else
                            {
                                e.Stop();
                            }
                        }
                    }
                   
                    /*Buffer buffer = new Buffer();
                    Buffer message = new Buffer(c.data);

                    int mCode = message.ReadU8();
               
                    switch (mCode)
                    {
                        case 0:
                            
                            break;
                        default:
                            break;
                    }
   
                    message.Clear();
                    buffer.Clear();*/
                    RecieveDataAsync(c);
                }
                else
                {
                    c.Release();
                    c.socket.Close();
                    Console.WriteLine("{0}(id={1}) disconnected!", c.nick, c.id);
                }
            }
           

        }

        public Task<int> RecieveDataAsyncTask(Connection c)
        {
            return Task.Run(() =>
            {
                try
                {
                    int size = c.socket.Receive(c.data);
                    return size;
                }catch(Exception)
                {
                    return 0;
                }
            });

        }
        
 
    }
}
