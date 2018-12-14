using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struggle
{
    class Entity
    {
        
        
        private Vector2i coords; 
        public Vector2i Coords
        {
            get { return coords; }
            set { coords = value; }
        }
        private uint radius;
        public uint Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public uint fraction;
        public Color colors;  
        public Entity(Vector2i crd, uint f, uint r = 5)
        {
            radius = r;
            coords = crd;
            fraction = f;

        }

        public void Draw(RenderWindow window)
        {
            CircleShape shape = new CircleShape(radius, 40);
            
            switch (fraction)
            {
                case 0:
                    shape.FillColor = Color.Blue;
                    break;
                case 1:
                    shape.FillColor = Color.Green;
                    break;
                case 2:
                    shape.FillColor = Color.Red;
                    break;
                case 3:
                    shape.FillColor = Color.Magenta;
                    break;
                case 4:
                    shape.FillColor = new Color(255, 255, 255, 100);
                    break;
                
            }
            
            shape.Position = new Vector2f(coords.X - (ushort)radius, coords.Y - (ushort)radius);
            window.Draw(shape);

        }
    }
}
