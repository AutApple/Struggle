using SFML.System;

namespace Struggle
{
    class Unit : Entity
    {
        uint level;
        public Unit(Vector2f crd, Fraction f, uint m) : base(crd, f, m)
        {
            level = 1;
        }

        public void Eat(Unit u)
        {

        }
    }
}
