using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    public partial class STAT
    {
        double AlphaForKZKolmogorov;

        public event EventHandler<KZKolmogorovEventArgs> TestingKolmogorov;

        public string KZKolvogorov()
        {
            AlphaForKZKolmogorov = 0.05;

            double DNp = Math.Abs(Seria3Y[0] - _FTeor[0]);
            double DNm = 0;
 
            for (int i = 1; i < d.Length; i++)
            {
                DNp = Math.Max(DNp, Math.Abs(Seria3Y[i] - _FTeor[i]));

                DNm = Math.Max(DNm, Math.Abs(Seria3Y[i] - _FTeor[i - 1]));
            }

            double z = Math.Sqrt(d.Length) * Math.Max(DNp, DNm);

            double sum = 0;

            int k = 0;

            double temp = 0;

            do
            {
                k++;

                double f1 = k * k - 0.5 * (1 - Math.Pow(-1, k));

                double f2 = 5 * k * k + 22 - 7.5 * (1 - Math.Pow(-1, k));

                #region temp-овые переменные, для упрощения большой суммы

                double a = Math.Pow(-1, k) * Math.Exp(-2 * k * k * z * z);

                double b = 1 - 2 * k * k * z / (3 * Math.Sqrt(d.Length));

                double c = 1d/(18*d.Length)*((f1-4*(f1+3))*k*k*z*z+8*Math.Pow(k,4)*Math.Pow(z,4));
                
                //d использовать нельзя

                double e = k * k * z / (27 * Math.Pow(d.Length, 3d / 2)) * 
                           (f2 * f2 / 5d - 4 * (f2 + 45) * k * k * z * z / 15 + 8 * Math.Pow(k, 4) * Math.Pow(z, 4));

                #endregion
                
                temp = a * (b - c + e);

                sum += temp;
                               
            } while (temp>=0.0001);

            double Kz = 1 + 2 * sum;

            double Pz = 1 - Kz;

            //Вывод решения решение
            string result;

            if (Pz >= AlphaForKZKolmogorov)
                result = "Пройдено";
            else
                result = "Не пройдено";

            if (TestingKolmogorov != null)
                TestingKolmogorov(this, new KZKolmogorovEventArgs(
                    AlphaForKZKolmogorov,DNp, DNm, z, k, Kz, Pz, result));

            return result;
        }

        public string KZPirsona()
        {
            AlphaForKZKolmogorov=0.05;

            double min = 0;
            double max = 0;
            double sum = 0;

            for (int i = 0; i < M; i++)
            {
                min = Min + h * i;
                max = min + h;

                double n0= _ReproductDistribution.F(max) - _ReproductDistribution.F(min);

                n0 *= d.Length;

                double ni = masY[i];

                sum+=Math.Pow((ni-n0),2)/n0;
            }
                       
            string result = "";
            double kv = Kvantili.Hi2(AlphaForKZKolmogorov, M - 1);

            Action<string> add2log = v => result += v + Environment.NewLine;
            Func<double, double> r = v => Math.Round(v);
            add2log("kv = " + r(kv));
            add2log("sum = " + r(sum));
            if (sum < kv)
            {
                add2log("sum < kv");
                add2log("Пройдено");
            }
            else
            {
                add2log("sum >= kv");
                add2log("Не пройдено");
            }
            System.Windows.Forms.MessageBox.Show(result);

            return result;
        }
    }
}
