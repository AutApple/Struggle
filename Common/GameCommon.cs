using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    [Serializable]
    public class GameCommon
    {
        public List<FractionCommon> fractions;
        public GameCommon()
        {
            fractions = new List<FractionCommon>();
        }
    }
}
