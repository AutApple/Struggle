﻿using System;
using SFML.Graphics;
using SFML.System;

namespace Struggle
{
    class Entity
    {
        protected Vector2f coords; 
        private uint mass;
        private Fraction fraction;

        private bool moving;
        private bool selected;

        private Vector2f destination;
        private float acceleration, speed;

        public Vector2f Position
        {
            get
            {
                return coords;
            }
        }

        public Entity(Vector2f crd, uint m = 5)
        {
            mass = m;
            coords = crd;
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

            window.Draw(shape); 
        }


        public uint GetMass()
        {
            return mass;
        }

        public virtual void Select()
        {
            selected = !selected;
        }

        public virtual void Update()
        {
            if (selected)
                coords += (destination - coords) * speed;
        }

        public void Move(Vector2f destination, float speed)
        {
            this.destination = destination;
            this.speed = speed;
        }
    }
}
