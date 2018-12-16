using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Chart1._1;
using SimpleMatrix;

namespace Chart5._1
{
    class STATND
    {
        int n;          //длинна массива stats
        STAT[] stats;   //одномерные выборки

        double[][] R;   //кореляционная матрица
        double[][] DC;  //дисперсионно-ковариационная матрица

        //вариационный ряд
        VariationalSeriesBuilder variationalSeries;

        //полчение элемента массива по индексу
        public STAT GetStat(int index)
        {
            return stats[index];
        }

        public int N { get { return n; } }

        public STATND(STAT[] stats)
        {
            n = stats.Length;
            if (n < 2)
                throw new Exception("Для использования STATND нужна выборка размерностью >=2");

            this.stats = stats;

            // заполняем мат ожидание и дисперсию
            for (int i = 0; i < stats.Length; i++)
                stats[i].CalcMainParams();

            //заполнение вариационного ряда
            variationalSeries = new VariationalSeriesBuilder(stats);

            FillRandDC();
        }

        public string GetAllVariationalSeries()
        {
            return variationalSeries.OutputAllVariationalSeries();
        }

        public void FillRandDC()
        {
            R = ArrayMatrix.GetJaggedArray(n, n);
            DC = ArrayMatrix.GetJaggedArray(n, n);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var si = stats[i];
                    var sj = stats[j];

                    var stat2d = new STAT2D(si, sj);

                    var r = stat2d._koefLinearCorrelation;
                    R[i][j] = r;

                    DC[i][j] = r * si.Sigma * sj.Sigma;
                }
            }
        }

        public Matrix GetR()
        {
            return new Matrix(R);
        }

        public Matrix GetDC()
        {
            return new Matrix(DC);
        }

        public Vector GetAverageVector()
        {
            return new Vector(stats.Select(s => s.Expectation).ToArray());
        }

        public string GetExpectationsString()
        {
            //для удобства 
            string resultStr = "";
            Func<double, double> r = val => Math.Round(val, 4);
            Action<string> mess2log = str => resultStr += str + Environment.NewLine;

            int length = n;
            for (int i = 0; i < length; i++)
            {
                var curStat = stats[i];
                string temp = String.Format("m{0}:\ttн = {1}\tt={2}\ttв={3}\tsigma={4}",
                              i, r(curStat.Expectation_niz), r(curStat.Expectation),
                              r(curStat.Expectation_verh), r(curStat.Sigma_Expectation));

                mess2log(temp);
            }

            return resultStr;
        }

        public string GetSigmasString()
        {
            //для удобства 
            string resultStr = "";
            Func<double, double> r = val => Math.Round(val, 4);
            Action<string> mess2log = str => resultStr += str + Environment.NewLine;

            int length = n;
            for (int i = 0; i < length; i++)
            {
                var curStat = stats[i];
                string temp = String.Format("sigma{0}:\ttн = {1}\tt={2}\ttв={3}\tsigma={4}",
                              i, r(curStat.Sigma_niz), r(curStat.Sigma),
                              r(curStat.Sigma_verh), r(curStat.Sigma_Dispersia));

                mess2log(temp);
            }

            return resultStr;
        }

        //матрица диаграмм разброса
        public void GetMatrixOfScatterDiagrams(TableLayoutPanel tableLayout)
        {
            tableLayout.Controls.Clear();
            tableLayout.ColumnStyles.Clear();
            tableLayout.RowStyles.Clear();

            tableLayout.ColumnCount = tableLayout.RowCount = n;

            for (int i = 0; i < n; i++)
            {
                RowStyle rs = new RowStyle(SizeType.Percent, 100f / n);
                ColumnStyle cs = new ColumnStyle(SizeType.Percent, 100f / n);

                tableLayout.RowStyles.Add(rs);
                tableLayout.ColumnStyles.Add(cs);
            }

            tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;

            for (int i = 0; i < n; i++)
            {
                STAT stat_i = stats[i];
                for (int j = 0; j < n; j++)
                {
                    STAT stat_j = stats[j];

                    Chart chart = new Chart();
                    chart.Dock = DockStyle.Fill;


                    ChartArea chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chart.ChartAreas[0].AxisX.Minimum = stat_i.Min;
                    chart.ChartAreas[0].AxisX.Maximum = stat_i.Max;
                    chart.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
                    chart.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
                    chart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.00}";
                    chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

                    Series s = new Series();

                    if (i == j)
                    {
                        s.BorderColor = Color.Black;
                        s.CustomProperties = "PointWidth=1";
                        stat_i.GetSeria1_Column(s);
                        //chart.Series.Add(s);

                        chart.ChartAreas[0].AxisX.Interval = Math.Round(stat_i.h, 4);
                        chart.ChartAreas[0].AxisY.Maximum = stat_i.masYVid.Max();
                    }
                    else
                    {
                        s.ChartType = SeriesChartType.Point;
                        chart.ChartAreas[0].AxisY.Minimum = stat_j.Min;
                        chart.ChartAreas[0].AxisY.Maximum = stat_j.Max;
                        s.Points.DataBindXY(stat_i.d, stat_j.d);
                    }

                    chart.Series.Add(s);
                    tableLayout.Controls.Add(chart, j, i);
                }
            }
        }

        //проверка на количество реализаций признаков - для параллельных координат
        public bool CheckingLengthOfStats()
        {
            int stat0len = stats[0].d.Length;

            for (int i = 1; i < n; i++)
                if (stat0len != stats[i].d.Length)
                    return false;

            return true;

        }

        //построение диаграммы параллельных координат
        public void BuildparallelCoordsChart(SeriesCollection sCol)
        {
            //полная очиска. Пусть она и будет такой. Был когда-то баг с этим
            int sColLength = sCol.Count;
            for (int i = 0; i < sColLength; i++)
                sCol[i].Points.Clear();

            sCol.Clear();

            int SampleLength = stats[0].d.Length;


            for (int i = 0; i < SampleLength; i++)
            {
                Series series1 = new Series();
                series1.ChartType = SeriesChartType.Point;
                series1.Color = Color.DarkOrange;

                Series series2 = new Series();
                series2.ChartType = SeriesChartType.Line;
                series2.Color = Color.DodgerBlue;

                for (int j = 0; j < n; j++)
                {
                    //series1.Points.AddXY(j, stats[j].d[i]);

                    double value = stats[j].d[i];
                    double max = stats[j].Max;
                    double min = stats[j].Min;

                    double val2output = (value - min) / (max - min);

                    series2.Points.AddXY(j, val2output);
                }


                //sCol.Add(series1);
                sCol.Add(series2);
            }
        }

        //проверка на возможность построния пузырьковой диаграммы
        public bool CheckingBeforeBoubleBuild()
        {
            if (n == 3)
                return true;

            return false;
        }

        //получить узрьковую диаграмму
        public void GetBoubleDiagram(Series seria)
        {
            seria.Points.Clear();

            double maxr = 250000000;       //подбор максимального радиуса

            var array = stats[2].d;
            double MaxRealRadius = array.Max();
            int length = array.Length;

            double[] XArr = stats[0].d;
            double[] YArr = stats[1].d;

            for (int i = 0; i < length; i++)
            {
                double s = array[i];
                double RealR = Math.Sqrt(s / Math.PI);
                double rToOut = RealR * maxr / MaxRealRadius;

                seria.Points.AddXY(XArr[i], YArr[i], rToOut);
            }
        }

        //частичные коэффициент корреляции
        public double GetPartialCoefCorrelation(int i, int j, int[] c)
        {
            int cLen = c.Length;

            if (cLen < 1)
                throw new ArgumentException("Длинна массива с должна быть больше 1, поскольку при подсчете частичного коэф. корреляции хотябы олдно измерение нужно откидывать", "c");

            else if (cLen == 1)
            {
                int d = c[0];

                double rij = R[i][j];
                double rid = R[i][d];
                double rjd = R[j][d];

                return (rij-rid*rjd) / Math.Sqrt((1 - rid * rid) * (1 - rjd * rjd));
            }

            else
            {
                int d = c.Last();
                c = c.GetAllWithoutLast().ToArray();

                double rij = GetPartialCoefCorrelation(i, j, c);
                double rid = GetPartialCoefCorrelation(i, d, c);
                double rjd = GetPartialCoefCorrelation(j, d, c);

                return (rij - rid * rjd) / Math.Sqrt((1 - rid * rid) * (1 - rjd * rjd));
            }
        }

        double[][] PartialR;

        public Matrix GetAllPartialCoefsCorrelation()
        {
            //double[][] PartialR = ArrayMatrix.GetJaggedArray(n, n);
            PartialR = ArrayMatrix.GetJaggedArray(n, n);

            if (n < 3)
                throw new Exception("n должно быть >=3");

            //for (int i = 0; i < n; i++)
            Parallel.For(0, n, (i) =>
              {
                  for (int j = i; j < n; j++)
                  {
                      if (i == j)
                      {
                          PartialR[i][j] = 1;
                          continue;
                      }

                      List<int> c = new List<int>();
                      for (int k = 0; k < n; k++)
                          if (!(k == i || k == j))
                              c.Add(k);

                      PartialR[i][j] = GetPartialCoefCorrelation(i, j, c.ToArray());
                  }
              });

            return new Matrix(PartialR);
        }

        public ExteendedCoef GetPartialCoefCorrelationInfo(int i,int j)
        {
            //значение 
            double rij = PartialR[i][j];

            //значимость
            int w = PartialR.GetLength(0) - 2;

            int NN = PartialR.GetLength(0) * N; 
            double V = NN - w - 2;   //Это чисто мое обозначение для упрощения кол-ва степеней свободы

            double t = (rij * Math.Sqrt(V) / Math.Sqrt(1 - rij * rij));

            string state = Math.Abs(t) < Kvantili.Student(0.05, V) ? "Не значущий" : "Значущий";


            //интервальное оценивание
            double V1 = Math.Log((1 + rij) / (1 - rij)) / 2.0 - Kvantili.Normal(0.05) / (V - 1); 
            double V2 = Math.Log((1 + rij) / (1 - rij)) / 2.0 + Kvantili.Normal(0.05) / (V - 1); 


            double bottomborder = (Math.Exp(2 * V1) - 1) / (Math.Exp(2 * V1) + 1); 
            double Topborder = (Math.Exp(2 * V2) - 1) / (Math.Exp(2 * V2) + 1);

            return new ExteendedCoef()
            {

                info = Environment.NewLine+"Частковий коефіцієнт кореляції r(" + i + ")(" + j+"):",
                BottomValue = bottomborder,
                Value = rij,
                TopValue = Topborder,
                State = state
            };
        }

        //множественные коэф. корреляции
        double[] MultipleR;

        //множественные коэффициенты корреляции
        public Vector GetMultipleCoefsCorrelation()
        {
            //множественные коэффициенты корреляции
            MultipleR = new double[n];

            //большая дельта
            Matrix matrix = new Matrix(R);
            double CommonDeterminant = matrix.Determinant();

            for (int i = 0; i < n; i++)
            {

                double minorDeterminant = 0;
                try
                {

                    Matrix minor = matrix.GetMinor(i, i);
                    minorDeterminant = minor.Determinant();

                    MultipleR[i] = Math.Sqrt(1 - CommonDeterminant / minorDeterminant);
                }
                catch
                {
                    MultipleR[i] = double.NaN;
                    continue;
                }
            }

            return new Vector(MultipleR);
        }

        //множественный коэффициент корреляции
        public double GetMultipleR(int i)
        {
            double result = 0;

            Matrix matrix = new Matrix(R);
            double CommonDeterminant = matrix.Determinant();

            double minorDeterminant = 0;
            try
            {

                Matrix minor = matrix.GetMinor(i, i);
                minorDeterminant = minor.Determinant();

                result = Math.Sqrt(1 - CommonDeterminant / minorDeterminant);
            }
            catch
            {
                result = double.NaN;
            }

            return result;
        }

        //значимость множественных коэф корреляции
        public string SignificanceOfMultipleR(int i)
        {
            int NN = stats[0].d.Length * N;

            double r = MultipleR[i];

            double f = (NN - n - 1) / (double)n * r * r / (1 - r * r);


            string res = f > Kvantili.Fishera(0.05, n, NN - n - 1) ? "Значущий" : "Не значущий";// or <  ! но пока пусть будет так

            return String.Format("{1}Множинний коефіцієнт ({0}) - {2}", i, Environment.NewLine, res);
        }

        //значимость всех множественных коэф.
        public string SignificanceOfALLMultipleR()
        {
            string res= "";

            for (int i = 0; i < N; i++)
                res += SignificanceOfMultipleR(i);

            return res;
        }

        internal void SaveInFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                int size = N;
                int Nd = stats[0].d.Length;


                for (int i = 0; i < Nd; i++)
                {
                    string str = "";

                    for (int j = 0; j < size; j++)
                    {
                        str += stats[j].d[i] + ";";
                    }

                    writer.WriteLine(str);
                }
            }
        }

        public LaverierFadeevaExtendedResult[] MGKInfo;

        //7семестр
        public LaverierFadeevaExtendedResult[] MGKGetinfo()
        {
            Matrix r = new Matrix(R);
            var laverierFadeevaResult = LaverierFadeevaMethod.Solve(r, LaverierFadeevaSolvingOptions.FullSolving);
            var extended = MGKInfo =  LaverierFadeevaExtendedResult.ConvertToExtendedLaverierFadeevaResult(laverierFadeevaResult);

            
            return extended;
        }

        internal void UseDirectTransitionMGK(int value)
        {
            var originalData = stats.Select(stat => stat.InputData).ToArray();
            Matrix originalDataMatrix = new Matrix(originalData);

            originalDataMatrix = originalDataMatrix.Transpose();

            for (int i = 0; i < value; i++)
            {
                MGKInfo[i].includeInMGK = true;
            }


            List<double[]> transitionMatrixArrList = MGKInfo.Where(v=>v.includeInMGK).Select(v=>v.eigenVector.GetCloneOfData()).ToList();
            Matrix transitionMatrix = Matrix.Create.JoinVectors(transitionMatrixArrList);


            Matrix MGKDirectResult = originalDataMatrix * transitionMatrix;
        }
    }

    
    public static class IEnumerableHelper
    {
        public static IEnumerable<T> GetAllWithoutLast<T>(this IEnumerable<T> inputLst)
        {
            List<T> list = inputLst.ToList();
            List<T> Outlist = new List<T>();

            int count = list.Count;

            for (int i = 0; i < count - 1; i++)
                Outlist.Add(list[i]);

            return Outlist;
        }
    }


    public class ExteendedCoef
    {
        public string info { get; set; }
        public double Value { get; set; }
        public double BottomValue { get; set; }
        public double TopValue { get; set; }
        public string State { get; set; }

        public override string ToString()
        {
            Func<double, double> r = v => Math.Round(v, 4);

            string result = "";
            Action<string> mess2log = str => result += str + Environment.NewLine;


            mess2log(info);
            mess2log("Нижня межа\tЗначення\tВерхня межа\tЗнаущість");
            mess2log(String.Format("{0}\t{1}\t{2}\t{3}", r(BottomValue), r(Value), r(TopValue), State));

            return result;
        }
    }
}
