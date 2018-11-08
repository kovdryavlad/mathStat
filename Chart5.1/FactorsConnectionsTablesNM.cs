using Chart1._1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    //коэффициенты сообщений таблиц n на m
    class FactorsConnectionsTablesNM
    {
        //для подсчетов значений нужных коэфициентов
        double[][] nij;     //сама таблица
        double[] m;         //суммы по столбцам
        double[] n;         //суммы по строкам
        double alpha;       //для гипотезы

        //следующие элементы вынесены с целью оптимизации
        int N;              //размер данных
        double[] xData;     //массив по иксу
        double[] yData;     //массив по игреку
        double xMin;        //минимумы
        double yMin;
        double hx;          //шаг по иксу
        double hy;          //шаг по у
        int SizeM;          //столбцов в таблице
        int SizeN;          //строк в таблице

        //Вынесены в область видимости полей класса для проверки значимости
        double kendal;      //статистика Кендалла. 
        double stuart;      //статистика Стюарта. 

        //конструктор
        public FactorsConnectionsTablesNM(STAT x, STAT y, int SizeN, int SizeM, double alpha)
        {
            //заполняем данные
            this.alpha = alpha;

            xData = x.d;
            yData = y.d;

            //размерность таблицы
            this.SizeM = SizeM;
            this.SizeN = SizeN;

            //объем данных
            N = xData.Length;

            //минимумы
            xMin = x.Min;
            yMin = y.Min;

            //шаги
            hx = (x.Max - x.Min) / SizeM + 0.0001;    // x от  до m
            hy = (y.Max - y.Min) / SizeN + 0.00001;   // y от  до n

            //создание массивов для дальнейших подсчетов. 
            m = new double[SizeM];      //суммы в конце столбцов
            n = new double[SizeN];      //суммы в конце строк

            nij = new double[SizeN][];  //сама таблица

            for (int i = 0; i < SizeN; i++)
                nij[i] = new double[SizeM];

            //вызов ф-и заполнения полей
            Perform_Table_Links();

            //Посчитать суммы
            calcSumm();
        }

        //считаем значения в массивах m и n
        private void calcSumm()
        {
            //заполняем n
            for (int i = 0; i < SizeN; i++)
            {
                double SumN = 0;

                for (int j = 0; j < SizeM; j++)
                {
                    //зафаксировали строку, пробежались по столбикам, взяли сумму.
                    SumN += nij[i][j];
                }

                n[i] = SumN;
            }

            //заполняем m
            for (int i = 0; i < SizeM; i++)
            {
                double SumM = 0;

                for (int j = 0; j < SizeN; j++)
                {
                    SumM += nij[j][i];
                }

                m[i] = SumM;
            }

        }

        //заполняем поля для дальнейших подсчетов. (таблица и суммы)
        void Perform_Table_Links()
        {
            //связываем икс и у
            Tuple<double, double>[] lst = new Tuple<double, double>[N];

            for (int i = 0; i < N; i++)
                lst[i] = Tuple.Create(xData[i], yData[i]);

            string s = "";

            string check = "";
            //i  - строка j столбец 
            //x по строкам y по столбцам
            for (int i = 0; i < N; i++)
            {
                int iIndex = (int)Math.Truncate((xData[i] - xMin) / hx);
                int jIndex = (int)Math.Truncate((yData[i] - yMin) / hy);


                //check += String.Format("[{0}][{1}]\n", iIndex, jIndex);

                //!опытным путем выяснены перевмены индексов местами
                nij[jIndex][iIndex]++;

                s += String.Format("\n({0},{1})\tis in\t({2:0.0000}-{3:0.0000};\t{4:0.0000}-{5:0.0000})",
                                   xData[i], yData[i],
                                  hx * iIndex + xMin, hx * (iIndex + 1) + xMin,
                                  hy * jIndex + yMin, hy * (jIndex + 1) + yMin
                                  );
            }

            //

        }

        double Hi2;

        //проверка незавизимости X, Y
        public string IndependenceHypothesis()
        {
            CalcHi2();

            string result = "Перевірка незалежності X та Y";
            double kv = Kvantili.Hi2(alpha, (SizeM - 1) * (SizeN - 1));

            result += String.Format("Hi2 = {0}\nKvantil = {1}\n", Hi2, kv);

            if (Hi2 > kv)
                result += "Гіпотеза вірна";
            else
                result += "Гіпотеза НЕ вірна\nІснує зв'язок";

            return result;
        }

        //Подсчет Hi2
        private void CalcHi2()
        {
            for (int i = 0; i < SizeN; i++)
            {
                for (int j = 0; j < SizeM; j++)
                {
                    //это по локальной формуле
                    double Nij = n[i] * m[j] / N;

                    Hi2 += Math.Pow(nij[i][j] - Nij, 2);
                }
            }
        }

        //коэффициент сообщений Пирсона
        public double СoefficientConnectionsPirsona()
        {
            CalcHi2();

            return Math.Sqrt(Hi2 / (N + Hi2));
        }

        //значимость коеф сообщений Пирсона
        public string СoefficientConnectionsPirsona_significance()
        {
            double C = Math.Sqrt(Hi2 / (N + Hi2));
            double nu = (SizeN - 1) * (SizeM - 1);
            double Hi2k = Kvantili.Hi2(1 - alpha, nu);
            string significance = "C = " + Math.Round(C, 4) + Environment.NewLine + "Hi^2(" + (1 - alpha) + ", " + nu + ")= " + Math.Round(Hi2k, 4) + Environment.NewLine;
            significance += "Hi^2 = " + Math.Round(Hi2, 4) + Environment.NewLine;
            if (Hi2 >= Hi2k)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }

        //для Кендалла и Стюарта
        double P = 0, Q = 0, T1 = 0, T2 = 0;

        //заполняем поля для Кендалла и Стюарта
        private void CalcPandQ()
        {
            #region считаем P

            for (int i = 0; i < SizeN; i++)
            {
                for (int j = 0; j < SizeM; j++)
                {
                    //вторая пара сумм (в скобках) 
                    double sumInBrackets = 0;

                    for (int k = i + 1; k < SizeN; k++)
                        for (int l = 0; l < SizeM; l++)
                            sumInBrackets += nij[k][l];

                    P += nij[i][j] * sumInBrackets;
                }
            }

            #endregion

            #region считаем Q

            for (int i = 0; i < SizeN; i++)
            {
                for (int j = 0; j < SizeM; j++)
                {
                    //вторая пара сумм (в скобках) 
                    double sumInBrackets = 0;

                    for (int k = i + 1; k < SizeN; k++)
                        for (int l = 0; l < j - 1; l++)
                            sumInBrackets += nij[k][l];

                    Q += nij[i][j] * sumInBrackets;
                }
            }

            #endregion
        }

        //посчитать T1 И T2
        private void CalcT1T2()
        {
            #region считаем T1

            for (int i = 0; i < SizeN; i++)
                T1 += n[i] * (n[i] - 1);


            T1 /= 2d;

            #endregion

            #region считаем T2

            for (int j = 0; j < SizeM; j++)
                T2 += n[j] * (n[j] - 1);


            T2 /= 2d;


            #endregion
        }

        //мера связи Кендалла
        public double MeasureOfConnectionKendella()
        {
            if (SizeM != SizeN)
                throw new Exception("Этот коэффициент считается только для m==n");

            //посчитать P и Q
            CalcPandQ();
            //посчитать T1 и T2
            CalcT1T2();

            //общее под корнем
            double Common = 0.5 * N * (N - 1);

            //результат
            double MeasureKendella = (P - Q) / Math.Sqrt((Common - T1) * (Common - T2));

            //это заполнение поля, к логике метода не относится
            this.kendal = MeasureKendella;

            return MeasureKendella;
        }

        //значимость статистики Кендалла
        public string MeasureOfConnectionKendella_significance()
        {
            double Tau = kendal;
            double Uk = Kvantili.Normal(alpha / 2.0);
            double U = 3 * Tau / Math.Sqrt(2 * (2 * N + 5)) * Math.Sqrt(N * (N - 1));
            string significance = "Tau = " + Math.Round(Tau, 4) + Environment.NewLine + "U(" + (1 - alpha / 2.0) + ")= " + Math.Round(Uk, 4) + Environment.NewLine;
            significance += "U = " + Math.Round(U, 4) + Environment.NewLine;
            if (Math.Abs(U) <= Uk)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }

        //статистика Стюарта
        public double StatStuarta()
        {
            if (SizeM == SizeN)
                throw new Exception("Этот метод для m!=n. Смотри книгу!");

            //посчитать P и Q
            CalcPandQ();

            double min = Math.Min(SizeM, SizeN);

            double Stuart = (2 * (P - Q) * min) / (N * N * (min - 1));

            //это заполнение поля(оно не связано с логикой данного метода/ нужно для проверки значимости)
            this.stuart = Stuart;

            return Stuart;
        }

        //значимость Стюарта
        public string CheckSignificanceOfStuart()
        {
            //минимум
            double min = Math.Min(SizeM, SizeN);

            //сумма под корнем
            double Sum = 0;

            for (int i = 0; i < SizeN; i++)
            {
                for (int j = 0; j < SizeM; j++)
                {
                    double A = 0, B = 0;

                    //считаем А
                    for (int k = 0; k < SizeN; k++)
                        for (int l = 0; l < SizeM; l++)
                            A += nij[k][l];

                    for (int k = 0; k < i - 1; k++)
                        for (int l = 0; l < j - 1; l++)
                            A += nij[k][l];

                    //считаем B
                    for (int k = 0; k < SizeN; k++)
                        for (int l = 0; l < j - 1; l++)
                            B += nij[k][l];

                    for (int k = 0; k < i - 1; k++)
                        for (int l = 0; l < SizeM; l++)
                            B += nij[k][l];

                    //считаем саму сумму
                    Sum += nij[i][j] * Math.Pow(A - B, 2);
                }
            }

            double sigma = 2 * min / (N * N * N * (min - 1)) * Math.Sqrt(N * N * Sum - 4 * N * (P - Q));

            //проверка на значимость(работа с квантилем)
            double Tau = this.stuart;

            double tk = Kvantili.Student(alpha, SizeM + SizeN - 2);
            double t = Tau / sigma;
            string significance = "Tau = " + Math.Round(Tau, 4) + Environment.NewLine + "t(" + (1 - alpha / 2.0) + ", " + (SizeM + SizeN - 2) + " )= " + Math.Round(tk, 4) + Environment.NewLine;
            significance += "T = " + Math.Round(t, 4) + Environment.NewLine;
            if (Math.Abs(t) <= tk)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }
    }
}
