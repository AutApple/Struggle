using SFML.Graphics;
using SFML.System;
using System;

namespace Struggle
{
    class BuildPlace : Entity
    {
        string type;
        int buildersCount;
        int buildersRequired;
        Fraction f;


        public BuildPlace(string type, int buildersRequired, Vector2f crd, uint m) : base(crd, m)
        {
            this.type = type;
            this.buildersRequired = buildersRequired;
            titleString = type + "\n" + buildersCount.ToString() + "/" + buildersRequired.ToString();
            Console.WriteLine(titleString);
        }

        public void Increase(Fraction f)
        {
            if(this.f != f && buildersCount > 0)
               buildersCount--;
            else
            {
                this.f = f;
                buildersCount++;
            }

            if(buildersCount == buildersRequired)
            {
                //create unit
                switch (type)
                {
                    case "Warrior":
                        f.AddEntity(new Warrior(Position, 16));
                        break;
                    default:
                        f.AddEntity(new Unit(Position, 32));
                        break;
                }
                buildersCount = 0;
                f = null;
            }

            titleString = type + "\n" + buildersCount.ToString() + "/" + buildersRequired.ToString();
        }

        public override void Update()
        {
            base.Update();
            if (f != null)
                title.Color = f.color;
            else
                title.Color = Color.White;
        }

        public override void Draw(RenderWindow window)
        {
            base.Draw(window);
        }
    }
}
