using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using System;


namespace Struggle
{
    [Serializable]
    class Fraction
    {
        public List<Entity> entities;
        
        public Color color;
        uint score;
 
        Game game;
        public Game Game
        {
            get
            {
                return game;
            }
        }
        public Fraction(Color c, Game game)
        {
            color = c;
            score = 0;
            entities = new List<Entity>();
            this.game = game;
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
        public void MoveEntities()
        {
            foreach(Entity e in entities)
                if (game.SelectedEntities.Contains(e.Id))
                    e.Move();
        }
        public void StopEntities()
        {
            foreach (Entity e in entities)
                if (game.SelectedEntities.Contains(e.Id))
                    e.Stop();
        }

        public void StopAllEntities()
        {
            foreach (Entity e in entities)
                    e.Stop();
        }

        public void RemoveEntity(Entity e)
        {
            entities.Remove(e); 
        }

        public void SelectEntity(Vector2f coords)
        {
            foreach(Entity e in entities)
            { 
                uint r = e.Mass;

                if (r > Utils.Distance(e.Position, coords))
                    if (game.SelectedEntities.Contains(e.Id))
                        game.SelectedEntities.Remove(e.Id);
                    else
                        game.SelectedEntities.Add(e.Id);
            }
        }

        public void AddScore(uint score)
        {
            this.score += score;
        }

        public void SelectEntityRectangle(RectangleShape rect)
        {
            foreach (uint id in game.SelectedEntities)
            {
                Console.Write(id + " ");
            }

            foreach (Entity e in entities)
                if (game.SelectedEntities.Contains(e.Id))
                    game.SelectedEntities.Remove(e.Id);
            
            float xMin = Math.Min(rect.Position.X, rect.Position.X + rect.Size.X);
            float yMin = Math.Min(rect.Position.Y, rect.Position.Y + rect.Size.Y);
            float xMax = Math.Max(rect.Position.X, rect.Position.X + rect.Size.X);
            float yMax = Math.Max(rect.Position.Y, rect.Position.Y + rect.Size.Y);

            foreach (Entity e in entities)
            {
                if (e.Position.X >= xMin && e.Position.X <= xMax
                   && e.Position.Y >= yMin && e.Position.Y <= yMax)
                    game.SelectedEntities.Add(e.Id);
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (Entity e in entities)
                e.Draw(window);
        }

        public void HandleCollisions()
        {
            game.HandleCollisions();
        }

        public void TargetEntitiesMovement(Vector2f coords)
        {
            foreach (Entity e in entities)
                if (game.SelectedEntities.Contains(e.Id))
                {
                    Console.WriteLine("TARGETING " + e.Id + " TO X = " + coords.X + "Y = " + coords.Y );
                    e.Target(coords);
                }
        }

        public void UpdateEntities()
        {
            for (int i = entities.Count - 1; i >= 0; --i)
            {
                entities[i].Update();
                entities[i].UpdateMovement();
            }
        }

        public void DeselectAll()
        {
            foreach (Entity e in entities)
                game.SelectedEntities.Remove(e.Id);
        }
    }
}
