using Chart1._1;
using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.Clustering.Agglomerative.ClasterMetrics
{
    public abstract class ClasterMetricsBase : IClasterMetrics
    {
        LansaWilliamsaParamsType lwParams;
        public LansaWilliamsaParamsType LansaWilliamsaParamsType 
        {
            get => lwParams;
            protected set {
                lwParams = value;
            }
        }

        protected Func<double[], double[], double> d;
        protected LansaWilliamsaObject lansaWilliamsaObject = new LansaWilliamsaObject();

        public abstract double GetClasterDistance(List<double[]> S1, List<double[]> S2);
        public double GetClasterDistance(Claster S1, Claster S2)=> GetClasterDistance(S1.Points, S2.Points);

        public void SetPointMetrics(Func<double[], double[], double> d) => this.d = d;

        public virtual double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm)
        {
            return lansaWilliamsaObject.Distance(Sl, Sh, Sm);
        }

        //public virtual double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm, double Nl, double Nh)
        //{
        //    CheckingOnLansaWilliamsaParamsType(LansaWilliamsaParamsType.SpecialType2);
        //
        //    return 0;
        //}
        //
        //public virtual double LansaWilliamsDistance(Claster Sl, Claster Sh, Claster Sm, double Nl, double Nh, double Nm)
        //{
        //    CheckingOnLansaWilliamsaParamsType(LansaWilliamsaParamsType.SpecialType3);
        //
        //    return 0;
        //}

        //проброс параметров
        protected void SetLansaWiliamsParams(double alpha_l, double alpha_h, double beta, double gama)
        {
            lwParams = LansaWilliamsaParamsType.CommonType;
            lansaWilliamsaObject.SetParams(alpha_l, alpha_h, beta, gama);
        }

        protected void CheckingOnLansaWilliamsaParamsType(LansaWilliamsaParamsType ParamsNededInMethodType)
        {
            if (lwParams!=ParamsNededInMethodType)
                throw new Exception("This is incorrect method for this metrics");

        }

        protected double[] PrecessClasterByDimentions(List<double[]> claster, Func<STAT, double> geterResultValueFunc, Action<STAT> provessAction)
        {
            int n = claster[0].Length;
            int N = claster.Count;

            var arr = ArrayMatrix.GetJaggedArray(n, N);

            for (int i = 0; i < N; i++)
                for (int j = 0; j < n; i++)
                    arr[j][i] = claster[i][j];

            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                STAT s = new STAT();
                s.Setd2Stat2d(arr[i]);

                provessAction?.Invoke(s);

                result[i] = geterResultValueFunc(s);
            }

            return result;
        }

        public ClasterMetricsBase()
        {
            lansaWilliamsaObject.SetDistanceDelegat(GetClasterDistance);
        }
    }
}
