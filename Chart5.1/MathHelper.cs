using System;

namespace Chart1._1
{
    static class MathHelper
    {
        public static double Pow(this double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double Pow(this int x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double Pow(this double x, int y)
        {
            return Math.Pow(x, y);
        }

        public static double Pow(this int x, int y)
        {
            return Math.Pow(x, y);
        }
    }
}