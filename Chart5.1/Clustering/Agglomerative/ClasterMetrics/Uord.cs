using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    class Uord : ClasterMetricsBase
    {
        public override double GetClasterDistance(List<double[]> S1, List<double[]> S2)
        {
            int N1 = S1.Count;
            int N2 = S2.Count;

            var avg1 = PrecessClasterByDimentions(S1, s => s.Expectation, null);
            var avg2 = PrecessClasterByDimentions(S2, s => s.Expectation, null);

            return d(avg1, avg2) * (N1 * N2) / (N1 + N2);
        }

        public override double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm)
        {
            int Nl = Sl.Points.Count;
            int Nh = Sh.Points.Count;
            int Nm = Sm.Points.Count;

            double den = Nl + Nh + Nm;

            double alpha_l = (Nm + Nl) / den;
            double alpha_h = (Nm + Nh) / den;
            double beta = -Nm  / den;

            SetLansaWiliamsParams(alpha_l, alpha_h, beta, 0);

            return base.LansaWilliamsDistance(Sl, Sh, Sm);
        }
    }
}
