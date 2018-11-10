/*[OP]
[Argument]

incoming ops:
	0 - -player responds timeout
	1 - player requests timeout
	2 - player sends message directly to server
	3 - player changes nickname


    [Entities group]
	4 - player changes  x entity buffer
	5 - player changes y entity buffer
    6 - player changes size entity buffer
	7 - player creates entity 
	8 - player updates entity



outcoming ops:
	  0 - server sends test package to check client's responsability
	  1 - server's response to the client's test package
	  2 - connection id stuff
	  3 - server sends text message to the client
    [Entities group]
	  	4 - server changes  x entity buffer
		5 - server changes y entity buffer
		6 - server changes size entity buffer	
	    7 - server sends request to create entity with specified id
	    8 -  server sends request to update coordinates  of entity with specified id

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;

namespace Eternal_Struggle_Server
{
    class Server
    {

        Socket socket;



        ConcurrentDictionary<int, Connection> connections;
        IdMap connectionIds;


        EntityContainer entc;

        WorldMap worldMap;

        int defaultFraction;


        public Server(short port, int maxPlayers, int maxEntities, int defaultFraction)
        {



            this.defaultFraction = defaultFraction;




            connections = new ConcurrentDictionary<int, Connection>();
            connectionIds = new IdMap(maxPlayers);

            worldMap = new WorldMap(maxEntities, "map.xml");
            entc = worldMap.entityContainer;


            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(maxPlayers);



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


                    c.SetupTimeout(ref connections, ref connectionIds);

                    Buffer buff = new Buffer();
                    buff.SeekStart();
                    buff.WriteInt8(2);

                    buff.WriteInt16((short)connectionId);
                    //  buff.WriteInt16((short)worldMap.width);
                    //    buff.WriteInt16((short)worldMap.height);
                    //   buff.WriteString(worldMap.mapName);

                    connection.Send(buff.GetBytes(), 0, buff.Tell(), 0);
                    buff.SeekStart();
                    buff.WriteInt8(3);

                    buff.WriteInt16((short)defaultFraction);

                    connection.Send(buff.GetBytes());



                    buff.SeekStart();
                    buff.WriteInt8(4);

                    buff.WriteInt16((short)worldMap.width);
                    buff.WriteInt16((short)worldMap.height);
                    buff.WriteString(worldMap.mapName);

                    connection.Send(buff.GetBytes(), 0, buff.Tell(), 0);



                    string[] motd = File.ReadAllLines("motd.txt");

                    foreach (string str in motd)
                    {
                        buff.SeekStart();
                        buff.WriteInt8(10);
                        buff.WriteString(str);

                        connection.Send(buff.GetBytes(), 0, buff.Tell(), 0);
                    }
                    buff.Clear();


                    //worldMap.entityContainer.sendClient(ref c);
                    entc.sendClient(ref c);

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
                    //есть что-то, что надо обработать 

                    Buffer buffer = new Buffer();
                    Buffer message = new Buffer(c.data);

                     
                    int mCode = message.ReadU8();
               
                    switch (mCode)
                    {
                        case 0:
                   
                            //timeout
                            c.timeoutFlag = true;
                            break;
                        case 1:
                        

                            //another timeout
                            buffer.SeekStart();
                            buffer.WriteInt8(1);
 
                             
                            c.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);
                            
                            break;
                        case 2: //send message to the server
                            Console.WriteLine(message.ReadString());
                            break;
                        case 3:
                            //nickname stuff
                            string nick = message.ReadString();
                            c.ChangeNickname(nick);

                            Console.WriteLine("Player {0} changed his nickname to {1}", c.id, c.nick);
                            break;
                        case 4:
                           
                            int connectionFraction = message.ReadU16();

                             
                            c.SetFraction(connectionFraction);
                        
                            buffer.SeekStart();
                            buffer.WriteInt8(3);
                            buffer.WriteInt16((short)connectionFraction);


                            
                            c.socket.Send(buffer.GetBytes());
                             
                            break;
                        //////////////////
                        //              //
                        // [FREE SPACE] //
                        //              //
                        //////////////////
 
               
                        case 7:

                            worldMap.entityContainer.addEntity(ref connections, message.ReadFloat(), message.ReadFloat(), message.ReadU16(), message.ReadString(), message.ReadU16());
                            
                            break;

                        case 8:
                           //update
                         
                            worldMap.entityContainer.updateEntityPosition(ref c, ref connections, message.ReadU16(), message.ReadFloat(), message.ReadFloat());
                             
                            break;
                        
                        case 9:
                            string chatMessage = message.ReadString();
                            if (chatMessage.Length != 0) {
                                if (chatMessage[0] !='!')
                                {
                                    Console.WriteLine("[{0}]: {1}", c.nick, chatMessage);
                                    string chatNickname = "[" + c.nick + "]: ";
                                    foreach (Connection client in connections.Values)
                                    {
                                        if (c != null)
                                        {
                                            buffer.SeekStart();
                                            buffer.WriteInt8(10);
                                            buffer.WriteString(chatNickname + chatMessage);
                                            client.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);

                                        }
                                    }
                                }else
                                    CommandParser.Parse(chatMessage, ref connections, ref worldMap, ref buffer, ref c);

                                
                               
                            }
                            break;
                        //////////////////
                        //              //
                        // [FREE SPACE] //
                        //              //
                        //////////////////
                        case 11:
                             worldMap.entityContainer.removeEntity(ref c, ref connections, message.ReadU16());
                            break;
                        case 12:
                            //update entity size
                            worldMap.entityContainer.updateEntitySize(ref c, ref connections, message.ReadU16(), message.ReadU16());
                            break;
                        default:
                                
                            break;

                    }

                    
                    message.Clear();
                    buffer.Clear();


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
                }catch(Exception e)
                {
                    return 0;
                }
            });

        }
        
 
    }
}
