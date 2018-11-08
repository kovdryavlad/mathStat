using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    public partial class STAT
    {
        //это ню
        //считает начальные статистические моменты k-го порядка
        public double InitialMoment(int k)
        {
            double _initial_moment_sum = 0;
            for (int i = 0; i < d.Length; i++)
            {
                _initial_moment_sum += Math.Pow(d[i], (double)k);
            }
            return _initial_moment_sum / d.Length;
        }

        //это мю
        //считает центральные статистические моменты k-го порядка
        private double CentralMoment(int k)
        {
            double _central_moment_sum = 0;
            for (int i = 0; i < d.Length; i++)
            {
                _central_moment_sum += Math.Pow(d[i] - Expectation, (double)k);
            }

            return _central_moment_sum / d.Length;
        }

        //Лаплас переехал в Distributions.Normal

        /// <summary>Ф-я возвращает значение имперической ф-и распределения в точке</summary>
        public double GetEmpireYinPoint(double x)
        {
            double normalizer = 0.5*h;
            if (x < masX[0] - normalizer)
                return 0;
            else if (x > (masX[masX.Length - 1] - normalizer))
                return 1;
            
            //случаи 0 и 1 рассмотрены, теперь смотрим все нормальные значения
            int numberOfclass = 0;

            //пока не входит вкласс увеличиваем номар класса
            while (!(x >( masX[numberOfclass] - normalizer) && x <= masX[numberOfclass] + 0.5*h))
                numberOfclass++;

            return EmpiricalF[numberOfclass];
        }
    }
}
