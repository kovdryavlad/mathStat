using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    public interface IClasterMetrics
    {
        double GetClasterDistance(Claster S1, Claster S2);

        double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm);
        //double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm, double Nl, double Nh);
        //double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm, double Nl, double Nh, double Nm);

        void SetPointMetrics(Func<double[], double[], double> d);
    }
}
