using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    [Serializable]
    public class Command
    {
        float dx, dy;
        uint id;

        public float Dx
        {
            get
            {
                return dx;
            }
        }

        public float Dy
        {
            get
            {
                return dy;
            }
        }

        public uint Id
        {
            get
            {
                return id;
            }
        }

        public Command(float dx, float dy, uint id)
        {
            this.dx = dx;
            this.dy = dy;
            this.id = id;
        }
    }
}
