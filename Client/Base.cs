
using SFML.Graphics;
using SFML.System;

namespace Struggle
{
    class Base : Entity
    {
        
        public Base(Vector2f crd, uint id, uint m) : base(crd, id, m)
        {
        }

        public override void Update()
        {
            base.Update();
            //generate unit
        }

        public override void Draw(RenderWindow window)
        {
            base.Draw(window);
        }
    }
}
