using SFML.System;

namespace Struggle
{
    class Unit : Entity
    {
        //uint level;
        public Unit(Vector2f crd, uint id, uint m) : base(crd, id, m)
        {
            //level = 1;
        }

        public virtual void Eat(Unit u)
        {
         
        }
    }
}
