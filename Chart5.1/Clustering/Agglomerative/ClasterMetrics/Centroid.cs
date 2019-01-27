using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    class Centroid : ClasterMetricsBase
    {
        public override double GetClasterDistance(List<double[]> S1, List<double[]> S2)
        {
            var avg1 = PrecessClasterByDimentions(S1, s => s.Expectation, null);
            var avg2 = PrecessClasterByDimentions(S2, s => s.Expectation, null);

            return d(avg1, avg2);
        }

        public override double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm)
        {
            int Nl = Sl.Points.Count;
            int Nh = Sh.Points.Count;

            double den = Nl + Nh;

            SetLansaWiliamsParams(Nl / den, Nh / den, -Nl * Nh / den.Pow(2),0);

            return base.LansaWilliamsDistance(Sl, Sh, Sm);
        }
    }
}
