using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    static class Kvantili
    {
        public static double Normal(double p)
        {
            double c0 = 2.515517;
            double c1 = 0.802853;
            double c2 = 0.010328;
            double d1 = 1.432788;
            double d2 = 0.1892659;
            double d3 = 0.001308;
            double t = Math.Sqrt(Math.Log(1 / (p * p)));

            return t - (c0 + c1 * t + c2 * t * t) / (1 + d1 * t + d2 * t * t + d3 * t * t * t);
        }

        public static double Student(double p, double v)
        {
            double u = Normal(p);
            double g1 = 1 / 4.0 * (u * u * u + u);
            double g2 = 1 / 96.0 * (5 * Math.Pow(u, 5) + 16 * u * u * u + 3 * u);
            double g3 = 1 / 384.0 * (3 * Math.Pow(u, 7) + 19 * Math.Pow(u, 5) + 17 * u * u * u - 15 * u);
            double g4 = 1 / 92160.0 * (79 * Math.Pow(u, 9) + 779 * Math.Pow(u, 7) + 1482 * Math.Pow(u, 5) - 1920 * Math.Pow(u, 3) - 945 * u);
            return u + g1 / v + g2 / (v * v) + g3 / (v * v * v) + g4 / (Math.Pow(v, 4));
        }

        public static double Hi2(double p, double v)
        {
            return v * Math.Pow(1 - 2 / (9 * v) + Normal(p) * Math.Sqrt(2 / (9 * v)), 3);
        }

        public static double Fishera(double alpha, double v1, double v2)
        {
            double s = 1 / v1 + 1 / v2;
            double d = 1 / v1 - 1 / v2;
            double u = Normal(alpha);
            double z = u * Math.Sqrt(s / 2) - 1 / 6.0 * d * (u * u + 2) + Math.Sqrt(s / 2) * (s / 24.0 * (u * u + 3 * u) + 1 / 72.0 * d * d / s * (u * u * u + 11 * u));
            z -= s * d / 120.0 * (Math.Pow(u, 4) + 9 * u * u + 8);
            z += d * d * d / 3240 / s * (3 * Math.Pow(u, 4) + 7 * u * u - 16) + Math.Sqrt(s / 2) * (s * s / 1920.0 * (Math.Pow(u, 5) + 20 * u * u * u + 15 * u));
            z += Math.Pow(d, 4) / 2880.0 * (Math.Pow(u, 5) + 44 * u * u * u + 183 * u) + Math.Pow(d, 4) / (155520.0 * s * s) * (9 * Math.Pow(u, 5) - 284 * u * u * u - 1513 * u);
            return Math.Exp(2 * z);
        }
    }
}
