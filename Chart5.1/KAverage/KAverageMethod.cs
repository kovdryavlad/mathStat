using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chart5._1.KAverage;

namespace Chart5._1
{
    class KAverageMethod
    {
        int m_k;
        int m_N;
        double[][] m_data;
        Claster[] m_clasters;

        public double Epsilon { get; set; } = 0.000001;

        public KAverageMethod(STATND statNd, int k, IKFirstPointsSelector KFirstPointsSelector)
        {
            m_clasters = new Claster[k];
            m_k = k;

            int n = statNd.N;
            m_N = statNd.GetStat(0).d.Length;

            m_data = new double[n][];

            for (int i = 0; i < n; i++)
                m_data[i] = statNd.GetStat(i).d;

            m_data = ArrayMatrix.TransposeArr(m_data);

            double[][] centers = KFirstPointsSelector.GetFirstKPoints(m_data, k);

            for (int i = 0; i < k; i++)
                m_clasters[i] = new Claster(centers[i]);
        }

        public Claster[] BollaHolla(Func<double[], double[], double> d, int iterations)
        {
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                for (int i = 0; i < m_N; i++)
                {
                    double[] currentPoint = m_data[i];

                    var distances = m_clasters.Select((claster, index) => 
                                                    new { Index = index, Distance = d(claster.Center, currentPoint) })
                                              .OrderBy(el => el.Distance)
                                              .ToArray();
                    int minIndex = distances[0].Index;

                    m_clasters[minIndex].Points.Add(currentPoint);
                }

                //recalc Centers of clasters and remember status
                bool status = true;
                for (int i = 0; i < m_k; i++)                
                    status = status && m_clasters[i].RecalcCenter(Epsilon);

                if (status && iteration==iterations-1)
                    break;

                for (int k = 0; k < m_k; k++)
                    m_clasters[k].ClearPoints();
                
            }

            return m_clasters;            
        }

        public Claster[] McKinna(Func<double[], double[], double> d, int iterations)
        {
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                for (int i = 0; i < m_N; i++)
                {
                    double[] currentPoint = m_data[i];

                    var distances = m_clasters.Select((claster, index) =>
                                                    new { Index = index, Distance = d(claster.Center, currentPoint) })
                                              .OrderBy(el => el.Distance)
                                              .ToArray();
                    int minIndex = distances[0].Index;

                    m_clasters[minIndex].Points.Add(currentPoint);

                    //recalc Center of clasters and remember status
                    for (int j = 0; j < m_k; j++)
                        m_clasters[j].RecalcCenter(Epsilon);
                }

                if (iteration == iterations - 1)
                    break;

                for (int k = 0; k < m_k; k++)
                    m_clasters[k].ClearPoints();
            }


            return m_clasters;
        }

    }
}
