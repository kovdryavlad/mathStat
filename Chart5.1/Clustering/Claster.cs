using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    public class Claster
    {
        public double[] Center { get; set; }

        public double[][] Dimentions { get; set; }

        public Claster(double[] center)
        {
            Center = center;
        }

        public List<double[]> Points = new List<double[]>();

        public int Nj => Points.Count;

        //returns true if abs of difference of all between new and old centers smaller then epsilon 
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

        public Matrix GetMatrixOfCovariations()
        {
            int n = Center.Length;
            double[] averages = Dimentions.Select(dim => dim.Average()).ToArray();

            double[][] cov = ArrayMatrix.GetJaggedArray(n, n);

            for (int k = 0; k < n; k++)
                for (int p = 0; p < n; p++)
                {
                    double v = 0;
                    for (int l = 0; l < Nj; l++)
                        v += (Points[k][l] - averages[k]) * (Points[p][l] - averages[p]);
                    

                    cov[k][p] = v / Nj;
                }

            return new Matrix(cov);
        }

        public void AddPoint(double[] point) => Points.Add(point);

        public void AppendPointsFromClater(Claster claster) => Points.AddRange(claster.Points);
    }
}
