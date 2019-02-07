using System;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Struggle
{
    class Entity
    {
       
        protected Vector2f coords; 
        protected uint mass;

        protected Fraction fraction;

        private bool selected;

        protected string titleString;
        protected Text title;

        private Vector2f destination;

        bool drawMovingLine;
        

        private uint id;

        public uint Id
        {
            get
            {
                return id;
            }
        }

        public Vector2f Position
        {
            get
            {
                return coords;
            }
        }
        public Fraction Fraction
        {
            get
            {
                return fraction;
            }
        }

        public uint Mass
        {
            get
            {
                return mass;
            }
        }
        
        public bool Selected
        {
            get
            {
                return selected;
            }
        }

        public Vector2f Destination
        {
            get
            {
                return destination;
            }
        }

        public bool DrawMoving
        {
            set
            {
                drawMovingLine = DrawMoving;
            }
            get
            {
                return drawMovingLine;
            }
        }

        public Entity(Vector2f coords, uint id, uint mass)
        {
            this.mass = mass;
            this.coords = coords;
            this.id = id;

            selected = false;

            drawMovingLine = false;
            /*title = new Text(titleString, new Font("C:\\Windows\\Fonts\\Arial.ttf"), 15);
            title.Color = Color.White;
            title.Position = coords;
            title.Origin = new Vector2f(title.GetLocalBounds().Width / 2, (title.GetLocalBounds().Height / 2) + 4);*/
        }
        
        public void Move()
        {
            Console.WriteLine("moving id " + id + " x = " + destination.X + " y = " + destination.Y);
            Command cmd = new Command(destination.X, destination.Y, id);

            Stream str = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(str, cmd);
            str.Seek(0, SeekOrigin.Begin);

            List<byte> buffer = new List<byte>();
            while (!(str.Position == str.Length))
            {
                buffer.Add((byte)str.ReadByte());
            }
            Console.WriteLine(buffer.Count);
            str.Close();
            fraction.Game.Client.Send(buffer.ToArray());

            drawMovingLine = true;
        }

        public void Stop()
        {
            Command cmd = new Command(-1, -1, id);

            Stream str = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(str, cmd);
            str.Seek(0, SeekOrigin.Begin);

            List<byte> buffer = new List<byte>();
            while (!(str.Position == str.Length))
            {
                buffer.Add((byte)str.ReadByte());
            }
            Console.WriteLine(buffer.Count);
            str.Close();
            fraction.Game.Client.Send(buffer.ToArray());

            drawMovingLine = false;
        }

        public void SetFraction(Fraction fraction)
        {
            this.fraction = fraction;
        }

        public virtual void Draw(RenderWindow window)
        {
            CircleShape shape = new CircleShape(mass);
            shape.FillColor = fraction.color;    
            shape.Position = new Vector2f(coords.X - (ushort)mass, coords.Y - (ushort)mass);

            if (fraction.Game.SelectedEntities.Contains(id))
            {
                CircleShape selection = new CircleShape(mass);
                selection.FillColor = Color.Transparent;
                selection.OutlineColor = Color.White;
                selection.OutlineThickness = 1;
                selection.Position = new Vector2f(coords.X - (ushort)mass, coords.Y - (ushort)mass);
                window.Draw(selection);
            }            
            window.Draw(shape);
            //window.Draw(title);
        }
        

        public virtual void UpdateMovement()
        {
            /*if (movingFlag && Utils.Distance(coords, destination) > mass / 2)
            {
                coords += ((destination - coords) / (float)Math.Sqrt(Math.Pow((destination - coords).X, 2) + Math.Pow((destination - coords).Y, 2))) * speed;
                fraction.HandleCollisions();
            }
            else if (Utils.Distance(coords, destination) <= mass / 2 && movingFlag)
            {
                movingFlag = false;
            }*/
        }
        public virtual void Update()
        {
           /* title.Origin = new Vector2f(title.GetLocalBounds().Width / 2, (title.GetLocalBounds().Height / 2) + 4);
            title.Position = coords;
            title.DisplayedString = titleString;*/
        }

        public void Target(Vector2f destination)
        {
                this.destination = destination;
        }
    }
}
