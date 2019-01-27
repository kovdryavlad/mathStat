using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    class WeightedAverage:ClasterMetricsBase
    {
        public override double GetClasterDistance(List<double[]> S1, List<double[]> S2)
        {
            double dSum = 0;

            int N1 = S1.Count;
            int N2 = S2.Count;

            for (int i = 0; i < N1; i++)
                for (int j = 0; j < N2; j++)
                    dSum += d(S1[i], S2[j]);

            return dSum / (N1 * N2);
        }

        public override double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm)
        {
            int Nl = Sl.Points.Count;
            int Nh = Sh.Points.Count;
            
            double den = Nl + Nh;

            SetLansaWiliamsParams(Nl / den, Nh / den,0,0);

            return base.LansaWilliamsDistance(Sl,Sh,Sm);
        }
    }
}
