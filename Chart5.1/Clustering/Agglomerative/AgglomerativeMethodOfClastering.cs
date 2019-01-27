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

            Matrix distances = CalcMatrixOfDistances(clasters, D);

            while (clasters.Count > needClasterCount)
            {

                int[] ij = FindMinDistance(distances);
                int l = ij[0], h = ij[1];   //l always bigger than h???

                clasters[h].AppendPointsFromClater(clasters[l]);

                distances = distances.RemoveRow(l);
                distances = distances.RemoveColumn(l);

                //go by row
                for (int m = 0; m < h; m++)
                    distances[h, m] = D.LansaWilliamsDistance(clasters[l], clasters[h], clasters[m]);

                //go by column
                for (int m = h+1; m < distances.Rows; m++)
                    distances[m, h] = D.LansaWilliamsDistance(clasters[l], clasters[h], clasters[m]);
                
                clasters.RemoveAt(l);
            }

            return clasters.ToArray();
        }

        private static List<Claster> formClasterForEachPoint(STATND statNd)
        {
            double[][] data = statNd.getJaggedArrayOfData();
            int N = data.GetLength(0);

            //Claster[] clasters = new Claster[N];
            Claster[] clasters = Enumerable.Range(0, N).Select(el => new Claster()).ToArray();

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

            int minIndex_i=1, minIndex_j = 0;
            double min = data[minIndex_i][minIndex_j];

            for (int i = 1; i < n; i++)
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
