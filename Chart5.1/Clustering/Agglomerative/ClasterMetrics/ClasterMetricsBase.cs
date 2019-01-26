using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    public abstract class ClasterMetricsBase : IClasterMetrics
    {
        protected Func<double[], double[], double> d;
        protected LansaWilliamsaObject lansaWilliamsaObject;

        public abstract double GetClasterDistance(List<double[]> S1, List<double[]> S2);
        public double GetClasterDistance(Claster S1, Claster S2) => GetClasterDistance(S1.Points, S2.Points);

        public void SetPointMetrics(Func<double[], double[], double> d) => this.d = d;

        public double LansaWilliamsDistance(List<double[]> Sl, List<double[]> Sh, List<double[]> Sm)
        {
            return lansaWilliamsaObject.Distance(Sl, Sh, Sm);
        }
    }
}
