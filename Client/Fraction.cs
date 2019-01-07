using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using System;

namespace Struggle
{
    class Fraction
    {
        public List<Entity> entities;
        uint id;
        public Color color;
        uint score;
 
        Game gm;

        public Fraction(Color c, uint id, Game gm)
        {
            color = c;
            score = 0;
            this.id = id;
            entities = new List<Entity>();
            this.gm = gm;
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }
        public void AddEntity(Entity e)
        {
            e.SetFraction(this);
            entities.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            entities.Remove(e); 
        }

        public void SelectEntity(Vector2f coords)
        {
            foreach(Entity e in entities)
            {
                float dx = Math.Abs(e.Position.X - coords.X);
                float dy = Math.Abs(e.Position.Y - coords.Y);

                uint r = e.Mass;
                
                if(r * r > dx*dx + dy*dy)
                    e.Select();
            }
        }

        public void SelectEntityRectangle(RectangleShape rect)
        {
            float xMin = Math.Min(rect.Position.X, rect.Position.X + rect.Size.X);
            float yMin = Math.Min(rect.Position.Y, rect.Position.Y + rect.Size.Y);
            float xMax = Math.Max(rect.Position.X, rect.Position.X + rect.Size.X);
            float yMax = Math.Max(rect.Position.Y, rect.Position.Y + rect.Size.Y);

            foreach (Entity e in entities)
            {
                if (e.Position.X >= xMin && e.Position.X <= xMax
                   && e.Position.Y >= yMin && e.Position.Y <= yMax)
                    e.Select();
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (Entity e in entities)
                e.Draw(window);
        }

        public void HandleCollisions()
        {
            gm.HandleCollisions();
        }

        public void UpdateEntitiesMovement(Vector2f coords)
        {
            foreach (Entity e in entities)
                e.Target(coords, 2f / e.Mass);
        }

        public void UpdateEntities()
        {
            foreach(Entity e in entities)
                e.Update();
        }
    }
}
