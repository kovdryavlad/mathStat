using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chart1._1;
using SimpleMatrix;

namespace Chart5._1
{
    class ClastersMetrics
    {
        public double NearestNeighbor(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            double minDistance = d(S1[0], S2[0]);

            for (int i = 0; i < S1.Count; i++)
                for (int j = 0; j < S2.Count; j++)
                    minDistance = Math.Min(d(S1[i], S2[j]), minDistance);

            return minDistance;
        }

        public double FarestNeighbor(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            double maxDistance = d(S1[0], S2[0]);

            for (int i = 0; i < S1.Count; i++)
                for (int j = 0; j < S2.Count; j++)
                    maxDistance = Math.Max(d(S1[i], S2[j]), maxDistance);

            return maxDistance;
        }

        public double WeightedAverage(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            double dSum = 0;

            int N1 = S1.Count;
            int N2 = S2.Count;

            for (int i = 0; i < N1; i++)
                for (int j = 0; j < N2; j++)
                    dSum += d(S1[i], S2[j]);

            return dSum/(N1*N2);
        }

        public double NotWeightedAverage(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            double dSum = 0;

            int N1 = S1.Count;
            int N2 = S2.Count;

            for (int i = 0; i < N1; i++)
                for (int j = 0; j < N2; j++)
                    dSum += d(S1[i], S2[j]);

            return dSum / (4);
        }

        public double Median(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            var me1 = PrecessClasterByDimentions(S1, s => s.MED, s => s.CalcMEDnMAD());
            var me2 = PrecessClasterByDimentions(S2, s => s.MED, s => s.CalcMEDnMAD());
            
            return d(me1, me2) / 2;
        }

        public double ByCenters(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            var avg1 = PrecessClasterByDimentions(S1, s => s.Expectation, null);
            var avg2 = PrecessClasterByDimentions(S2, s => s.Expectation, null);

            return d(avg1, avg2);
        }

        public double Uord(List<double[]> S1, List<double[]> S2, Func<double[], double[], double> d)
        {
            int N1 = S1.Count;
            int N2 = S2.Count;
            
            var avg1 = PrecessClasterByDimentions(S1, s => s.Expectation, null);
            var avg2 = PrecessClasterByDimentions(S2, s => s.Expectation, null);

            return d(avg1, avg2)*(N1*N2)/(N1+N2);
        }

        private double[] PrecessClasterByDimentions(List<double[]>claster, Func<STAT, double> geterResultValueFunc, Action<STAT> provessAction)
        {
            int n = claster[0].Length;
            int N = claster.Count;

            var arr = ArrayMatrix.GetJaggedArray(n, N);

            for (int i = 0; i < N; i++)
                for (int j = 0; j < n; i++)
                    arr[j][i] = claster[i][j];

            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                STAT s = new STAT();
                s.Setd2Stat2d(arr[i]);

                provessAction?.Invoke(s);

                result[i] = geterResultValueFunc(s);
            }

            return result;
        }
    }
}
