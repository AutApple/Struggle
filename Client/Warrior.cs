using SFML.Graphics;
using SFML.System;
using System;

namespace Struggle
{
    class Warrior : Unit
    {
        Text title;
        public Warrior(Vector2f crd, Fraction f, uint m) : base(crd, f, m)
        {
            title = new Text("Warrior", new Font("C:\\Windows\\Fonts\\Arial.ttf"));
            title.Color = Color.White;
            title.Position = coords;
        }

        public override void Draw(RenderWindow app)
        {
            base.Draw(app);
            app.Draw(title);
        }
    }
}
