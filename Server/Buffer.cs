using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class Buffer
    {
        MemoryStream ms;

        BinaryWriter bw;
        BinaryReader br;

        public Buffer()
        {
            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        public Buffer(byte[] data)
        {
            ms = new MemoryStream(data);
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        public void WriteString(string str)
        {
            foreach (char c in str)
                bw.Write(c);

            bw.Write("\0");
        }

        public void WriteInt8(byte data)
        {
            bw.Write(data);
        }

        public void WriteInt16(short data)
        {
            bw.Write(data);
        }
        
        public void WriteFloat(float data)
        {
            bw.Write(data);
        }
        public byte ReadU8()
        {
            return br.ReadByte();
        }

        public short ReadU16()
        {
            return br.ReadInt16();
        }

        public string ReadString()
        {
            string str = "";
            char c;
            while ((int)(c = br.ReadChar()) != 0)
                str += c;
            return str;
        }
        
        public float ReadFloat()
        {
            return br.ReadSingle();
        }

        public void SeekStart()
        {
            ms.Seek(0, SeekOrigin.Begin);
        }

        public int Tell()
        {
            return (int)ms.Position;
        }


        public byte[] GetBytes()
        {
            return ms.ToArray();
        }

        public void Clear()
        {
            bw.Close();
            bw.Dispose();

            ms.Close();
            ms.Dispose();
        }

    }
}
