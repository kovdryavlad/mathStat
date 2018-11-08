using System;
using System.Linq;
using Disrtibutions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chart1._1
{
    public partial class STAT
    {
        public double p = 0.05;      //для доверительных интервалов на графике

        double[] _FTeor;             //средняя линия на правом графике

        public event EventHandler<ReproductEventArgs> Reproducting;   //для вывода найденых оценок параметров

        //чтобы хранить воссозданное распределение,
        //возможно пригодяться параметры + точно имя для автоматического прохождения критерией согласия
        OneDimentionalDistribution _ReproductDistribution;

        /// <summary>
        /// Делегат, в котором есть все ф-и воссоздания распределений
        /// </summary>
        public List<Action<Series,Series,Series,Series>> ReproductFunctions;

        /// <summary>
        /// функция которая рисует на левом графике график плотности, а на правом 
        /// теоретическую ф-ю распеределения + вернюю и нижнюю интервальные оценки 
        /// </summary>
        public void VisualizeDistribution(IReproductable Distr, Series plotn, 
                                          Series teor, Series niz, Series verh)
        {
            _ReproductDistribution = Distr as OneDimentionalDistribution;

            var dSortedCopy = d.OrderBy(x => x).ToArray();
            for (int i = 0; i < d.Length; i++)
            {
                double x = dSortedCopy[i];

                double f = Distr.f(x);       //еще нужно применить ф-ю нормализации - obr

                double F = Distr.F(x);       //еще нужно применить ф-ю нормализации - obr

                double DF = Distr.DF(x);

                plotn.Points.AddXY(x, f * h);

                teor.Points.AddXY(x, F);

                niz.Points.AddXY(x, F - Kvantili.Normal(p) * Math.Sqrt(DF));

                verh.Points.AddXY(x, F + Kvantili.Normal(p) * Math.Sqrt(DF));

                _FTeor[i] = F;       //для КС Колмогорова
            }
         }

        //перед созданием каждого из распределений в события будут транслироваться 
        //найденые параметры

        public void ReproductExp(Series plotn, Series teor, Series niz, Series verh)
        {
            double l = 1d / Expectation;

            double Dl = l * l / d.Length;

            if (Reproducting!=null)
                Reproducting(this, new ReproductEventArgs(
                             new Param("Лямбда", l, Math.Sqrt(Dl), _kvantil)
                            ));

            IReproductable Distr = new Exponential(l, Dl);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }
        
        public void ReproductArcSin(Series plotn,Series teor, Series niz, Series verh)
        {
            double a = Math.Sqrt(2*(InitialMoment(2)-Expectation*Expectation));

            double Da = Math.Pow(a,4)/(8*d.Length);

            if (Reproducting != null)
                Reproducting(this, 
                            new ReproductEventArgs(
                                     new Param("а", a, Math.Sqrt(Da), _kvantil)
                            ));

            IReproductable Distr = new Arcsin(a, Da);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }

        public void ReproductNormal(Series plotn, Series teor, Series niz, Series verh)
        {
            double m = Expectation;

            double s = (double)d.Length / (d.Length - 1) * Math.Sqrt(InitialMoment(2) - Expectation * Expectation);

            double Dm = s * s / d.Length;

            double Ds = Dm / 2d;

            if (Reproducting != null)
                Reproducting(this,
                            new ReproductEventArgs(
                                     new Param("m", m, Math.Sqrt(Dm), _kvantil),
                                     new Param("s", s, Math.Sqrt(Ds), _kvantil)
                            ));

            IReproductable Distr = new Normal(m, Dm, s, Ds);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }
        
        public void ReproductLaplasa(Series plotn, Series teor, Series niz, Series verh)
        {
            double m = Expectation;

            double l = Math.Sqrt(2d) / Sigma;

            double Dl = 5 * l * l / d.Length;

            double Dm = 2d / (d.Length * l * l);

            double cov = -3d / (2 * d.Length);

            if (Reproducting != null)
                Reproducting(this,
                            new ReproductEventArgs(
                                     new Param("m", m, Math.Sqrt(Dm), _kvantil),
                                     new Param("l", l, Math.Sqrt(Dl), _kvantil)
                            ));

            IReproductable Distr = new Laplasa(m, Dm, l, Dl, cov);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }

        public void ReproductReley(Series plotn, Series teor, Series niz, Series verh)
        {
            double s = 0.8 * Expectation;

            double Ds = (0.2752*s*s)/d.Length;

            if (Reproducting != null)
                Reproducting(this,
                            new ReproductEventArgs(
                                     new Param("s", s, Math.Sqrt(Ds), _kvantil)
                            ));

            IReproductable Distr = new Reley(s, Ds);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }

        public void ReproductVeibula(Series plotn, Series teor, Series niz, Series verh)
        {
            #region   Часть для нахождения A и Beta
            
            double a11 = d.Length - 1;

            double a12=0;

            for (int i = 0; i < d.Length - 1; i++)
                a12 += Math.Log(d[i]);

            double a21 = a12;

            double a22 = 0;

            for (int i = 0; i < d.Length - 1; i++)
                a22 += Math.Pow(Math.Log(d[i]),2);

            //эта часть встречается в остаточной дисперсии, b1 и b2
            //входной аргумент ни в коесм случае не d[i]
            //только Seria3[i]
            Func<double, double> LnPart = (F1NVAlue) =>
            {
                if (F1NVAlue == 1)
                    return 0;

                double Ln = Math.Log(1d / (1 - F1NVAlue));

                return Math.Log(Ln);
            };
            
            double b1 = 0,
                   b2 = 0;

            for (int i = 0; i < d.Length-1; i++)
            {
                double F1N = Seria3Y[i];

                b1 += LnPart(F1N);

                b2 += Math.Log(d[i]) * LnPart(F1N);
            }

            double delta = a11*a22-a12*a21;

            double deltaA = b1 * a22 - a12 * b2;

            double deltaBeta = a11 * b2 - b1 * a21;

            double A = deltaA / delta;

            double Beta = deltaBeta / delta;
           
            #endregion
            
            double Alpha = Math.Exp(-A);

            double DispZal=0;

            for (int i = 0; i < d.Length - 1; i++)
                DispZal += Math.Pow(LnPart(Seria3Y[i]) - A - Beta * Math.Log(d[i]),2);
              

            DispZal /= d.Length - 3;

            double denominator = delta;

            double DA = a22 * DispZal / denominator;

            double DBeta = a11 * DispZal / denominator;

            double cov_AvsBeta = -a12 * DispZal / denominator;

            double DAlpha = Math.Exp(-2 * A) * DA;

            double cov_AlphavsBeta = -Math.Exp(A) * cov_AvsBeta;

            if (Reproducting != null)
                Reproducting(this,
                            new ReproductEventArgs(
                                     new Param("alpha", Alpha, Math.Sqrt(DAlpha), _kvantil),
                                     new Param("beta", Beta, Math.Sqrt(DBeta), _kvantil)
                            ));

            IReproductable Distr = new Veibula(Alpha, DAlpha, Beta, DBeta, cov_AlphavsBeta);

            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }


        public void ReproductRavn(Series plotn, Series teor, Series niz, Series verh)
        {
            var InitMoment = InitialMoment(2);
            var exp = Expectation;

            double commonForAB = Math.Sqrt(3*(InitialMoment(2)-Expectation*Expectation));
           
            double a = Expectation - commonForAB;
            double b = Expectation+commonForAB;

            //a = Min;
            //b = Max;

            double dH1dExpectation = 1 + 3 * (a + b) / (b - a);
            double dH2dExpectation = 1 - 3 * (a + b) / (b - a);

            double dH1dxkv = -3d / (b - a);
            double dH2dxkv = 3d / (b - a);

            double DExp = Math.Pow(b - a, 2) / (12 * d.Length);
            double cov = DExp * (a + b);

            double Dxkv = 1d/(180*d.Length)*(Math.Pow(b-a,4) +15*Math.Pow(a+b,2)*Math.Pow(b-a,2));

            double Da = Math.Pow(dH1dExpectation, 2) * DExp + Math.Pow(dH1dxkv, 2) * Dxkv + 2 * dH1dExpectation * dH1dxkv * cov;
            double Db = Math.Pow(dH2dExpectation, 2) * DExp + Math.Pow(dH2dxkv, 2) * Dxkv + 2 * dH2dExpectation * dH2dxkv * cov;

            double covAB = dH1dExpectation * dH2dExpectation * DExp + dH1dxkv * dH2dxkv * Dxkv + (dH1dExpectation * dH2dxkv + dH1dxkv * dH2dExpectation) * cov;

            IReproductable Distr = new Ravn(a, Da, b,Db, covAB);
            VisualizeDistribution(Distr, plotn, teor, niz, verh);
        }

        public void AutoReproduct(Series plotn, Series teor, Series niz, Series verh, TextBox tb)
        {
            string log="";

            log += String.Format("{0}---Автоматичне відтворення розподілів---{0}{0}",Environment.NewLine);

            Action clear = () =>
                {
                    plotn.Points.Clear();

                    teor.Points.Clear();

                    niz.Points.Clear();

                    verh.Points.Clear();
                };

            log += String.Format("Розподіл:{0}", Environment.NewLine);

            int saver=0;;

            for (int i = 0; i < ReproductFunctions.Count; i++)
            {
                clear();

                ReproductFunctions[i](plotn,teor, niz, verh);

                log += String.Format("*{1}{0}", Environment.NewLine, _ReproductDistribution.Name);

                string s = KZKolvogorov();

                log += String.Format("\tКУ Колмогорова: {1}{0}", Environment.NewLine, s);

                string stpirs = KZPirsona();

                log += String.Format("\tКУ Пірсона: {1}{0}", Environment.NewLine, stpirs);

               // if (s.Equals("Пройдено"))
                  //  saver = i;
                //и тут в будущем KSPirsona
            }
            //MessageBox.Show(saver + "");
            
              clear();
            
            tb.Text = log;
            /*
            clear();

            if (saver == 0)
                clear();
            else
            ReproductFunctions[saver](plotn, teor, niz, verh);
             * */
        }

    }
}
