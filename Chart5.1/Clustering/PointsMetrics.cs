using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    class PointsMetrics
    {
        public static Func<double[], double[], double> Evklid(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                double d = 0;
                for (int i = 0; i < length; i++)
                    d += Math.Pow(A[i] - B[i], 2);

                return Math.Sqrt(d);
            };
        }

        public static Func<double[], double[], double> WeightedEvklidean(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                double d = 0;
                double[] w = Param as double[];

                for (int i = 0; i < length; i++)
                    d += w[i] * Math.Pow(A[i] - B[i], 2);

                return Math.Sqrt(d);
            };
        }

        public static Func<double[], double[], double> Manheten(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                double d = 0;

                for (int i = 0; i < length; i++)
                    d += Math.Abs(A[i] - B[i]);

                return d;
            };
        }

        public static Func<double[], double[], double> Chebishev(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                List<double> d = new List<double>();

                for (int i = 0; i < length; i++)
                    d.Add(Math.Abs(A[i] - B[i]));

                return d.Max();
            };
        }

        public static Func<double[], double[], double> Minkovskogo(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                double d = 0;
                double m = (double)Param;

                for (int i = 0; i < length; i++)
                    d += Math.Pow(Math.Abs(A[i] - B[i]), m);

                return Math.Pow(d, 1d / m);
            };
        }

        public static Func<double[], double[], double> Mahalanobisa(object Param)
        {
            return (A, B) =>
            {
                int length = A.Length;

                double d = 0;
                Matrix R = Param as Matrix;

                Vector Avector = new Vector(A);
                Vector Bvector = new Vector(B);

                int n = A.Length;

                return Math.Sqrt((Avector - Bvector) * R.Inverse() * (Avector - Bvector));
            };
        }
    }
}
