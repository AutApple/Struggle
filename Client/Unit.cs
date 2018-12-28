using SFML.System;

namespace Struggle
{
    class Unit : Entity
    {
        uint level;
        public Unit(Vector2f crd, uint m) : base(crd, m)
        {
            level = 1;
        }

        public void Eat(Unit u)
        {

        }
    }
}
