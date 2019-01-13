using SFML.System;
using System;

namespace Struggle
{
    static class Utils
    {
        public static float Distance(Vector2f a, Vector2f b)
        {
            float dx = Math.Abs(a.X - b.X);
            float dy = Math.Abs(a.Y - b.Y);

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
