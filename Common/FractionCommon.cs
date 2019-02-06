using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    [Serializable]
    public class FractionCommon
    {
        public List<EntityCommon> entities;

        public FractionCommon()
        {
            entities = new List<EntityCommon>();
        }
    }
}
