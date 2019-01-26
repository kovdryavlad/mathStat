using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    public interface IClasterMetrics
    {
        double GetClasterDistance(List<double[]> S1, List<double[]> S2);
        double GetClasterDistance(Claster S1, Claster S2);

        double LansaWilliamsDistance(List<double[]> Sl, List<double[]> Sh, List<double[]> Sm);

        void SetPointMetrics(Func<double[], double[], double> d);
    }
}
