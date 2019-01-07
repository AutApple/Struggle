using SFML.Graphics;
using SFML.System;
using System;

namespace Struggle
{
    class Warrior : Unit
    {
        
        public Warrior(Vector2f crd, uint m) : base(crd, m)
        {
            title = new Text("Warrior", new Font("C:\\Windows\\Fonts\\Arial.ttf"), 15);
            title.Color = Color.White;
            title.Position = coords;
            title.Origin = new Vector2f(title.GetLocalBounds().Width / 2, (title.GetLocalBounds().Height / 2) + 4);
        }

        public override void Draw(RenderWindow app)
        {
            base.Draw(app);
            
        }

        public override void Update()
        {
            base.Update();
            
        }
    }
}
