using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Struggle
{
    class Base : Entity
    {
        
        public Base(Vector2f crd, uint m = 5) : base(crd,  m)
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
