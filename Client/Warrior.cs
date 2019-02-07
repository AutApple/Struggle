using SFML.Graphics;
using SFML.System;
using System;

namespace Struggle
{
    class Warrior : Unit
    { 
        public Warrior(Vector2f crd, uint id, uint m) : base(crd, id, m)
        {
            titleString = "Warrior";
        }

        public override void Draw(RenderWindow app)
        {
            base.Draw(app);
        }

        public override void Update()
        {
            base.Update();    
        }

        public override void Eat(Unit u)
        {
            base.Eat(u);
            mass += u.Mass;
            u.Fraction.RemoveEntity(u);
            fraction.AddScore(1);
        }
    }
}
