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

        public double[][] Dimentions { get; set; }

        public Claster(double[] center)
        {
            Center = center;
        }

        public List<double[]> Points = new List<double[]>();

        public int Nj => Points.Count;

        public bool RecalcCenter(double eps)
        {
            if (Points.Count == 0)
                return true;            //значение, по факту, ничего не меняет.

            int n = Center.Length;
            double[] newCenter = new double[n];
            double[][] pnts = ArrayMatrix.TransposeArr(Points.ToArray());

            for (int i = 0; i < n; i++)
                newCenter[i] = pnts[i].Average();

            //epsilon cheking
            bool status = true;

            for (int i = 0; i < n; i++)
                status = status && (Center[i] - newCenter[i]).Abs() < eps;

            Center = newCenter;
            return status;
        }

        public void PrepareForVisualization() => Dimentions = ArrayMatrix.TransposeArr(Points.ToArray());

        public void ClearPoints() => Points.Clear();
    }
}
