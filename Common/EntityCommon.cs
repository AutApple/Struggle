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
        public int x, y;
        public uint mass;

        public EntityCommon(int x, int y, uint mass)
        {
            this.x = x;
            this.y = y;
            this.mass = mass; 
        }
    }
}
