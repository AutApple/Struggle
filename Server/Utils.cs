using System;

namespace Struggle
{
    static class Utils
    {
        public static float Distance(float ax, float ay, float bx, float by)
        {
            float dx = Math.Abs(ax - bx);
            float dy = Math.Abs(ay - by);

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
