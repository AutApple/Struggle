using System;
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
        protected bool selected;

        private Vector2f destination, velocity;
        private float acceleration;

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

        public Vector2f GetCoords()
        {
            return coords;
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
            if (moving)
            {

            }
        }

        public void Move(Vector2f destination, Vector2f velocity, float acceleration)
        {
            moving = selected;
            this.destination = destination;
            this.velocity = velocity;
            this.acceleration = acceleration;
        }
    }
}
