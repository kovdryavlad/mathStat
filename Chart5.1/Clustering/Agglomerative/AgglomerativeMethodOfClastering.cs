using Chart5._1.Clustering.Agglomerative.ClasterMetrics;
using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative
{
    class AgglomerativeMethodOfClastering
    {
        public Claster[] Clasterize(STATND statNd, int needClasterCount, IClasterMetrics D)
        {
            List<Claster> clasters = formClasterForEachPoint(statNd);
            
            while (clasters.Count > needClasterCount)
            {
                Matrix distances = CalcMatrixOfDistances(clasters, D);

                int[] ij = FindMinDistance(distances);
                int l = ij[0], h = ij[1];   //l always bigger than h???

                clasters[h].AppendPointsFromClater(clasters[l]);

                clasters.RemoveAt(l);
                distances.RemoveRow(l);
                distances.RemoveColumn(l);

                //go by row
                for (int m = 0; m < h; m++)
                    distances[h, m] = D.LansaWilliamsDistance(clasters[l], clasters[h], clasters[m]);

                //go by column
                for (int m = h+1; m < distances.Rows; m++)
                    distances[m, h] = D.LansaWilliamsDistance(clasters[l], clasters[h], clasters[m]);
            }

            return clasters.ToArray();
        }

        private static List<Claster> formClasterForEachPoint(STATND statNd)
        {
            double[][] data = statNd.getJaggedArrayOfData();
            int N = data.GetLength(0);

            Claster[] clasters = new Claster[N];

            for (int i = 0; i < N; i++)
                clasters[i].AddPoint(data[i]);

            return clasters.ToList();
        }

        private Matrix CalcMatrixOfDistances(List<Claster> clasters, IClasterMetrics D)
        {
            int n = clasters.Count;
            double[][] distances = ArrayMatrix.GetJaggedArray(n, n);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (j > i) break;
                    else
                        distances[i][j] = i == j ? 0 : D.GetClasterDistance(clasters[i], clasters[j]);

            return new Matrix(distances);
        }

        private int[] FindMinDistance(Matrix distances)
        {

            int n = distances.Rows;
            double[][] data = distances.data;
            double min = data[0][0];

            int minIndex_i=0, minIndex_j = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j >= i) break;

                    if (data[i][j] < min)
                    {
                        min = data[i][j];

                        minIndex_i = i;
                        minIndex_j = j;
                    }
                }
            }

            return new[] { minIndex_i, minIndex_j};
        }
    }
}
