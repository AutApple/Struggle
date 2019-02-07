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
        private List<FractionCommon> fractions;
        private int fractionId;

        public List<FractionCommon> Fractions
        {
            get
            {
                return fractions;
            }
        }

        public int FractionId
        {
            get
            {
                return fractionId;
            }
        }

        public GameCommon()
        {
            fractions = new List<FractionCommon>();
        }

        public void AddFraction(FractionCommon f)
        {
            fractions.Add(f);
        }

        public void SetFractionId(int id)
        {
            fractionId = id;
        }

        public void Test()
        {
            Console.WriteLine("hi");
        }
    }
}
