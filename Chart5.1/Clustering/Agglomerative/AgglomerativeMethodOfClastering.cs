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
            Claster[] clasters = formClasterForEachPoint(statNd);

            while (clasters.Length > needClasterCount)
            {
                Matrix distances = CalcMatrixOfDistances(clasters, D);

                int[] ij = FindMinDistance(distances);
                int i = ij[0], j = ij[1];


            }
        }

        private static Claster[] formClasterForEachPoint(STATND statNd)
        {
            double[][] data = statNd.getJaggedArrayOfData();
            int N = data.GetLength(0);

            Claster[] clasters = new Claster[N];

            for (int i = 0; i < N; i++)
                clasters[i].AddPoint(data[i]);

            return clasters;
        }

        private Matrix CalcMatrixOfDistances(Claster[] clasters, IClasterMetrics D)
        {
            int n = clasters.Length;
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
                    if (j > i) break;

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
