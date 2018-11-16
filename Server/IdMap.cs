using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class IdMap
    {
        private bool[] ids;
        public IdMap(int size)
        {
            ids = new bool[size];
            for (int i = 0; i < size; i++)
                ids[i] = false;
        } 
        public int getLength()
        {
            return ids.Length;
        }
        public int getId()
        {
            for (int i = 0; i < ids.Length; ++i)
                if (ids[i] == false)
                {
                    ids[i] = true;
                    return i;
                }
            return -1;
        }

        public void releaseId(int id)
        {
            ids[id] = false;
        }
    }
}
