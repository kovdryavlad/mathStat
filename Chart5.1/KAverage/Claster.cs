using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.KAverage
{
    class Claster
    {
        public double[] Center { get; set; }

        public Claster(double[] center)
        {
            Center = center;
        }

        public List<double[]> Points = new List<double[]>();

        public int Nj => Points.Count;

        public bool RecalcCenter(int k, double eps)
        {
            double[] newCenter = new double[k];
            double[][] pnts = ArrayMatrix.TransposeArr(Points.ToArray());

            for (int i = 0; i < k; i++)
                newCenter[i] = pnts[i].Average();

            //epsilon cheking
            bool status = true;

            for (int i = 0; i < k; i++)
                status = status && (Center[i] - newCenter[i]).Abs() < eps;

            Center = newCenter;
            return status;
        } 
    }
}
