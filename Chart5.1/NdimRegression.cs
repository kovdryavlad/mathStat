using Chart1._1;
using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chart5._1
{
    class NdimRegression
    {
        public static Vector FindParamsForThreeDim(STAT x1Stat, STAT x2Stat, STAT yStat)
        {
            int n = x1Stat.d.Length;

            //подсчет средних
            x1Stat.CalcMainParams();
            x2Stat.CalcMainParams();
            yStat.CalcMainParams();

            double x1x2 = 0, yx1 = 0, yx2 = 0;

            //перенос массивов в переменные
            var x1 = x1Stat.d;
            var x2 = x2Stat.d;
            var y = yStat.d;

            for (int i = 0; i < n; i++)
            {
                //перенос значений массивов в переменные
                var x1Val = x1[i];
                var x2Val = x2[i];
                var yVal = y[i];

                x1x2 += x1Val * x2Val;
                yx1 += yVal * x1Val;
                yx2 += yVal * x2Val;
            }

            x1x2 /= n;
            yx1 /= n;
            yx2 /= n;

            //перенос средних в переменные
            var x1Expectation = x1Stat.Expectation;
            var x2Expectation = x2Stat.Expectation;
            var yExpectation = yStat.Expectation;


            //заполнение матрицы
            Matrix matrix = new Matrix(3, 3, new[]
            {
                1,                x1Expectation,             x2Expectation,
                x1Expectation,    x1Stat.InitialMoment(2),   x1x2,
                x2Expectation,    x1x2,                      x2Stat.InitialMoment(2)
            });

            Vector b = new Vector(new[] { yExpectation, yx1, yx2 });

            return matrix.Solve(b);
        }

        public static RegressionResult FindParamsForNDim(STATND stats, int[] Xindexes, int Yindex, double alpha, Chart diagnosticdiagram)
        {
            #region Подготовка
            //размерность иксов
            int n = Xindexes.Length;

            //собраем все STAT-ы по указанным индексам
            List<STAT> XStats = Xindexes.Select(index => stats.GetStat(index)).ToList();

            //средние по каждому измерению
            double[] AverageArray = XStats.Select(stat => stat.Expectation).ToArray();

            //вектор средних
            Vector AverageVector = new Vector(AverageArray);

            //значения из многомерной выборки, по нужным индексам 
            List<Vector> XVectorLst = XStats.Select(stat => new Vector(stat.d)).ToList();

            //отнимаем среднее от каждой колонки
            for (int i = 0; i < n; i++)
                XVectorLst[i] -= AverageVector[i];

            //образуем из значений иксов матрицу
            Matrix XMatrix = Matrix.Create.JoinVectors(XVectorLst);

            //работа с игреком
            STAT yStat = stats.GetStat(Yindex);

            double YExpectation = yStat.Expectation;
            Vector YVector = new Vector(yStat.d);

            Vector Y0 = YVector - YExpectation;
            #endregion

            //мое обозначение 
            Matrix A = XMatrix;//= XMatrix.MultiplyOnVectorRow(AverageVector); // не множить а минусовать

            Matrix ATranspose = A.Transpose();

            //ищем an без а0
            var res = ((ATranspose * A).Inverse() * ATranspose).MultiplyOnVectorColumn(Y0);

            //ищем свободный член а0
            double a0 = YExpectation;

            for (int i = 0; i < n; i++)
                a0 -= res[i] * AverageVector[i];

            //расширенный список результатов с а0
            List<double> extendedResultLst = res.GetCloneOfData().ToList();

            extendedResultLst.Insert(0, a0);

            Vector ExtendedparamsVector = Vector.Create.New(extendedResultLst.ToArray());

            //новый метод
            FillDiagnosticDiagram(diagnosticdiagram, Xindexes.Select(index => stats.GetStat(index)).ToList(), stats.GetStat(Yindex), a0, res.GetCloneOfData());
            
            return GetConfidenceIntervalsOnRegressionParametrs(alpha, n, XStats, yStat, YVector, ExtendedparamsVector);
        }

        private static void FillDiagnosticDiagram(Chart diagnosticdiagram, List<STAT> Xlist, STAT YsTAT, double a0, double[] an)
        {
            diagnosticdiagram.Series[0].Points.Clear();
            
            int Nd = YsTAT.d.Length;
            List<double> MasX = new List<double>();
            List<double> MasY = new List<double>();

            for (int i = 0; i < Nd; i++)
            {
                double yl = YsTAT.d[i];
                MasY.Add(yl);

                yl -= a0;

                int xlen = an.Length;
                for (int j = 0; j < xlen; j++)
                    yl -= an[j] * Xlist[j].d[i];


                MasX.Add(yl);
            }

            diagnosticdiagram.Series[0].Points.DataBindXY(MasX, MasY);
        }

        private static RegressionResult GetConfidenceIntervalsOnRegressionParametrs(double alpha, int n, List<STAT> XStats, STAT yStat, Vector YVector, Vector ExtendedparamsVector)
        {
            //это величина доьавлением/отниманием которой получаются доверительные интервалы на параметры регресии 
            double[] confidenceValues = new double[n+1];

            //берем исходные иксы
            List<Vector> XInputVectors = XStats.Select(stat => new Vector(stat.d)).ToList();
            //добавляю игрекв первую колонку
            XInputVectors.Insert(0, YVector);

            Matrix X = Matrix.Create.JoinVectors(XInputVectors);

            //ищем квантиль
            int N = XStats[0].d.Length * n;
            int Nu = N - n; //по формуле N-n
            double kv = Kvantili.Student(alpha, Nu);

            //коэф детерминации
            XStats.Add(yStat);  //добавляем y к иксам, чтобы получить общую матрицу

            STATND STATNDForR = new STATND(XStats.ToArray());   //заносим полученную матрицу в многомерный анализ
            double MultipleR = STATNDForR.GetMultipleR(n);
            double DeterminationCoef = MultipleR * MultipleR;

            //ищем остаточную дисперсию и сигму
            double S2 = (double)(N - 1) / (N - n) * yStat.Dispersia * (1 - DeterminationCoef);
            double sigma = Math.Sqrt(S2);

            //значимость параметров
            string[] paramsState = new string[7];
            double KvForParams = Kvantili.Student(alpha, N - n);

            //ищем сkk
            Matrix C = (X.Transpose() * X).Inverse();

            for (int i = 0; i < n+1; i++)
            {
                //дл интервальных оценок
                double Ckk = C[i, i]; 
                confidenceValues[i] = kv * sigma * Math.Sqrt(Ckk);

                //проверка параметров регрессии на значимость
                paramsState[i] = Math.Abs(ExtendedparamsVector[i] / (sigma * Math.Sqrt(Ckk)))<=KvForParams? "не значущий": "Значущий";
            }

            //переносим из массива в вектор
            Vector ConfidenceVector = new Vector(confidenceValues);

            //доверительные границы
            Vector topLimits = ExtendedparamsVector + ConfidenceVector;
            Vector bottomLimits = ExtendedparamsVector - ConfidenceVector;

            //проверка значимости регрессии
            string state = "";
            double f = (double)(N - n - 1) / n * DeterminationCoef * DeterminationCoef / (1 - DeterminationCoef * DeterminationCoef);
            int Nu1 = n;
            int Nu2 = N - n - 1;
            Func<double, double> r = v => Math.Round(v,4);
            Action<string> Add2Log = s => state += s + Environment.NewLine;

            Add2Log("f = " + r(f));
            Add2Log("kv = " + r(kv));
            if (f > kv)
            {
                Add2Log("f > kv");
                Add2Log("Регресійна модель Значуща");
            }
            else
            {
                Add2Log("f <= kv");
                Add2Log("Регресійна модель НЕ Значуща");
            }

            //Интервалы дисперсии
            double nominator = S2 * (N - n);
            double TopDisp= nominator / Kvantili.Hi2((2d-alpha)/2, N - n);
            double BottomDisp = nominator / Kvantili.Hi2(-alpha/2d, N - n);

            string disprsionIntervals = String.Format("{0}<={1}<={2}", r(BottomDisp), r(S2), r(TopDisp));

            RegressionResult result = new RegressionResult()
            {
                BotoomConfidenceLimits = bottomLimits,
                RegressionParams = ExtendedparamsVector,
                TopConfidenceLimits = topLimits,
                R = DeterminationCoef,
                state = state,
                ParamsState = paramsState,
                DispersionIntervals = disprsionIntervals
            };

            return result;
        }

        
    }

    class RegressionResult
    {
        //параметры регрессии
        public Vector RegressionParams { get; set; }
        public Vector TopConfidenceLimits { get; set; }
        public Vector BotoomConfidenceLimits { get; set; }

        //коэфициент детерминации
        public double R { get; set; }

        //значимость Регрессии
        public string state { get; set; }

        public string[] ParamsState { get; set; }

        public string DispersionIntervals { get; set; }

        public override string ToString()
        {
            string result = ""+Environment.NewLine+"Оцінка параметрів регресії"+Environment.NewLine;

            Func<double, double> r = v => Math.Round(v, 4);
            int n = RegressionParams.Length;

            for (int i = 0; i < n; i++)
                result+=String.Format("a{0}н = {1} \t a{0} = {2}\t a{0}в = {3}\t {4}",i, r(BotoomConfidenceLimits[i]), 
                        r(RegressionParams[i]), r(TopConfidenceLimits[i]), ParamsState[i])+Environment.NewLine;

            result += Environment.NewLine;

            result += "Коефіцієнт детермінації:" + r(R) + Environment.NewLine;

            result += state + Environment.NewLine;

            result += "Довірчі інтервали залишкової дисперсії:" + Environment.NewLine;
            result += DispersionIntervals + Environment.NewLine;

            return result;
        }
    }

}
