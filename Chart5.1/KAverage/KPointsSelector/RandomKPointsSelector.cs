using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.KAverage
{
    class RandomKPointsSelector : IKFirstPointsSelector
    {
        public double[][] GetFirstKPoints(double[][] data, int k)
        {
            double[][] result = new double[k][];
            List<int> indexes = new List<int>();
            
            Random r = new Random();
            int maxBorder = data.GetLength(0);

            for (int i = 0; i < k; i++)
            {
                int index = r.Next(0, maxBorder);

                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                    result[i] = data[index];
                }
            }

            return result;
        }
    }
}
