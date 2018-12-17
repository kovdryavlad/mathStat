using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    public static class RFA
    {
        public static void Solve(Matrix R, double epsilon)
        {
            double[] h1Max = GetH1MaxCor(R.data);

            double[] h2Max = GetH2ByMGK(R);

        }
        
        private static double[] GetH1MaxCor(double[][] R)
        {
            int n = R[0].Length;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                List<double> lst = new List<double>();
                for (int j = 0; j < n; j++) { 
                    lst.Add(R[i][j]);
                }

                result[i] = lst.OrderByDescending(v=>v).ToArray()[1];
            }

            return result;
        }

        private static double[] GetH2ByMGK(Matrix R)
        {
            var l = LaverierFadeevaMethod.Solve(R, LaverierFadeevaSolvingOptions.FullSolving);
            LaverierFadeevaExtendedResult[] ext = LaverierFadeevaExtendedResult.ConvertToExtendedLaverierFadeevaResult(l);
            ext = ext.OrderByDescending(v => v.EigenValue.Abs()).ToArray();

            int w = ext.Count(v => v.EigenValue.Abs() > 1);

            var neededVectors = ext.Take(w).Select(el => el.eigenVector).ToList();

            double[] result = new double[ext.Length];

            for (int i = 0; i < ext.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < w; j++)
                {
                    sum += R.data[i][j];
                }
                result[i] = sum;
            }


            return result;
        }

    }
}
