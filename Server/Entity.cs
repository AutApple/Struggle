using System;
using System.Collections.Concurrent;

namespace Struggle
{
    class Entity
    {
        private float x, y;
        private uint mass;

        public uint id;

        private bool moving;
        public bool Moving
        {
            get
            {
                return moving;
            }
        }

        private float dx, dy;
        float speed;

        ConcurrentDictionary<int, Connection> connections;

        public float X
        {
            get
            {
                return x;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
        }
        public float Speed
        {
            get
            {
                return speed;
            }
        }
        public uint Mass
        {
            get
            {
                return mass;
            }
        }

        public Entity(float x, float y, uint mass, uint id)
        {
            this.mass = mass;
            this.x = x;
            this.y = y;
            this.id = id;
        }
     
        public void SetConnections(ref ConcurrentDictionary<int, Connection> connections)
        {
            this.connections = connections;
        }

        public void Move()
        {
            moving = true;
        }

        public void Stop()
        {
            moving = false;
            speed = 0;
        }

        public void Target(float dx, float dy, float speed)
        {
            this.dx = dx;
            this.dy = dy;
            this.speed = speed;
        }
        public void Update()
        {
            if (moving && Utils.Distance(x, y, dx, dy) > mass / 2)
            {
                x += ((dx - x) / (float)Math.Sqrt(Math.Pow(dx - x, 2) + Math.Pow(dy - y, 2))) * speed;
                y += ((dy - y) / (float)Math.Sqrt(Math.Pow(dx - x, 2) + Math.Pow(dy - y, 2))) * speed;
            }
            else if (Utils.Distance(x, y, dx, dy) <= mass / 2 && moving)
            {
                moving = false;
            }
        }
    }
}