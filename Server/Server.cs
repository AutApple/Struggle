using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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
                    c.SetupTimeout(ref connections, ref connectionIds);
                     
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
                            Console.WriteLine("timeout flag");
                            //timeout
                            c.timeoutFlag = true;
                            break;
                        case 1:
                            //another timeout
                            buffer.SeekStart();
                            buffer.WriteInt8(1);
                            c.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);
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
