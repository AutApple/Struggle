using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class EntityContainer
    {
        ConcurrentDictionary<int, Entity> entities;
        IdMap entityIdMap;
       // public float xBuffer, yBuffer;
     //   public short sizeBuffer;



        public EntityContainer(int maxEntities)
        {
            entities = new ConcurrentDictionary<int, Entity>();
            entityIdMap = new IdMap(maxEntities);
        }
         
        public int addEntity(ref ConcurrentDictionary<int, Connection> connections, float x, float y, int size, string type, int fraction)
        {
            int id = entityIdMap.getId();
            Entity e = new Entity(id, x, y, size, type, fraction);
            entities.TryAdd(id, e);

            Buffer buffer = new Buffer();
            foreach (Connection client in connections.Values)
            {
                if (client != null)
                {
                    

                    buffer.SeekStart();
                    buffer.WriteInt8(7);

                    buffer.WriteInt16((short)id);
                    buffer.WriteFloat(e.x);
                    buffer.WriteFloat(e.y);
                    buffer.WriteInt16((short)e.size);
                    buffer.WriteString(e.type);
                    buffer.WriteInt16((short)e.fraction);
                    client.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);

                   
                }
            }
            buffer.Clear();
            return id;
        }


        public int addEntity(float x, float y, int size, string type, int fraction)
        {
            int id = entityIdMap.getId();
            Entity e = new Entity(id, x, y, size, type, fraction);
            entities.TryAdd(id, e);

            return id;
        }



        public void Clear(ref Connection c, ref ConcurrentDictionary<int, Connection> connections)
        {
            for (int i = 0; i < entityIdMap.getLength(); i++)
            {
                removeEntity(ref c, ref connections, i);
            }
        }


        public void removeEntity(ref Connection c, ref ConcurrentDictionary<int, Connection> connections, int id)
        {
            Entity e;
            entities.TryRemove(id, out e);
            entityIdMap.releaseId(id);
            Buffer buffer = new Buffer();
            foreach (Connection client in connections.Values)
            {
                if (client != null)
                {
                    
                    buffer.SeekStart();
                    buffer.WriteInt8(8);
                    buffer.WriteInt16((short)id);
                    client.socket.Send(buffer.GetBytes(), 0, buffer.Tell(), 0);
                   

                }
            }
            buffer.Clear();
        }

        public void removeEntity(int id)
        {
            Entity e;
            entities.TryRemove(id, out e);
            entityIdMap.releaseId(id);
        }

        public void updateEntityPosition(ref Connection c, ref ConcurrentDictionary<int, Connection> connections, short id, float x, float y)
        {
            foreach (Entity e in entities.Values)
            {
                if (e.id == id)
                {
                    if (e.fraction == c.fraction)
                    {
                        e.ChangePosition(x, y);
                    }   
                }
            }
            Buffer bf = new Buffer();
            foreach (Connection client in connections.Values)
            {
                if (client.id != c.id && c.fraction != client.fraction && client.socket.Connected && client.socket != null)
                {
                    
                    bf.SeekStart();
                    bf.WriteInt8(11);
                    bf.WriteInt16(id);
                    bf.WriteFloat(x);
                    bf.WriteFloat(y);
                   
                    client.socket.Send(bf.GetBytes(), 0, bf.Tell(), 0);
                   
                }
            }
            bf.Clear();
        }
        public void updateEntitySize(ref Connection c, ref ConcurrentDictionary<int, Connection> connections, short id, short size)
        {
            foreach (Entity e in entities.Values)
            {
                if (e.id == id)
                {
                    if (e.fraction == c.fraction)
                    {
                        e.ChangeSize(size);
                    }
                }
            }
            Buffer bf = new Buffer();
            foreach (Connection client in connections.Values)
            {
                if (client.id != c.id && c.fraction != client.fraction && client.socket.Connected && client.socket != null)
                {

                    bf.SeekStart();
                    bf.WriteInt8(12);
                    bf.WriteInt16(id);
                    bf.WriteInt16(size);
                   

                    client.socket.Send(bf.GetBytes(), 0, bf.Tell(), 0);

                }
            }
            bf.Clear();
        }
        public void sendClient(ref Connection c)
        {
            Buffer buff = new Buffer();
            if(!entities.IsEmpty)
             foreach(Entity e in entities.Values)
             {
                buff.SeekStart();
                buff.WriteInt8(7);

                buff.WriteInt16((short)e.id);
                buff.WriteFloat(e.x);
                buff.WriteFloat(e.y);
                buff.WriteInt16((short)e.size);
                buff.WriteString(e.type);
                buff.WriteInt16((short)e.fraction);

                c.socket.Send(buff.GetBytes(), 0, buff.Tell(), 0);
             }


            buff.Clear();
        }
    }
}
