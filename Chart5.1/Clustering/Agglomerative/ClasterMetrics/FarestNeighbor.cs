using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    class FarestNeighbor:ClasterMetricsBase
    {
        public FarestNeighbor()
        {
            SetLansaWiliamsParams(0.5,0.5,0,0.5);
        }

        public override double GetClasterDistance(List<double[]> S1, List<double[]> S2)
        {
            double maxDistance = d(S1[0], S2[0]);

            for (int i = 0; i < S1.Count; i++)
                for (int j = 0; j < S2.Count; j++)
                    maxDistance = Math.Max(d(S1[i], S2[j]), maxDistance);

            return maxDistance;
        }
    }
}
