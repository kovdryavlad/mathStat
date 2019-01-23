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
        public static double Evklid(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            double d = 0;
            for (int i = 0; i < length; i++)
                d+= Math.Pow(A[i] - B[i],2);

            return Math.Sqrt(d);
        }

        public static double WeightedEvklidean(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            double d = 0;
            double[] w = Param as double[];

            for (int i = 0; i < length; i++)
                d += w[i]*Math.Pow(A[i] - B[i], 2);

            return Math.Sqrt(d);
        }

        public static double Manheten(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            double d = 0;
            
            for (int i = 0; i < length; i++)
                d += Math.Abs(A[i] - B[i]);

            return d;
        }

        public static double Chebishev(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            List<double> d = new List<double>();

            for (int i = 0; i < length; i++)
                d.Add(Math.Abs(A[i] - B[i]));

            return d.Max();
        }

        public static double Minkovskogo(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            double d = 0;
            double m = (double)Param;

            for (int i = 0; i < length; i++)
                d += Math.Pow(Math.Abs(A[i] - B[i]), m);

            return Math.Pow(d, 1d/m);
        }

        public static double Mahalanobisa(double[] A, double[] B, object Param)
        {
            int length = A.Length;

            double d = 0;
            Matrix R = Param as Matrix;

            Vector Avector = new Vector(A);
            Vector Bvector = new Vector(B);

            int n = A.Length;

            return Math.Sqrt((Avector - Bvector) * R.Inverse()*(Avector - Bvector));
        }
    }
}
