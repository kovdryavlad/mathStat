using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    //файл изьятия аномалий

    partial class STAT
    {
        public double BorderA;
        public double BorderB;
 
        public void AnomalFirt(AnomalOptions options)
        {
            //скопировал значения для увеличения читаемосости кода
            double x = Expectation; 

            double s = Sigma;

            double t1 = 2 + 0.2 * Math.Log10(0.04 * d.Length);

            double t2 = Math.Sqrt(19 * Math.Sqrt(Excess + 2) + 1);

            double a, b; //границы

            #region Определение границ a и b
            
            if (A < -0.2)
            {
                a = x - t2 * s;
                b = x + t1 * s;
            }

            else if (A > 0.2)
            {
                a = x - t1 * s;
                b = x + t2 * s;
            }

            else 
            {
                a = x - t1 * s;
                b = x + t1 * s;
            }
            #endregion

            #region Одиинаковый код, который позволяет только найти границы без изменения обрабатываемо массива

            BorderA = a;

            BorderB = b;

            if ((options & AnomalOptions.OnlyFindBordersAandB) != 0)
                return;
            
            #endregion

            d = d.Where(value => value >= a && value <= b)
                .ToArray();
        }

        //ф-я для удаления вторым и третим методом
        //отличаются только t
        private void Anomal2n3(double t, AnomalOptions options)
        {
            //скопировал значения для увеличения читаемосости кода
            double x = Expectation;

            double s = Sigma;

            double a = x - t * s;

            double b = x + t * s;

            #region Одиинаковый код, который позволяет только найти границы без изменения обрабатываемо массива

            BorderA = a;

            BorderB = b;

            if ((options & AnomalOptions.OnlyFindBordersAandB) != 0)
                return;

            #endregion

            d = d.Where(value => value >= a && value <= b)
               .ToArray();
        }

        public void AnomalSecond(AnomalOptions options)
        {
            double t = 1.2 + 3.6 * (1 - KontrExcess) * Math.Log10(d.Length / 10d);

            Anomal2n3(t, options);
        }

        public void AnomalThird(AnomalOptions options)
        {
            double t = 1.55+0.8*Math.Sqrt(Math.Abs(Excess_zsun-1))* Math.Log10(d.Length / 10d);

            Anomal2n3(t, options);
        }

        public void RemoveAnomals(double a, double b)
        {
            d = d.Where(value => value >= a && value <= b)
               .ToArray();
        }
    }

    public enum AnomalOptions
    { 
        None=1,
        OnlyFindBordersAandB=2,
    }
}
