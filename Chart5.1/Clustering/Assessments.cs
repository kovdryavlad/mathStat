using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering
{
    class Assessments
    {
        public double SumOfTheInternalDispersions(Claster[] clasters, Func<double[], double[], double> d)
        {
            double distance = 0;
            int k = clasters.Length;

            for (int j = 0; j < k; j++)
                for (int l = 0; l < clasters[j].Nj; l++)
                    distance += d(clasters[j].Points[l], clasters[j].Center).Pow(2);

            return distance;
        }

        public double SumOfPairwiseInternalDistances(Claster[] clasters, Func<double[], double[], double> d)
        {
            double distance = 0;
            int k = clasters.Length;

            for (int j = 0; j < k; j++)
                for (int l = 0; l < clasters[j].Nj-1; l++)
                    for (int h = l+1; h < clasters[j].Nj; h++)
                        distance += d(clasters[j].Points[l], clasters[j].Points[h]);

            return distance;
        }

        public double TotalIntroClusterDispersion(Claster[] clasters, Func<double[], double[], double> d)
        {
            int k = clasters.Length;
            int n = clasters[0].Center.Length;
            Matrix sumMatrix = Matrix.Create.New(n);

            for (int i = 0; i < k; i++)
            {
                Claster current = clasters[i];
                sumMatrix += current.Nj * current.GetMatrixOfCovariations();
            }

            return sumMatrix.Determinant();
        }

        public double RatiOfFunctionals(Claster[] clasters, Func<double[], double[], double> d)
        {
            int k = clasters.Length;
            double a = SumOfPairwiseInternalDistances(clasters, d);

            double denominatorToACoef = 0;
            double denominatorToBCoef = 1;

            for (int i = 0; i < k; i++)
            {
                var Nj = clasters[i].Nj;
                denominatorToACoef += (Nj * (Nj - 1) / 2d);

                denominatorToBCoef *= Nj;
            }

            a /= 1d / denominatorToACoef;

            double b = 0;

            for (int j = 0; j < k-1; j++)
                for (int l = 0; l < clasters[j].Nj; l++)
                    for (int m = j + 1; m < k; m++)
                        for (int h = 0; h < clasters[m].Nj; h++)
                            b += d(clasters[j].Points[l], clasters[m].Points[h]);

            b /= 1d / denominatorToBCoef;

            return a / b;
        }
    }
}
