using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class Entity
    {
        public float x {get; private set;}
        public float y { get; private set; }

        public int id { get; private set; }

        public int size { get; private set; }

        public string type { get; private set; }

        public int fraction { get; private set; }

        public Entity()
        {
            x = -1;
            y = -1;
            size = -1;
            type = "undef";
        }

        public Entity(int id, float x, float  y, int size, string type, int fraction)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.size = size;
            this.type = type;
            this.fraction = fraction;
        }

        public void ChangeSize(int size)
        {
            this.size = size;
        }

        public void ChangePosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


    }
}
