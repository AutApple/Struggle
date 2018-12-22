using SFML.Graphics;
using SFML.System;

namespace Struggle
{
    class Entity
    {
        protected Vector2f coords; 
        private uint mass;
        private Fraction fraction;
        
        private float speed, acelleration; 

        public Entity(Vector2f crd, Fraction f, uint m = 5)
        {
            mass = m;
            coords = crd;
            fraction = f;
        }

        public virtual void Draw(RenderWindow window)
        {
            CircleShape shape = new CircleShape(mass, 40);
            shape.FillColor = fraction.color;    
            shape.Position = new Vector2f(coords.X - (ushort)mass, coords.Y - (ushort)mass);
            window.Draw(shape);
        }
    }
}
