using Chart1._1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    class TwoDimModel
    {
        public void ModelKvaziLinear(double Xmin, double Xmax, int n, double a, double b, double SigEp, string filename)
        {
            double m = (Xmax - Xmin) / 2d;
            double sigmaX = (Xmax - Xmin) / 6d;

            List<double> lstY = new List<double>();

            var x = Distribution.Normal(m, sigmaX, n);
            var eps = Distribution.Normal(0, SigEp, n);

            for (int i = 0; i < n; i++)
            {         
                var y = a + b * Math.Log(x[i]) + eps[i];
                lstY.Add(y);
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < n; i++)
                {
                    var line = String.Format("{0:0.0000}\t{1:0.0000}", Math.Round(x[i], 4), Math.Round(lstY[i], 4));

                    writer.WriteLine(line);
                }
            }
        }

        public void ModelParabolic(double Xmin, double Xmax, int n, double a, double b,double c, double SigEp, string filename)
        {
            double m = (Xmax - Xmin) / 2d;
            double sigmaX = (Xmax - Xmin) / 6d;

            List<double> lstY = new List<double>();

            var x = Distribution.Normal(m, sigmaX, n);
            var eps = Distribution.Normal(0, SigEp, n);

            for (int i = 0; i < n; i++)
            {
                var y = a + b*x[i]+c*x[i]*x[i] + eps[i];
                lstY.Add(y);
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < n; i++)
                {
                    var line = String.Format("{0:0.0000}\t{1:0.0000}", Math.Round(x[i], 4), Math.Round(lstY[i], 4));

                    writer.WriteLine(line);
                }
            }
        }
    }

    class Distribution
    {
        static Random F = new Random();
        public static double[] Normal(double m, double s, int n)
        /*  m, s параметры норм распр  n размер*/
        {
            double[] Var = new double[n];
            int k = 0;
            while (k < n)
            {
                double p = 2 * F.NextDouble() - 1;
                double t = 2 * F.NextDouble() - 1;
                double c = p * p + t * t;
                if (c > 0 && c <= 1)
                {
                    Var[k] = p * Math.Sqrt(-2 * Math.Log(c) / c);
                    k += 1;
                    if (k < n)
                    {
                        Var[k] = t * Math.Sqrt(-2 * Math.Log(c) / c);
                        k += 1;
                    }
                }
            }
            for (k = 0; k < n; k++)
            {
                Var[k] = m + s * Var[k];
            }
            return Var;
        }
        public static double[] Exp(double l, int n)
        /*  l параметры распр  n размер*/
        {
            double[] vib = new double[n];
            Random F = new Random();
            for (int p = 0; p < n; p++)
            {
                vib[p] = Math.Log(1.0 / (1.0 - 1 * F.NextDouble())) / l;
            }
            return vib;
        }
        public static double[] Lapl(double a, double b, int n)
        /*  a, b параметры распр  n размер*/
        {
            double[] vib1 = new double[n / 2];
            double[] vib2 = new double[n - n / 2];
            double[] vib = new double[n];
            Random F = new Random();
            vib1 = Exp(a, n / 2);
            vib2 = Exp(a, n - n / 2);
            for (int p = 0; p < n / 2; p++)
            {
                vib[p] = Math.Log(0.5) * vib1[p] + b;
            }
            for (int p = 0; p < n - n / 2; p++)
            {
                vib[p + n / 2] = -Math.Log(0.5) * vib1[p] + b;
            }

            return vib;
        }
        public static double[] Ravn(double a, double b, int n)
        /*  a, b параметры распр  n размер*/
        {
            double[] vib = new double[n];
            Random F = new Random();
            for (int p = 0; p < n; p++)
            {
                vib[p] = F.NextDouble();
            }
            return vib;
        }


    }
    
}
