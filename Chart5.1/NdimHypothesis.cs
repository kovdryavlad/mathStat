using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chart1._1;
using SimpleMatrix;

namespace Chart5._1
{
    class NdimHypothesis
    {
        //равенство двух многомерных средних в случае равных ДК
        public static string ComparingTwoNdimAverage(STATND xStat, STATND yStat, double alpha)
        {
            //проверки
            if (!xStat.CheckingLengthOfStats())
                throw new ChekingLengthOfStatsException("xStat");
            if (!yStat.CheckingLengthOfStats())
                throw new ChekingLengthOfStatsException("yStat");

            int nLen1 = xStat.N;
            int nLen2 = yStat.N;

            if (nLen1 != nLen2)
                throw new DIfferentDimentionsLengthException();
            //конец проверок

            //вынос нужных переменных
            int N1 = xStat.GetStat(0).d.Length;  //длинна всех признаков по иксу
            int N2 = yStat.GetStat(0).d.Length;  //длинна всех признаков по y
            int n = nLen1;                       //количество признаков (должно быть одинаковым для x и y)

            double[][] S0 = ArrayMatrix.GetJaggedArray(n, n);
            double[][] S1 = ArrayMatrix.GetJaggedArray(n, n);

            //лямбды для удобного получения x и y
            Func<int, int, double> x = (NumberOfDim, NumberOfElementInDim) => xStat.GetStat(NumberOfDim).d[NumberOfElementInDim];
            Func<int, int, double> y = (NumberOfDim, NumberOfElementInDim) => yStat.GetStat(NumberOfDim).d[NumberOfElementInDim];
            
            //лямбды для получения суммы
            Func<int, double> xSum = NumberOfDim => xStat.GetStat(NumberOfDim).d.Sum();
            Func<int, double> ySum = NumberOfDim => yStat.GetStat(NumberOfDim).d.Sum();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double CommonDenominator = N1+N2-2;

                    double S0Temp = 0;
                    double S1Temp = 0;

                    //по формулам
                    for (int l = 0; l < N1; l++)
                        S0Temp += x(i, l) * x(j, l);

                    for (int l = 0; l < N2; l++)
                        S0Temp += y(i, l) * y(j, l);

                    S1Temp = S0Temp; //до этого места формулы одинаковые

                    //подсчет частей, которые отличаются
                    S0Temp -= ((xSum(i) + ySum(i)) * (xSum(j) + ySum(j))) / (N1 + N2);

                    S1Temp -= (xSum(i) * xSum(j)) / N1;
                    S1Temp -= (ySum(i) * ySum(j)) / N2;

                    //перенос значений в матрицу
                    S0[i][j] = S0Temp / CommonDenominator;
                    S1[i][j] = S1Temp / CommonDenominator;
                }
            }

            //перенос значений в сложный тип - матрицу(для нахождения определителей)
            Matrix S0Matrix = new Matrix(S0);
            Matrix S1Matrix = new Matrix(S1);

            //подсчет статистики
            double V = N1 + N2 - 2 - n / 2d;
            V *= Math.Log(Math.Abs(S1Matrix.Determinant() / S0Matrix.Determinant()));

            //для удобства 
            string resultStr = "";
            Func<double, double> r = val => Math.Round(val, 4);
            Action<string> mess2log = str => resultStr += str + Environment.NewLine;

            //делаем выводы
            double kv = Kvantili.Hi2(alpha, n);

            mess2log("V = " + r(V));
            mess2log("kv = " + r(kv));
            
            if (V <= kv)
            {
                //гипотезу принято;//средние совпадают
                mess2log("V<kv");
                mess2log("Гіпотезу прийнято!");
                mess2log("Середні співпадають!");
            }
            else
            {
                //гипотезу не принято
                mess2log("V>kv");
                mess2log("Гіпотезу НЕ прийнято!");
                mess2log("Середні НЕ співпадають!");
            }

            return resultStr;
        }

        //совпадение к н-меных  средних при расхождении ДК матриц
        public static string ComparingKNdimAverage(STATND[] stats, double alpha)
        {
            int k, n;
            int[] Nd;
            Vector[] Averagexd;
            double[] Sd;
            //начало работы с к н-мерными = заполнение переменных
            StartWorkWithKNdim(stats, out k, out n, out Nd, out Averagexd, out Sd);

            //подсчет обобщенного среднего
            Vector GenerilizedAverage;          //само обобщенное среднее
            double leftPart = 0;
            Vector RightPart = Vector.Create.New(n);
            for (int i = 0; i < k; i++)
            {
                double commonPart = Nd[i] / Sd[i];

                leftPart += commonPart;
                RightPart += commonPart * Averagexd[i];
            }

            GenerilizedAverage = RightPart / leftPart;  //обобщенное среднее подсчитано

            double V = 0;   //результирующая статистика

            for (int i = 0; i < k; i++)
            {
                Vector v = Averagexd[i] - GenerilizedAverage;
                V += Nd[i] * v / Sd[i] * v;
            }

            //делаем выводы
            double kv = Kvantili.Hi2(alpha, n * (k - 1));

            //для удобства 
            string resultStr = "";
            Func<double, double> r = val => Math.Round(val, 4);
            Action<string> mess2log = str => resultStr += str + Environment.NewLine;

            mess2log("V = " + r(V));
            mess2log("kv = " + r(kv));
            
            if (V <= kv)
            {
                //гипотезу принято; средние совпадают
                mess2log("V<kv");
                mess2log("Гіпотезу прийнято!");
                mess2log("Середні співпадають!");
            }
            else
            {
                //гипотезу не принято
                mess2log("V>kv");
                mess2log("Гіпотезу НЕ прийнято!");
                mess2log("Середні НЕ співпадають!");
            }

            return resultStr;
        }

        private static void StartWorkWithKNdim(STATND[] stats, out int k, out int n, out int[] Nd, out Vector[] Averagexd, out double[] Sd)
        {
            k = stats.Length;
            n = stats[0].N;

            //обработка ошибок
            HandleExceptions(stats, k, n);
            //заполняем Nd
            Nd = GetNd(stats, k);

            //получаем вектора средних значений для каждой из к выборок
            Averagexd = stats.Select(st => st.GetAverageVector()).ToArray();

            //подсчет Sd
            Sd = GetSd(stats, k, Nd, Averagexd);
        }

        private static void HandleExceptions(STATND[] stats, int k, int n)
        {
            #region Выброс ошибкок при некорректном вводе параметров
            if (stats.Length < 2)
                throw new ArgumentException("Недопустимо малый размер входного массива", "stats");

            for (int i = 0; i < k; i++)
                if (!stats[i].CheckingLengthOfStats())
                    throw new ChekingLengthOfStatsException(String.Format("stats[{0}]", i));


            for (int i = 1; i < k; i++)
                if (stats[i].N != n)
                    throw new DIfferentDimentionsLengthException();

            #endregion
        }

        private static int[] GetNd(STATND[] stats, int k)
        {
            int[] Nd = new int[k];

            for (int i = 0; i < k; i++)
                Nd[i] = stats[i].GetStat(0).d.Length;

            return Nd;
        }

        //получение Sd
        private static double[] GetSd(STATND[] stats, int k, int[] Nd, Vector[] Averagexd)
        {
            double[] Sd = new double[k];

            for (int d = 0; d < k; d++)
            {
                List<double[]> ListOfDimentions = new List<double[]>(); //список измерений
                int lengthOfDimentions = stats[d].N;
                for (int i = 0; i < lengthOfDimentions; i++)
                    ListOfDimentions.Add(stats[d].GetStat(i).d);

                //собираем все измерения в матрицу
                Matrix JoinedInMatrixDimentions = Matrix.Create.JoinVectors(ListOfDimentions);
                //транспонируем ее для более быстрого по времени доступа к строкам (теперь нужно брать столбец)
                Matrix Transposed = JoinedInMatrixDimentions.Transpose();

                double SumForDIvide = 0; //по формуле сумма которая,  будет делиться на Nd-1 

                int CurrentNd = Nd[d];   //длинна признаков в каждой многомерной выборке
                for (int l = 0; l < CurrentNd; l++)
                {
                    Vector leftPart = Transposed.GetColumn(l) - Averagexd[d];
                    Vector rightpart = (Vector)leftPart.Clone();

                    SumForDIvide += leftPart * rightpart;
                }

                Sd[d] = SumForDIvide / (CurrentNd - 1);
            }

            return Sd;
        }

        //совпадение к н-меных ДК матриц
        public static string ComparingKNdimDC(STATND[] stats, double alpha)
        {
            int k, n;
            int[] Nd;
            Vector[] Averagexd;
            double[] Sd;
            //начало работы с к н-мерными = заполнение переменных
            StartWorkWithKNdim(stats, out k, out n, out Nd, out Averagexd, out Sd);

            double S = 0;
            int N = 0;

            for (int i = 0; i < k; i++)
            {
                S += Sd[i] * (Nd[i] - 1);
                N += Nd[i];
            }
            S /= (N - k);

            double V = 0;   //статистика
            for (int i = 0; i < k; i++)
                V += (Nd[i] - 1) / 2d * Math.Log(Math.Abs(S / Sd[i]));

            //делаем выводы
            double kv = Kvantili.Hi2(alpha, n *(n+1) *(k - 1)/2);

            //для удобства 
            string resultStr = "";
            Func<double, double> r = val => Math.Round(val, 4);
            Action<string> mess2log = str => resultStr += str + Environment.NewLine;

            mess2log("V = " + r(V));
            mess2log("kv = " + r(kv));
            
            if (V <= kv)
            {
                //гипотезу принято; ДК совпадают
                mess2log("V<kv");
                mess2log("Гіпотезу прийнято!");
                mess2log("DC співпадають!");
            }
            else
            {
                //гипотезу не принято
                mess2log("V>kv");
                mess2log("Гіпотезу НЕ прийнято!");
                mess2log("DC НЕ співпадають!");

            }

            return resultStr;
        }
    }

    class ChekingLengthOfStatsException:Exception
    {
        public ChekingLengthOfStatsException(string paramName)
            :base("Разная размерность признаков в "+ paramName)
        {
            
        }
    }

    class DIfferentDimentionsLengthException :Exception
    {
        public DIfferentDimentionsLengthException()
            :base("Разное количество признаков при сравнении многомерных выборок")
        {

        }
    }
}
