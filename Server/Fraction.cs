using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Struggle
{
    class Fraction
    {
        List<Entity> entities;
        ConcurrentDictionary<int, Connection> connections;

        public List<Entity>  Entities
        {
            get
            {
                return entities;
            }
        }

        public Fraction(ref ConcurrentDictionary<int, Connection> connections)
        {
            entities = new List<Entity>();
            this.connections = connections;
        }

        public void AddEntity(Entity e)
        {
            e.SetConnections(ref connections);
            entities.Add(e);
        }

        public void UpdateEntities()
        {
            foreach(Entity e in entities)
            {
                e.Update();
            }
        }
    }
}