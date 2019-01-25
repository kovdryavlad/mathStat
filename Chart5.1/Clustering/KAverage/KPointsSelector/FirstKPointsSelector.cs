using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.KAverage
{
    class FirstKPointsSelector : IKFirstPointsSelector
    {
        public double[][] GetFirstKPoints(double[][] data, int k)
        {
            double[][] result = new double[k][];

            for (int i = 0; i < k; i++)
                    result[i] = data[i];

            return result;
        }
    }
}
