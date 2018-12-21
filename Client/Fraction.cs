using System.Collections.Generic;
using SFML.Graphics;

namespace Struggle
{
    class Fraction
    {
        List<Entity> entities;
        uint id;
        public Color color;
        uint score;

        public Fraction(Color c, uint id)
        {
            color = c;
            score = 0;
            this.id = id;
            entities = new List<Entity>();
        }

        public void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            entities.Remove(e); 
        }

        public void Draw(RenderWindow window)
        {
            foreach (Entity e in entities)
                e.Draw(window);
        }
    }
}
