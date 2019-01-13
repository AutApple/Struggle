﻿using System;
using SFML.Graphics;
using SFML.System;


namespace Struggle
{
    class Entity
    {
        protected Vector2f coords; 
        protected uint mass;

        protected Fraction fraction;

        private bool selected;

        protected Text title;

        private Vector2f destination;
        private float acceleration, speed;

        private bool targetLock; 

        public Vector2f Position
        {
            get
            {
                return coords;
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
        public Entity(Vector2f crd, uint m = 5)
        {
            mass = m;
            coords = crd;
            selected = false;
            targetLock = false;
        }

        public void Lock()
        {
            targetLock = true;
        }

        public void Unlock()
        {
            targetLock = false;
        }
        public void Deselect()
        {
            selected = false;
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

            if (selected)
            {
                CircleShape selection = new CircleShape(mass);
                selection.FillColor = Color.Transparent;
                selection.OutlineColor = Color.White;
                selection.OutlineThickness = 1;
                selection.Position = new Vector2f(coords.X - (ushort)mass, coords.Y - (ushort)mass);
                window.Draw(selection);
            }

            if (targetLock)
            {
                Vertex[] targetLine = new Vertex[2];

                targetLine[0] = new Vertex(coords);
                targetLine[1] = new Vertex(destination);

                
                window.Draw(targetLine, 0, 2, PrimitiveType.Lines);
            }
            
            window.Draw(shape);
            window.Draw(title);
        }

        public void InvertSelection()
        {
            selected = !selected;
        }
        
        public void Select()
        {
            selected = true;
        }

        public virtual void Update()
        {
            if ((selected || targetLock) && Utils.Distance(coords, destination) > mass / 2)
            {
                coords += ((destination - coords) / (float)Math.Sqrt(Math.Pow((destination - coords).X, 2) + Math.Pow((destination - coords).Y, 2))) * speed;
                fraction.HandleCollisions();
                title.Position = coords;
            }
            else if (Utils.Distance(coords, destination) <= mass/2 && targetLock)
            {
                targetLock = false;
            }
        }

        public void Target(Vector2f destination, float speed)
        {
            if (!targetLock)
            {
                this.destination = destination;
                this.speed = speed;
            }
        }
    }
}
