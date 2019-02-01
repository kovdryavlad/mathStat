using Chart1._1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    internal class TrendHelper
    {
        public enum TrendType
        {
            Growing,
            Falling,
            Flat
        }

        internal static TrendType ExtremalPoint(STAT sample1D, double alpha)
        {
            var p = 0;
            var x = sample1D.d;
            int N = x.Length;
            for (int i = 1; i < N - 1; i++)
            {
                if ((x[i] > x[i - 1] && x[i] > x[i + 1]) || (x[i - 1] > x[i] && x[i + 1] > x[i]))
                    p++;
            }
            double E = (2d / 3) * (N - 2);
            double D = (1d / 90) * (16 * N - 29);
            double S = (p - E) / Math.Sqrt(D);

            //var u = BHData.Common.Quantile.Get_Quantile_normalization(BHData.Common.Constant.ParameterForQuantile);

            var u = Kvantili.Normal(alpha);
            
            if (Math.Abs(S) < u)
                return TrendType.Flat;
            else if (S < -u)
                return TrendType.Falling;
            else
                return TrendType.Growing;
        }

        internal static TrendType Sign(STAT sample1D, double alpha)
        {
            var x = sample1D.d;
            int N = x.Length;

            var c = Enumerable.Range(0, N - 1).Where(i => x[i + 1] >= x[i]).Count();

            double E = (1d / 2) * (N - 1);
            double D = 1d / 12 * (N + 1);
            double S = (c - E) / Math.Sqrt(D);

            //var u = BHData.Common.Quantile.Get_Quantile_normalization(BHData.Common.Constant.ParameterForQuantile);
            var u = Kvantili.Normal(alpha);


            if (Math.Abs(S) < u)
                return TrendType.Flat;
            else if (S < -u)
                return TrendType.Falling;
            else
                return TrendType.Growing;
        }

        internal static TrendType Abbe(STAT sample, double alpha)
        {
            int N = sample.d.Length;
            var elements = sample.d;
            double xAv = sample.Expectation;
            double q2 = Enumerable.Range(0, N - 1).Sum(i => Math.Pow(elements[i] - elements[i + 1], 2)) / (N - 1);

            double s2 = elements.Sum(e => Math.Pow(e - xAv, 2)) / (N - 1);

            double y = q2 / (2 * s2);
            double u = (y - 1) * Math.Sqrt((N * N - 1) / (N - 2));

            //var kv = Quantile.Get_Quantile_normalization(Constant.ParameterForQuantile);
            var kv = Kvantili.Normal(alpha);
            
            if (Math.Abs(u) < kv)
                return TrendType.Flat;
            else if (u < -kv)
                return TrendType.Falling;
            else
                return TrendType.Growing;
        }
    }
}
