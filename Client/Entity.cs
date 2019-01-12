using System;
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
        
        public Entity(Vector2f crd, uint m = 5)
        {
            mass = m;
            coords = crd;
            selected = false;
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
            if (selected)
            {
                coords += ((destination - coords) / (float)Math.Sqrt(Math.Pow((destination - coords).X, 2) + Math.Pow((destination - coords).Y, 2))) * speed;
                fraction.HandleCollisions();
            }
            title.Position = coords;
        }

        public void Target(Vector2f destination, float speed)
        {
            this.destination = destination;
            this.speed = speed;
        }
    }
}
