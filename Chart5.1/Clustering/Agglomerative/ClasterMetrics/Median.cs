using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    class Median:ClasterMetricsBase
    {
        public Median()
        {
            SetLansaWiliamsParams(0.5, 0.5, -0.25, 0);
        }

        public override double GetClasterDistance(List<double[]> S1, List<double[]> S2)
        {
            var me1 = PrecessClasterByDimentions(S1, s => s.MED, s => s.CalcMEDnMAD());
            var me2 = PrecessClasterByDimentions(S2, s => s.MED, s => s.CalcMEDnMAD());

            return d(me1, me2) / 2;
        }
    }
}
