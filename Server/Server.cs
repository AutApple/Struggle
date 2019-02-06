﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace Struggle
{
    class Server
    {
        Socket socket;
        ConcurrentDictionary<int, Connection> connections;
        IdMap connectionIds;
        
        public Server(short port, int maxPlayers, int maxEntities, int defaultFraction)
        {
            connections = new ConcurrentDictionary<int, Connection>();
            connectionIds = new IdMap(maxPlayers);
 
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
                    c.SetupConnection(ref connections, ref connectionIds);

                    GameCommon gc = new GameCommon();
                    FractionCommon fr = new FractionCommon();

                    fr.entities.Add(new EntityCommon(32, 32, 16));
                    gc.fractions.Add(fr);

                    Stream str = new MemoryStream();


                    BinaryFormatter bf = new BinaryFormatter();
                    bf.AssemblyFormat = FormatterAssemblyStyle.Simple;
                    bf.Serialize(str, gc);

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
                    //есть что-то, что надо обработать 
                    Buffer buffer = new Buffer();
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
                }catch(Exception)
                {
                    return 0;
                }
            });

        }
        
 
    }
}
