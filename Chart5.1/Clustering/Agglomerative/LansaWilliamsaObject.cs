using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    public class LansaWilliamsaObject
    {
        double alpha_l, alpha_h, beta, gama;
        Func<List<double[]>, List<double[]>, double> D;

        public void SetParams(double alpha_l, double alpha_h, double beta, double gama)
        {
            this.alpha_l = alpha_l;
            this.alpha_h = alpha_h;
            this.beta = beta;
            this.gama = gama;
        }

        public void SetDistanceDelegat(Func<List<double[]>, List<double[]>, double> D) => this.D = D;

        public double Distance(List<double[]> Sl, List<double[]> Sh, List<double[]> Sm)
        {
            return alpha_l * D(Sl, Sm) + alpha_h * D(Sh, Sm) + beta * D(Sl, Sh) + gama * (D(Sl, Sm) - D(Sh, Sm)).Abs();
        }

        public double Distance(Claster Sl, Claster Sh, Claster Sm) => Distance(Sl.Points, Sh.Points, Sm.Points);
    }
}
