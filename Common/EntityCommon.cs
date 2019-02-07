using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    [Serializable]
    public class EntityCommon
    {
        public float x, y;
        public uint mass;
        public bool moving;
        public float destinationX, destinationY;
        public float speed;
        public bool selected;
        public uint id;

        public EntityCommon(float x, float y, uint mass, uint id)
        {
            this.x = x;
            this.y = y;
            this.mass = mass;
            this.id = id;

            moving = false;
            destinationX = -1;
            destinationY = -1;
            speed = 0;
            selected = false;
        }
    }
}
