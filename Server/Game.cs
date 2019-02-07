using Struggle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Struggle
{
    class Game
    {
        List<Fraction> fractions;
        bool gameEnd;
        ConcurrentDictionary<int, Connection> connections;

        public List<Fraction> Fractions
        {
            get
            {
                return fractions;
            }
        }
        public Game(ref ConcurrentDictionary<int, Connection> connections)
        {
            fractions = new List<Fraction>();
            gameEnd = false;

            this.connections = connections;
        }

        public void Run()
        {
            while (!gameEnd)
            {
                Update();
                foreach(Connection c in connections.Values)
                {
                    GameCommon gc = GetInfo();
                    MemoryStream str = new MemoryStream();

                    gc.SetFractionId(c.id);


                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(str, gc);

                    str.Seek(0, SeekOrigin.Begin);

                    List<byte> buffer = new List<byte>();
                    while (!(str.Position == str.Length))
                    {
                        buffer.Add((byte)str.ReadByte());
                    }

                    str.Close();

                    c.socket.Send(buffer.ToArray());
                }
                    
                Thread.Sleep(1);
            }
        }

        public void Update()
        {
            foreach(Fraction f in fractions)
            {
                f.UpdateEntities();
            }
        }

        public void AddFraction(Fraction f)
        {
            fractions.Add(f);
        }

        public GameCommon GetInfo()
        {
            GameCommon gc = new GameCommon();

            foreach(Fraction f in fractions)
            {
                FractionCommon fc = new FractionCommon();
                foreach(Entity e in f.Entities)
                {
                    EntityCommon ec = new EntityCommon(e.X, e.Y, e.Mass, e.id);
                    ec.moving = e.Moving;
                    ec.speed = e.Speed;
                    fc.entities.Add(ec);
                }
                gc.AddFraction(fc);
            }

            return gc;
        }
    }
}
