using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Disrtibutions;
using SetsOpearations;
using System.Drawing;
using Chart5._1;

namespace Chart1._1
{
    //класса варианта для вида сверху на гистограмму
    class Varianta
    {
        public double leftBorder;
        public double topBorder;
        public double bottomBorder;
        public double rightBorder;


        public int n;
        public double p;

        public void SetLeftBorder(double leftBorder, double hx)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = leftBorder + hx;
        }
        public void SetBottomBorder(double bottomBorder, double hy)
        {
            this.bottomBorder = bottomBorder;
            this.topBorder = bottomBorder + hy;
        }

        public double MiddleX
        {
            get { return (leftBorder + rightBorder) / 2d; }
        }
        public double MiddleY
        {
            get { return (bottomBorder + topBorder) / 2d; }
        }
    }

    //первичный анализ двумерных данных
    class STAT2D
    {
        //для вывода сообщений 
        MessageService _messageService = new MessageService();

        //
        private SeriesCollection _serCollection;

        public STAT _x = new STAT();
        public STAT _y = new STAT();


        public int N;               //количество строк в файле
        int Mx;                     //кол-во классов по х
        int My;                     //кол-во классов по у
        double hx;                  //шаг по х
        double hy;                  //шаг по у
        
        public int _NumberOfCOlors = 13;    //количество оттенков цвета на чарте

        //уровень квантиля - для нижних и верхних интервалов
        public double alpha = 0.05;

        public Varianta[][] _var;

        //коэффициент корреляции
        public double _koefLinearCorrelation;
        public double _koefLinearCorrelation_niz;
        public double _koefLinearCorrelation_verh;

        //т тест
        public double _t;

        public STAT2D(STAT xForKvaz, STAT yForKvaz)
        {
            _x = xForKvaz;
            _y = yForKvaz;


            N = _x.d.Length;

            GetKoefLinearCorrelation();

        }

        public STAT2D(double[] x, double[] y)
        {
            _x = new STAT();
            _y = new STAT();

            //!! Клон не убирать. Потому что отсортирует. когда это ненужно
            Action act1 = () => 
            {
                _x.Setd2Stat2d((double[])x.Clone());
                //_x.CommonPartOfSetd();
                //_x.RestoreUnsortedValues();
            };
            Action act2 = () =>
            {
                _y.Setd2Stat2d((double[])y.Clone());
                //_y.CommonPartOfSetd();
                //_y.RestoreUnsortedValues();
            };

            Task t1 = Task.Factory.StartNew(act1);
            Task t2 = Task.Factory.StartNew(act2);

            Task.WaitAll(new[] { t1, t2 });


            N = _x.d.Length;

            GetKoefLinearCorrelation();

            this.Mx = _x.M;
        }


        //конструктор
        public STAT2D(string filename)
        {
            ReadFromFile(filename);
        }

        //чтение из файла
        public void ReadFromFile(string filename)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            using (StreamReader r = new StreamReader(filename))
            {
                string str;
                while ((str = r.ReadLine()) != null)
                {
                    var a = str.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    x.Add(Convert.ToDouble(a[0].Replace('.', ',')));
                    y.Add(Convert.ToDouble(a[1].Replace('.', ',')));
                }
            }

            Action act1 = () =>
            {
                _x.Setd2Stat2d(x.ToArray());
                //_x.CommonPartOfSetd();
                
            };
            Action act2 = () =>
            {
                _y.Setd2Stat2d(y.ToArray());
                //_y.CommonPartOfSetd();
               
            };

            Task t1 = Task.Factory.StartNew(act1);
            Task t2 = Task.Factory.StartNew(act2);



            Task.WaitAll(new[] { t1, t2 });

            this.N = x.Count;
            this.Mx = _x.M;

        }

        
        //корреляционное поле
        public void GetKorrelationField(Series s)
        {
            s.Points.Clear();

            s.Points.DataBindXY(_x.d, _y.d);
        }

        //гистограмма сверху
        public void GetTwoDimGist(SeriesCollection sc)
        {
            GetTwoDimGist(sc, Mx);
        }
        public void GetTwoDimGist(SeriesCollection sc, int Mx)
        {
            GetTwoDimGist(sc, Mx, Mx);
        }
        public void GetTwoDimGist(SeriesCollection sc, int Mx, int My)
        {
            _serCollection = sc;
            //формирование вариант
            FormVariants(Mx, My);
            //подсчет значений для варианты
            CalcVariantsValues(Mx, My);

            //тут нужно нарисовать
            _serCollection.Clear();
            DrawVariantsOnChart();
        }

        public double ColorH;

        List<List<PointD>> _pointsOfSeries = new List<List<PointD>>();
        List<Color> _colorsList = new List<Color>();

        private void DrawVariantsOnChart()
        {
            var VariantsArray = _var.Select(x => x).SelectMany(y => y).AsParallel().ToArray();
            //sc.Clear();
            //находим максимальную частоту
            double maxFrequency = VariantsArray.Max(z => z.p);
            //находим шаг для каждого оттенка
            ColorH = maxFrequency / _NumberOfCOlors;

            for (int i = 1; i < _NumberOfCOlors; i++)
            {
                var Variants2Draw = VariantsArray.Where(z => z.p > ColorH * i && z.p <= ColorH * (i + 1)).AsParallel().ToArray();
                
                foreach (Varianta item in Variants2Draw)
                {
                    int lines = 100;//полосок в одном прямоугольничке
                    double yh2lines = (item.topBorder - item.bottomBorder) / lines;
                    double xh2lines = (item.rightBorder - item.leftBorder) / lines;
                    var v = i * ColorH / maxFrequency * 255;

                    Series s = new Series();

                    List <PointD> localList = new List<PointD>();
                    for (int j = 0; j < lines; j++)
                    {
                        
                                s.ChartType = SeriesChartType.Line;

                                double y = item.bottomBorder + j * yh2lines;
                                s.Points.AddXY(item.leftBorder, y);
                                s.Points.AddXY(item.rightBorder, y);

                        localList.Add(new PointD(item.leftBorder, y));
                        localList.Add(new PointD(item.rightBorder, y));

                      
                    }
                    //s.BorderWidth = 2;
                    s.Color = System.Drawing.Color.FromArgb(255, ((int)(((byte)(255 - v)))), ((int)(((byte)(255 - v)))), ((int)(((byte)(255 - v)))));
                    _colorsList.Add(s.Color);
                    _pointsOfSeries.Add(localList);

                    _serCollection.Add(s);
                }
            }
        }
        
        //формирование вариант
        private void FormVariants(int Mx, int My)
        {
            _x.Seth(Mx);
            _y.Seth(My);
            this.Mx = Mx;
            this.My = My;

            _var = new Varianta[Mx][];
            for (int i = 0; i < Mx; i++)
                _var[i] = new Varianta[My];

            hx = _x.h;
            hy = _y.h;

            //double hx = (Xmax-XMin)/Mx;

            for (int i = 0; i < Mx; i++)
            {
                for (int j = 0; j < My; j++)
                {
                    _var[i][j] = new Varianta();

                    _var[i][j].SetLeftBorder(i * hx + _x.Min, hx);
                    _var[i][j].SetBottomBorder(j * hy + _y.Min, hy);
                }
            }
        }

        //подсчет значений
        private void CalcVariantsValues(int Mx, int My)
        {
            string s = "";

            var dataX = _x.d;
            var dataY = _y.d;
            double Xmin = _x.Min;
            double Ymin = _y.Min;

            // int z = 0;

            //for (int i = 0; i < dataX.Length; i++)
            for (int i = 0; i < N; i++)
            {
                int iIndex = (int)Math.Truncate((dataX[i] - Xmin) / _x.h);
                int JIndex = (int)Math.Truncate((dataY[i] - Ymin) / _y.h);

                _var[iIndex][JIndex].n++;
                _var[iIndex][JIndex].p += 1d / N;

                s += String.Format("\n({0},{1})\tis in\t({2:0.0000}-{3:0.0000};\t{4:0.0000}-{5:0.0000})",
                                   dataX[i], dataY[i],
                                   _var[iIndex][JIndex].leftBorder, _var[iIndex][JIndex].rightBorder,
                                   _var[iIndex][JIndex].bottomBorder, _var[iIndex][JIndex].topBorder);

            }
        }

        //критерий хи квадрат Пирсона
        public void KrytteriyHi2()
        {
            TwoDimNormal distr = new TwoDimNormal(_x.Expectation, _y.Expectation, _x.Sigma, _y.Sigma, _koefLinearCorrelation);

            double hi2 = 0;

            #region Цикл подсчета hi2
            for (int i = 0; i < Mx; i++)
            {
                for (int j = 0; j < My; j++)
                {
                    Varianta currentVarianta = _var[i][j];
                    double hx = _x.h;
                    double hy = _y.h;
                    double x = currentVarianta.MiddleX;
                    double y = currentVarianta.MiddleY;

                    //p - со звездочкой
                    double p_zv = distr.f(x, y) * hx * hy;
                    //чтобы не делить на ноль
                    if (p_zv == 0)
                        continue;

                    hi2 += Math.Pow(currentVarianta.p - p_zv, 2) / p_zv;
                }
            }
            #endregion

            double kv = Kvantili.Hi2(this.alpha, _x.M * _y.M - 2);

            string s = "hi2 = " + hi2;
            s += "\nkv = " + kv;

            if (hi2 <= kv)
            {
                //Лолита сказала
                s += "\nhi2<kv\n";
                s += "Розподіл адекватний";
            }
            else
            {
                s += "\nhi2>kv\n";
                s += "Розподіл Не адекватний";
            }
            _messageService.ShowInformation(s);
        }

        //коэффциент корреляции
        public double GetKoefLinearCorrelation()
        {
            var x = _x.d;
            var y = _y.d;

            double xy = 0;
            for (int i = 0; i < N; i++)
                xy += x[i] * y[i];

            xy = xy / N;

            double a = N * (xy - _x.Expectation * _y.Expectation);
            double b = (N - 1) * _x.Sigma * _y.Sigma;

            _koefLinearCorrelation = a / b;

            //интервальная оценка
            double r = _koefLinearCorrelation;     //перенес для более лаконичного вида

            double c = r + (r * (1 - r * r)) / (2 * N);
            double d = Kvantili.Normal(alpha/2d) * (1 - r * r) / Math.Sqrt(N - 1);

            _koefLinearCorrelation_niz = c - d;
            _koefLinearCorrelation_verh = c + d;

            return _koefLinearCorrelation;
        }

        public double _koefCorrelationRatio;
        public double _koefCorrelationRatio_niz;
        public double _koefCorrelationRatio_verh;

        //переформатированиая выборка таким образом, чтобы одму x соответствовало несколько y
        //для кор отношения и регресии
        koefRatioService[] newSet;

        public double GetKoefCorrelationRatio()
        {

            //double y=_y.Me



            double ro = 0;

            newSet = new koefRatioService[_x.M];

            #region формирование новой выборки
            for (int i = 0; i < _x.M; i++)
            {
                newSet[i] = new koefRatioService();
                newSet[i].x = _var[i][0].MiddleX;

                List<int> indexes = new List<int>();

                double[] arr = _y.d;
                int xlen = _y.d.Length;
                double xh = _y.h;

                for (int j = 0; j < xlen; j++)
                {
                    if ((arr[j] >= _var[i][0].leftBorder) && (arr[j] < _var[i][0].rightBorder))
                        indexes.Add(j);
                    continue;
                }

                newSet[i].y = indexes.Select(indx => _y.d[indx]).ToArray();
            }
            #endregion

            double chisl = 0;
            double znam = 0;

            double yExpectation = _y.Expectation;

            for (int i = 0; i < newSet.Length; i++)
            {
                chisl += newSet[i].mi * Math.Pow(newSet[i].AverageY - yExpectation, 2);

                for (int j = 0; j < newSet[i].mi; j++)
                {
                    znam += Math.Pow(newSet[i].y[j] - yExpectation, 2);
                }
            }

            var s = newSet.Sum(x => x.mi);

            ro = chisl / znam;
            _koefCorrelationRatio = Math.Sqrt(ro);

            int k = newSet.Length;
        
            double V1 = (double)Math.Pow(k - 1 + N * ro, 2) / (k - 1 + 2 * N * ro);
            double commonPart = -(k - 1) / (double)N;
            _koefCorrelationRatio_niz = commonPart + (N - k) * ro / (N * (1 - ro) * Kvantili.Fishera(alpha, V1, N - k));
            _koefCorrelationRatio_verh = commonPart + (N - k) * ro / (N * (1 - ro) * Kvantili.Fishera(1 - alpha, V1, N - k));


            return _koefCorrelationRatio;
        }

        public double _koefSpirmena;
        public double _koefSpirmena_niz;
        public double _koefSpirmena_verh;

        public double _koefKendella;
        public double _koefKendella_niz;
        public double _koefKendella_verh;
        

        public void CalcKoefSpirmenaAndKendella()
        {
            double spirmen = 0;

            Tuple<double,double>[] sigma= new Tuple<double, double>[N];

            var xData = _x.d;
            var yData = _y.d;
            
            //связывание икса и игрека
            for (int i = 0; i < N; i++)
                sigma[i] = Tuple.Create(xData[i], yData[i]);
            
            sigma = sigma.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToArray();

            //получаем расширенные ранги с информацией о связках
            ExtendedRanks xExtRank = RankWork.CalcExtendedRanksForArray(sigma.Select(x => x.Item1).ToArray());
            ExtendedRanks yExtRank = RankWork.CalcExtendedRanksForArray(sigma.Select(x => x.Item2).ToArray());
            
            //ранжированые x и y
            RankElement[] xRanedArray = xExtRank.rankedArray;
            RankElement[] yRanedArray = yExtRank.rankedArray;

            //сопоставление рангов
            var rx = xRanedArray.Select(x => x.rank).ToArray();
            var ry = new double[N];
            for (int i = 0; i < N; i++)
            {
                double y = sigma[i].Item2;
                RankElement a = yRanedArray.First(x => x.data == y);
                ry[i] = a.rank;
            }
            //перенос нужной информации в простые локальные переменные
            //связки
            var xbundles = xExtRank.BundlesInfo;
            var ybundles = yExtRank.BundlesInfo;
            //их длины
            var xBundlesN = xbundles.Count;
            var yBundlesN = ybundles.Count;


            #region Коеф. Спирмена
            //сумма квадрата разници рангов (d по книге)
            double SumD = 0;
            for (int i = 0; i < N; i++)
                SumD += Math.Pow(rx[i] - ry[i], 2);


            if ((xBundlesN == 0) && (yBundlesN == 0))
            {
                spirmen = 1 - 6d / (N * (N * N - 1)) * SumD;
            }
            else
            {
                //длЯ подсчета A и B
                Func<List<int>, double> SumBundles = bundlesLst =>
                {
                    double value = 0;
                    for (int i = 0; i < bundlesLst.Count; i++)
                    {
                        int a = bundlesLst[i];
                        value += Math.Pow(a, 3) - a;
                    }
                    return value / 12d;
                };

                double A = SumBundles(xbundles);
                double B = SumBundles(ybundles);

                double common = 1d / 6 * N * (N * N - 1);
                //числитель
                double num = common - SumD - A - B;
                //знаменатель
                double den = Math.Sqrt((common - 2 * A) * (common - 2 * B));
                spirmen = num / den;
            }
            _koefSpirmena = spirmen;

            //double tFOrSpirmen = (spirmen * Math.Sqrt(N - 2)) / (Math.Sqrt(1 - spirmen * spirmen));
            double sigmaSpirmen = Math.Sqrt((1 - spirmen * spirmen)/(N-2));

            _koefSpirmena_niz = spirmen - Kvantili.Student(alpha/2, N - 2) * sigmaSpirmen;
            _koefSpirmena_verh = spirmen + Kvantili.Student(alpha/2, N - 2) * sigmaSpirmen;

            #endregion

            #region Коеф Кендалла
            
            {
                double S = 0;

                for (int i = 0; i < N-1; i++)
                    for (int j = i+1; j < N; j++)
                        if (ry[i] < ry[j])
                            S++;
                        else if (ry[i] > ry[j])
                            S--;



                double kendal = 2 * S / (N * (N - 1));

                _koefKendella = kendal;

                double sigmaKendal = Math.Sqrt((4.0*N+10) / (9*(N*N - N)));

                _koefKendella_niz = kendal - Kvantili.Normal(alpha/2) * sigmaKendal;
                _koefKendella_verh = kendal + Kvantili.Normal(alpha/2) * sigmaKendal;

            }

            #endregion
        }


        //ПРОВЕРКИ НА ЗНАЧИМОСТЬ
        //линейный коеф. корреляции
        public void CheckForSignificance_LinearKoef()
        {
            string s ="";

            double r = _koefLinearCorrelation;
            double t = r * Math.Sqrt(N - 2)/Math.Sqrt(1 - r * r);
            double kv;

            if (N < 60)
                kv = Kvantili.Student(alpha, N - 2);
            else
                kv = Kvantili.Normal(alpha/2);
       

            s += String.Format("r = {0}\n", r);
            s += String.Format("t = {0}\n", t);
            s += String.Format("kv = {0}\n", kv);
            
            if (Math.Abs(t)<=kv)
            {
                s += String.Format("|t|<=kv\n");
                s += String.Format("Коефіцієнт лінійної кореляції НЕ значущий");
            }
            else 
            {
                s += String.Format("|t|>kv\n");
                s += String.Format("Коефіцієнт лінійної кореляції значущий");
            }
            _messageService.ShowInformation(s);
        }

        //корреляционное отношение
        public void CheckForSignificance_KorrelationRatio()
        {
            string s = "";

            int k = _x.M;
            double ro = _koefCorrelationRatio;
            
            double f = ro * ro / (1 - ro * ro) * (N - k) / (k - 1);
            double kv = Kvantili.Fishera(alpha, k - 1, N - k);

            s += String.Format("k = {0}\n", k);
            s += String.Format("ro = {0}\n", ro);
            s += String.Format("f = {0}\n", f);
            s += String.Format("kv = {0}\n", kv);

            if (f > kv)
            {
                s += String.Format("f>kv\n");
                s += "Кореляційне відношення значуще";
                //значущий
            }
            else 
            {
                s += String.Format("f>kv\n");
                s += "Кореляційне відношення НЕ значуще";
            }
            _messageService.ShowInformation(s);
        }

        //коеф Спирмена
        public void CheckForSignificance_KoefSparmena()
        {
            string s = "";

            double tau = _koefLinearCorrelation;
            double t = tau * Math.Sqrt(N - 2) / Math.Sqrt(1 - tau * tau);
            double kv = Kvantili.Student(alpha, N - 2);

            s += String.Format("tau = {0}\n", tau);
            s += String.Format("t = {0}\n", t);
            s += String.Format("kv = {0}\n", kv);

            if (Math.Abs(t) <= kv)
            {
                s += String.Format("|t|<=kv\n");
                s += String.Format("Коефіцієнт Спірмена НЕ значущий");
            }
            else
            {
                s += String.Format("|t|>=kv\n");
                s += String.Format("Коефіцієнт Спірмена значущий");
            }
            _messageService.ShowInformation(s);
        }


        //!!!КОЭФФИЦИЕНТЫ СООБЩЕНИЙ ТАБЛИЦ
        FactorsConnectionsTables _tConnect2on2;

        public FactorsConnectionsTables TableConnection2on2
        {
            get 
            {
                _tConnect2on2 = new FactorsConnectionsTables(_x, _y, this.alpha);

                return _tConnect2on2;
            }
        }

        FactorsConnectionsTablesNM _tConnectNonM;

        public FactorsConnectionsTablesNM TableConnectionNonM
        {
            get
            {
                _tConnectNonM = new FactorsConnectionsTablesNM(_x, _y,5,7, this.alpha);

                return _tConnectNonM;
            }
        }

        //!!РЕГРЕССИЯ
        //ЛИНЕЙНАЯ
        double m_a = 0;         //параметры
        double m_b = 0;
        double m_S2 = 0;        //остаточноя дисперсия
        double m_sigmaS2 = 0;   //корень из остаточной дисперсии
        double m_Sa = 0;        //сигма для а
        double m_Sb = 0;        //сигма для b


        public string GetKriteriaBartlet()
        {
            //дляинна новой выборки
            int k = newSet.Length;

            //значения для формулы
            double C, Spow2 = 0;
            Func<koefRatioService, double> S_index;

            #region Находим С
            double smallSumforC = 0;

            //подсчетмаленькой суммы для С
            for (int i = 0; i < newSet.Length; i++)
                smallSumforC += 1d / newSet[i].mi;

            C = 1 + 1d / (3 * (k - 1)) * (smallSumforC + 1 / N);

            #endregion

            #region Находим S_index (по книге S с индексом) - делегат

            S_index = (x) =>
            {

                double sum = 0;

                int mi = x.mi;
                double yAVG = x.AverageY;

                for (int j = 0; j < mi; j++)
                    sum += Math.Pow(x.y[j] - yAVG, 2);

                double result = 1d / (mi - 1) * sum;

                return result;

            };

            #endregion

            #region Ищем S

            {
                double sumForS = 0;

                for (int i = 0; i < k; i++)
                    sumForS += (newSet[i].mi - 1) * S_index(newSet[i]);

                Spow2 = 1d/(N-1)*sumForS;
            }
            #endregion

            //находим саму статистику L
            
            double SumForL = 0;
            for (int i = 0; i < k; i++)
            {
                //Current newSetElement
                var CurEl =newSet[i];

                SumForL += CurEl.mi * Math.Log(S_index(CurEl) / Spow2);
            }
            //подсчитаная Л
            double L = -1d / C * SumForL;

            double kv = Kvantili.Hi2(1 - alpha, k - 1);


            string message = "L = " + Math.Round(L, 4)+Environment.NewLine
                            + "kv = " + Math.Round(kv, 4) + Environment.NewLine;
            
            if (L>kv)
                message += "L>kv" + Environment.NewLine
                            + "Дисперсія залежної змінної у НЕ залишається сталою при зміні аргументу х";
            
            else
                message += "L<kv" + Environment.NewLine
                            + "Дисперсія залежної змінної у залишається сталою при зміні аргументу х";

            return message;
            //throw new NotImplementedException();
        }

        //находим параметры для линейной регрессии по МНК
        public Tuple<double, double> GetABForLinearRegressionOLS()
        {
            double a, b = 0;

            b = _koefLinearCorrelation * _y.Sigma / _x.Sigma;
            a = _y.Expectation - b * _x.Expectation;

            //перенос значений в поля
            m_a = a;
            m_b = b;


            //подсчет остаточной дисперсии
            CalcResidualDispersion();

            return Tuple.Create<double, double>(a, b);
        }

        //находим параметры для линейной регрессии по Методу Теила
        public Tuple<double, double> GetABForLinearRegressionTeil()
        {
            var x = _x.d;
            var y = _y.d;

            List<double> lstForB = new List<double>();
            
            for (int i = 0; i < x.Length-1; i++)
            {
                for (int j = i+1; j < x.Length; j++)
                {
                    double valueToList = (y[j]-y[i])/(x[j]-x[i]);
                    
                    lstForB.Add(valueToList);
                }
            }

            double b = STAT.ArrayMediana(lstForB.OrderBy(val=>val).ToArray());

            List<double> ListForA = new List<double>();

            for (int i = 0; i < x.Length; i++)
                ListForA.Add(y[i] - b * x[i]);

            double a = STAT.ArrayMediana(ListForA.OrderBy(val => val).ToArray());

            //перенос значений в поля
            m_a = a;
            m_b = b;


            //подсчет остаточной дисперсии
            CalcResidualDispersion();


            return Tuple.Create<double, double>(a, b);
        }

        //дослідження якості відтворення регресії
        
        //коэфициент детерминации 
        public double GetCoefDetermination()
        {
            return _koefLinearCorrelation * _koefLinearCorrelation * 100;
        }

        //подсчет остаточной дисперсии. Используется выше
        private void CalcResidualDispersion()
        {
            var x = _x.d;
            var y = _y.d;
            var a = m_a;
            var b = m_b;
            var N = x.Length;

            double Sum = 0;
            for (int i = 0; i < N; i++)
                Sum += Math.Pow(y[i] - a - b * x[i], 2);

            m_S2 = Sum / (N - 2);

            //возьмем корень и положим в поле класса
            m_sigmaS2 = Math.Sqrt(m_S2);

            //m_Sb = m_S2 / (_x.Sigma * Math.Sqrt(N - 1));

        }

        public Tuple<double, double, double, double> GetIntervalsToAB()
        {
            double a = m_a;
            double b = m_b;
            int N = _x.d.Length;
            double sigma_S = m_sigmaS2;

            m_Sa = sigma_S * Math.Sqrt(1d / N + Math.Pow(_x.Expectation, 2) / (_x.Dispersia * (N - 1)));

            m_Sb = sigma_S / (_x.Sigma * Math.Sqrt(N - 1));

            double nu = N - 2;
            double kv = Kvantili.Student(alpha / 2, nu);

            double a_niz, a_verh, b_niz, b_verh = 0;

            a_niz = a - kv *  m_Sa;
            a_verh = a + kv * m_Sa;
            b_niz = b - kv *  m_Sb;
            b_verh = b + kv * m_Sb;

            return Tuple.Create<double, double, double, double>(a_niz, a_verh, b_niz, b_verh);
        }

        //дослідження точності оцінок параметрів лінійної регресії
        public string CheckSugnificanceOfAB()
        {
            double ta = m_a / m_Sa;
            double tb = m_b / m_Sa;
            double kv = Kvantili.Student(alpha / 2, N-2);

            string message = "";

            message += "ta = " + Math.Round(ta,4)+Environment.NewLine
                    +"tb = " + Math.Round(tb, 4)+Environment.NewLine
                    +"kv = " + Math.Round(kv, 4) + Environment.NewLine;

            if (Math.Abs(ta) <= kv)
                message += "|ta|<=kv" + Environment.NewLine
                         + "ta не значуща!" + Environment.NewLine;
            else
            
                message += "|ta|>kv" + Environment.NewLine
                         + "ta значуща!"+ Environment.NewLine;

            if (Math.Abs(tb) <= kv)
                message += "|tb|<=kv" + Environment.NewLine
                         + "tb не значуща!" + Environment.NewLine;
            else

                message += "|tb|>kv" + Environment.NewLine
                         + "tb значуща!";


            return message;
        }
        
        //(перевірка рівності параметрів деяким числам)
        public string CheckSugnificanceOfParametrsLinearRegression(double a, double b)
        {
            double ta = (m_a-a) / m_Sa;
            double tb = (m_b-b) / m_Sa;
            double kv = Kvantili.Student(alpha / 2, N - 2);

            string message = "";

            message += "ta = " + Math.Round(ta, 4) + Environment.NewLine
                    + "tb = " + Math.Round(tb, 4) + Environment.NewLine
                    + "kv = " + Math.Round(kv, 4) + Environment.NewLine;

            if (Math.Abs(ta) <= kv)
                message += "|ta|<=kv" + Environment.NewLine
                         + "ta статистично дорівнює " + a + Environment.NewLine;
            else

                message += "|ta|>kv" + Environment.NewLine
                         + "ta статистично НЕ дорівнює " + a+ Environment.NewLine;

            if (Math.Abs(tb) <= kv)
                message += "|tb|<=kv" + Environment.NewLine
                         + "tb статистично дорівнює " + b;
            else

                message += "|tb|>kv" + Environment.NewLine
                         + "tb статистично НЕ дорівнює " + b;

            return message;
        }

        //торерантні межі
        //возвращает кортеж: (x[] одинаковые для нижнего и верхнего пределов, y[] для нижней границы,y[] для верхней границы)
        public Tuple<double[], double[], double[]> GetToleranceLimits()
        {

            double a = m_a;
            double b = m_b;
            int N = _x.d.Length;

            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();


            //из скольки точек рисовать интервалы (сами линии)
            int points = 1000;

            double h = (_x.Max - _x.Min) / points;

            //линия регресси
            //Func<double, double> y = (val) => a+b*val;

            //для +-
            double kv = Kvantili.Student(alpha / 2, N - 2);
            double term = kv*m_sigmaS2;

            for (double i = _x.Min; i <= _x.Max; i=i+h)
            {
                x.Add(i);
                y_niz.Add(a+b*i-term);
                y_verh.Add(a+b*i+term);
            }

            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());
        }

        //доверительные интервалы для новых наблюдений
        //(общие иксы, низ, верх)
        public Tuple<double[], double[],double[]> GetConfidenceIntervalsForNewObservations()
        {
            double a = m_a;
            double b = m_b;
            double xExp = _x.Expectation;
            int N = _x.d.Length;
            double S2 = m_S2;
            double Sb2 = m_Sb*m_Sb;

            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();


            //из скольки точек рисовать интервалы (сами линии)
            int points = 1000;

            double h = (_x.Max - _x.Min) / points;

            //линия регресси
            Func<double, double> y = (val) => a + b * val;

            //для +-
            double kv = Kvantili.Student(alpha / 2, N - 2);
            double leftPart = S2*(1+1d/N);

            Func<double, double> sigma = x0 => Math.Sqrt(leftPart + Sb2*Math.Pow((x0-xExp),2));

            for (double i = _x.Min; i <= _x.Max; i = i + h)
            {
                x.Add(i);
                y_niz.Add(a + b * i  - kv * sigma(i));
                y_verh.Add(a + b * i + kv * sigma(i));
            }

            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());
        
        }

        //доверительные интервалы для линии регрессии
        //(общие иксы, низ, верх)
        public Tuple<double[], double[], double[]> GetConfidenceIntervalsForRegressionLine()
        {
            double a = m_a;
            double b = m_b;
            double xExp = _x.Expectation;
            int N = _x.d.Length;
            double S2 = m_S2;
            
            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();


            //из скольки точек рисовать интервалы (сами линии)
            int points = 1000;

            double h = (_x.Max - _x.Min) / points;

            //линия регресси
            Func<double, double> y = (val) => a + b * val;

            //для +-
            double kv = Kvantili.Student(alpha / 2, N - 2);
            double leftPart = S2 / N;
            double Sb2 = m_Sb * m_Sb;
    
            

            Func<double, double> sigma = xval => Math.Sqrt(leftPart + Sb2 * Math.Pow(xval - xExp,2));

            for (double i = _x.Min; i <= _x.Max; i = i + h)
            {
                x.Add(i);
                y_niz.Add(y(i) - kv * sigma(i));
                y_verh.Add(y(i) + kv * sigma(i));
            }

            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());

        }

        //к линейной - ПРоверка адекватности воссозданной регрессии
        public string ValidationOfAdequacyRegression()
        {
            double f = m_S2/_y.Dispersia;
            double kv = Kvantili.Fishera(alpha, N - 1, N - 3);

            string message = "f = " + Math.Round(f, 4) + Environment.NewLine +
                             "kv =" + Math.Round(kv, 4) + Environment.NewLine;

            if (f<=kv)
                message += "f<=kv" + Environment.NewLine+
                            "Відтворена залежність адекватна";

            else
                message += "f<=kv" + Environment.NewLine +
                            "Відтворена залежність НЕ адекватна";


            return message;
        }
        //конец линейной Регрессии

        //ПАРАБОЛИЧЕСКАЯ РЕГРЕССИЯ
        //создам поля параметров, чтобы не попутать с линейной регрессией
        //префикс - mP- - member Parabolic
        double mP_a;    //параметры
        double mP_b;
        double mP_c;
        double mP_Sa;   //сигмы параметров
        double mP_Sb;
        double mP_Sc;
        double mP_Sigma;    //остаточная сигма


        //кортеж параметров (a - b - c)
        public Tuple<double, double, double> CalcParabolicRegresParametrsABC()
        {
            double a, b, c = 0;
            double xAVG = _x.Expectation;

            double[] x = _x.d;    //перенес, чтобы постоянно не искать ссылку
            double[] y = _y.d;

            #region Поиск параметров
            
            a = _y.Expectation;

            { //работа с параметром б
                double nom = 0;     //числитель
                double den = 0;     //знаменатель

                for (int i = 0; i < N; i++)
                {
                    double common = (x[i] - xAVG); //общее для числителя и знаменятеля

                    nom += common * y[i];
                    den += common * common;
                }
                b = nom / den;
            }

            //{ //работа с параметром с

                double nom1 = 0; //числитель
                double den1 = 0; //знаменатель

                for (int i = 0; i < N; i++)
                {
                    double fi2Value = fi2(x[i]);

                    nom1 += fi2Value * y[i];
                    den1 += fi2Value * fi2Value;
                    ;
                }

                c = nom1 / den1;
            //}
            #endregion 

            //перенос значений параметров в поля
            mP_a = a;
            mP_b = b;
            mP_c = c;

            //подсчет остаточной дисперсии
            GetParabolicResidualDispersion();

            return Tuple.Create<double, double, double>(a, b, c);
        }

        //вспомагательные фи для квадратического распределения
        double fi1(double x) { return x - _x.Expectation;}

        double fi2(double x)
        {
            double InM2 = _x.InitialMoment(2);
            double InM3 = _x.InitialMoment(3);

            double xAVG = _x.Expectation;
            double xDisp = _x.Dispersia;


            return x*x - (InM3-InM2*xAVG)/xDisp*(x-xAVG)-InM2; 
        }

        //параболическая регрессия - линия
        public Tuple<double[], double[]> GetParabolicRegressionLine()
        {
            double min = _x.Min;
            double max = _x.Max;

            double a = mP_a, b = mP_b, c = mP_c;

            Func<double, double> RegressionFunc = x => a + b * fi1(x) + c*fi2(x);

            int pointsQuantity = 1000;      //количество точек для отрисовки 
            double step = (_x.Max - _x.Min) / pointsQuantity;
 
            List<double> X = new List<double>(), Y = new List<double>();
            
            for (double  i = _x.Min; i <= _x.Max; i=i+step)
            {
                X.Add(i);
                Y.Add(RegressionFunc(i));
            }

            return Tuple.Create<double[], double[]>(X.ToArray(), Y.ToArray());
        }

        //подсчет отстаточной дисперсии
        private void GetParabolicResidualDispersion()
        {
            double sum = 0;

            var x = _x.d;
            var y = _y.d;

             double a = mP_a;
             double b = mP_b;
             double c = mP_c;

            for (int i = 0; i < N; i++)
                sum +=Math.Pow(y[i]- a - b*fi1(x[i]) - c*fi2(x[i]),2);

            double S2 = sum / (N - 3);

            mP_Sigma = Math.Sqrt(S2);
        }

        //интервальное оценивание параметров
        public Tuple<double, double, double, double, double, double> GetIntervalsForABC()
        {
            mP_Sa = mP_Sigma / Math.Sqrt(N);
            mP_Sb = mP_Sigma / (_x.Sigma * Math.Sqrt(N));

            {//для параметра с
                double sum = 0;
                var x = _x.d;

                for (int i = 0; i < N; i++)
                    sum += Math.Pow(fi2(x[i]), 2);

                mP_Sc = mP_Sigma / sum;
            }

            double kv = Kvantili.Student(alpha/2,N-3);

            double a_niz =  mP_a - kv * mP_Sa;
            double a_verh = mP_a + kv * mP_Sa;

            double b_niz  = mP_b - kv *  mP_Sb;
            double b_verh = mP_b + kv * mP_Sb;

            double c_niz  = mP_c - kv *  mP_Sc;
            double c_verh = mP_c + kv * mP_Sc;

            return Tuple.Create<double, double, double, double, double, double>(a_niz, a_verh, b_niz, b_verh, c_niz, c_verh);
        }

        public double GetParabolicDetermianation()
        {
            return _koefCorrelationRatio * _koefCorrelationRatio * 100;
        }

        //проверка значимости ABC
        public string ChekSignificanseABC()
        {
            double ta, tb, tc = 0;
            double[] x = _x.d;

            //поиск значений для сравнения с квантилем
            ta = mP_a / mP_Sigma * Math.Sqrt(N);

            { // для б
                double sum = 0;

                for (int i = 0; i < N; i++)
                    sum += Math.Pow(fi1(x[i]),2);

                tb = mP_b / mP_Sigma * Math.Sqrt(sum); 
            }

            { // для c - практически аналогично b
                double sum = 0;

                for (int i = 0; i < N; i++)
                    sum += Math.Pow(fi2(x[i]), 2);

                tc = mP_c / mP_Sigma * Math.Sqrt(sum);
            }

            double kv = Kvantili.Student(alpha / 2, N - 3);

            //это можно копировать в другие методы.
            string message = "";
            Action<string, double> AddParam2Mess = (PName, Pvalue) => message += PName + " = " + Math.Round(Pvalue, 4) + Environment.NewLine;
            Action<string> AddStr2Mess = str => message += str + Environment.NewLine;
            Action<string, string> Add2Str2Mess = (str1, str2) => { AddStr2Mess(str1); AddStr2Mess(str2); };


            AddParam2Mess("kv", kv);
            AddParam2Mess("ta", ta);
            AddParam2Mess("tb", tb);
            AddParam2Mess("tc", tc);

            if (Math.Abs(ta) <= kv)
                Add2Str2Mess("|ta|<=kv","Параметр а НЕ Значущий");
            else
                Add2Str2Mess("|ta|>kv", "Параметр а Значущий");

            if (Math.Abs(tb) <= kv)
                Add2Str2Mess("|tb|<=kv", "Параметр b НЕ Значущий");
            else
                Add2Str2Mess("|tb|>kv", "Параметр b Значущий");

            if (Math.Abs(tc) <= kv)
                Add2Str2Mess("|tc|<=kv", "Параметр c НЕ Значущий");
            else
                Add2Str2Mess("|tc|>kv", "Параметр c Значущий");


            return message;
        }

        public string CheckEqualityParabolicParametrA(double a)
        {
            //это можно копировать в другие методы.
            string message = "";
            Action<string, double> AddParam2Mess = (PName, Pvalue) => message += PName + " = " + Math.Round(Pvalue, 4) + Environment.NewLine;
            Action<string> AddStr2Mess = str => message += str + Environment.NewLine;
            Action<string, string> Add2Str2Mess = (str1, str2) => { AddStr2Mess(str1); AddStr2Mess(str2); };

            double ta = (mP_a - a) / mP_Sigma * Math.Sqrt(N);
            double kv = Kvantili.Student(alpha / 2, N - 3);


            AddParam2Mess("kv", kv);
            AddParam2Mess("ta", ta);

            if (Math.Abs(ta) <= kv)
                Add2Str2Mess("|ta|<=kv", "Параметр а статистично дорівнює "+a);
            else
                Add2Str2Mess("|ta|>kv", "Параметр а статистично НЕ дорівнює " + a);

            return message;
        }
        
        public string CheckEqualityParabolicParametrB(double b)
        {
            //это можно копировать в другие методы.
            string message = "";
            Action<string, double> AddParam2Mess = (PName, Pvalue) => message += PName + " = " + Math.Round(Pvalue, 4) + Environment.NewLine;
            Action<string> AddStr2Mess = str => message += str + Environment.NewLine;
            Action<string, string> Add2Str2Mess = (str1, str2) => { AddStr2Mess(str1); AddStr2Mess(str2); };

            double tb = 0;
            double kv = Kvantili.Student(alpha / 2, N - 3);

            double[] x = _x.d;
            { // для б
                double sum = 0;

                for (int i = 0; i < N; i++)
                    sum += Math.Pow(fi1(x[i]), 2);

                tb = (mP_b-b) / mP_Sigma * Math.Sqrt(sum);
            }

            AddParam2Mess("kv", kv);
            AddParam2Mess("tb", tb);

            if (Math.Abs(tb) <= kv)
                Add2Str2Mess("|tb|<=kv", "Параметр b статистично дорівнює " + b);
            else
                Add2Str2Mess("|tb|>kv", "Параметр b статистично НЕ дорівнює " + b);

            return message;
        }

        public string CheckEqualityParabolicParametrC(double c)
        {
            //это можно копировать в другие методы.
            string message = "";
            Action<string, double> AddParam2Mess = (PName, Pvalue) => message += PName + " = " + Math.Round(Pvalue, 4) + Environment.NewLine;
            Action<string> AddStr2Mess = str => message += str + Environment.NewLine;
            Action<string, string> Add2Str2Mess = (str1, str2) => { AddStr2Mess(str1); AddStr2Mess(str2); };

            double tc = 0;
            double kv = Kvantili.Student(alpha / 2, N - 3);

            double[] x = _x.d;

            { // для c - практически аналогично b
                double sum = 0;

                for (int i = 0; i < N; i++)
                    sum += Math.Pow(fi2(x[i]), 2);

                tc = (mP_c-c) / mP_Sigma * Math.Sqrt(sum);
            }

            AddParam2Mess("kv", kv);
            AddParam2Mess("tc", tc);

            if (Math.Abs(tc) <= kv)
                Add2Str2Mess("|tc|<=kv", "Параметр c статистично дорівнює " + c);
            else
                Add2Str2Mess("|tc|>kv", "Параметр c статистично НЕ дорівнює " + c);

            return message;
        }

        public Tuple<double[], double[], double[]> GetTolerantParablocLimits()
        {
            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();

            var xMin = _x.Min;
            var xMax = _x.Max;

            int pointsQuantity = 1000;  //количество точек
            double h = (xMax - xMin) / pointsQuantity;

            double kv = Kvantili.Student(alpha / 2, N - 3);
            double term = kv * mP_Sigma;

            double a = mP_a, b = mP_b, c = mP_c;
            Func<double, double> RegressionFunc = xvar => a + b * fi1(xvar) + c * fi2(xvar);


            for (double i = xMin; i <= xMax; i=i+h)
            {
                x.Add(i);
                y_niz.Add(RegressionFunc(i) - term);
                y_verh.Add(RegressionFunc(i) + term);
            }


            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());
        }

        //доверительные интервалы параболической регрессии
        public Tuple<double[], double[], double[]> GetParabolicConfidenceIntervals()
        {
            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();

            var xMin = _x.Min;
            var xMax = _x.Max;

            int pointsQuantity = 1000;  //количество точек
            double h = (xMax - xMin) / pointsQuantity;

            double kv = Kvantili.Student(alpha / 2, N - 3);
           
            double a = mP_a, b = mP_b, c = mP_c;
            double S2 = mP_Sigma * mP_Sigma;
            double S2b = mP_Sb * mP_Sb;
            double S2c = mP_Sc * mP_Sc;
 
            Func<double, double> RegressionFunc = xvar => a + b * fi1(xvar) + c * fi2(xvar);
            Func<double, double> S = xvar => Math.Sqrt(1d/N*S2 + S2b*Math.Pow(fi1(xvar),2)+S2c*Math.Pow(fi2(xvar),2));


            for (double i = xMin; i <= xMax; i = i + h)
            {
                double term = kv * S(i);

                x.Add(i);
                y_niz.Add(RegressionFunc(i) - term);
                y_verh.Add(RegressionFunc(i) + term);
            }


            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());
        }
        
        //доверительные интервалы для новых наблюдений в парабоилческой регрессии (от доврительных интервалов самой линии регрессии отличается на +1 при дисперсии погрешности)
        public Tuple<double[], double[], double[]> GetParabolicIntervalsForNewObservations()
        {
            List<double> x = new List<double>();
            List<double> y_niz = new List<double>();
            List<double> y_verh = new List<double>();

            var xMin = _x.Min;
            var xMax = _x.Max;

            int pointsQuantity = 1000;  //количество точек
            double h = (xMax - xMin) / pointsQuantity;

            double kv = Kvantili.Student(alpha / 2, N - 3);

            double a = mP_a, b = mP_b, c = mP_c;
            double S2 = mP_Sigma * mP_Sigma;
            double S2b = mP_Sb * mP_Sb;
            double S2c = mP_Sc * mP_Sc;

            Func<double, double> RegressionFunc = xvar => a + b * fi1(xvar) + c * fi2(xvar);
            Func<double, double> S = xvar => Math.Sqrt((1d / N+1) * S2 + S2b * Math.Pow(fi1(xvar), 2) + S2c * Math.Pow(fi2(xvar), 2));


            for (double i = xMin; i <= xMax; i = i + h)
            {
                double term = kv * S(i);

                x.Add(i);
                y_niz.Add(RegressionFunc(i) - term);
                y_verh.Add(RegressionFunc(i) + term);
            }

            return Tuple.Create<double[], double[], double[]>(x.ToArray(), y_niz.ToArray(), y_verh.ToArray());
        }

        //к линейной - ПРоверка адекватности воссозданной регрессии
        public string ParabolicValidationOfAdequacyRegression()
        {
            double f = Math.Pow(mP_Sigma,2)/ _y.Dispersia;
            double kv = Kvantili.Fishera(alpha, N - 1, N - 3);

            string message = "f = " + Math.Round(f, 4) + Environment.NewLine +
                             "kv =" + Math.Round(kv, 4) + Environment.NewLine;

            if (f <= kv)
                message += "f<=kv" + Environment.NewLine +
                            "Відтворена залежність адекватна";

            else
                message += "f<=kv" + Environment.NewLine +
                            "Відтворена залежність НЕ адекватна";


            return message;
        }

        //КВАЗИЛИНЕЙНАЯ РЕГРЕССИЯ
        STAT _kvazX = new STAT();   //для  x пересчитаного под квазилинейную регрессию
        STAT2D TwoDimForKvaz;
        double kvaz_a;
        double kvaz_b;

        //тут должна быть ф-я получения квазилинейной регрессии
        public Tuple<double, double> GetKvazRegressionParams()
        {
            //переформатирование икса
            FillKvazX();

            STAT a = new STAT();
            a.Setd2Stat2d((double[])_kvazX.d.Clone());
 
            STAT b = new STAT();
            b.Setd2Stat2d((double[])_y.d.Clone());
 
            TwoDimForKvaz = new STAT2D(a,b);

            //только тут доходим до параметров AB большие
            var ABTuple = TwoDimForKvaz.GetABForLinearRegressionOLS();
            kvaz_a = ABTuple.Item1;
            kvaz_b = ABTuple.Item2;

            //дальше считаем сигмы
            var t = TwoDimForKvaz._x.d;
            var z = TwoDimForKvaz._y.d;



            return ABTuple;
        }


        //выполнить сдвиг и прологарифмировать
        private void FillKvazX()
        {
            var x = _x.d;
            var xMin = x.Min();

            if (xMin <= 0)
            {
                var Xzsun = x.Select(v => v + Math.Abs(xMin) + 0.2).ToArray();

                //тут функция преобразования согласно варианта
                var XForKvaz = Xzsun.Select(v => v = Math.Log(v)).ToArray();

                _kvazX.Setd2Stat2d(XForKvaz);
            }
            else
            {
                var arr = x.Select(v => Math.Log(v)).ToArray();
                _kvazX.Setd2Stat2d(arr);
            }
        } 

        public Tuple<double,double, double, double> GetIntervalsForABKvaz()
        {
            return TwoDimForKvaz.GetIntervalsToAB();
        }

        //выполнение сдвига 
        public void ChangeCorrelationField(Series s)
        {
            var x = _x.d;
            var XMin = x.Min();
            if (XMin <= 0)
            {
                x = x.Select(v => v + Math.Abs(XMin) + 0.2).ToArray();

                _x.Setd2Stat2d(x);

                GetKorrelationField(s);
            }
        }

        //получение линии квазилинейной регрессии
        public Tuple<double[], double[]> GetKvazRegressionLine()
        {
            var points = 1000.0;
            var xMin = _x.d.Min();
            var xMax = _x.d.Max();
            var h = (xMax - xMin) / points;

            List<double> Xlst = new List<double>();
            List<double> Ylst = new List<double>();
            
            for (double x = xMin; x <= xMax; x=x+h)
            {
                Xlst.Add(x);

                var y = kvaz_a+kvaz_b*Math.Log(x);
                Ylst.Add(y);
            }

            return Tuple.Create<double[], double[]>(Xlst.ToArray(), Ylst.ToArray());
        }

        //доверительные интервалы для квазилинейной линии регрессии
        //(общие иксы, низ, верх)
        public Tuple<double[], double[], double[]> GetConfidenceIntervalsForRegressionLineKVAZ()
        {
            var points = 1000.0;
            var xMin = _x.d.Min();
            var xMax = _x.d.Max();
            var h = (xMax - xMin) / points;

            List<double> Xlst = new List<double>();
            List<double> YlstNiz = new List<double>();
            List<double> YlstVerh = new List<double>();


            //квазилинейность
            double S2 = TwoDimForKvaz.m_S2;
            double Sa = TwoDimForKvaz.m_Sa;
            double Sb = TwoDimForKvaz.m_Sb;

            double xAvg = _kvazX.Expectation; 
            Func<double, double> sigma = v=>Math.Sqrt(S2/N+ Math.Pow(Sb*(v-xAvg),2));
            for (double x = xMin; x <= xMax; x = x + h)
            {
                Xlst.Add(x);

                var y = kvaz_a+kvaz_b*Math.Log(x);
     
                var ElementaryPart = Kvantili.Student(alpha/2d, N-2)*sigma(Math.Log(x));

                YlstNiz.Add(y - ElementaryPart);
                YlstVerh.Add(y + ElementaryPart);
            }
            return Tuple.Create<double[], double[], double[]>(Xlst.ToArray(), YlstNiz.ToArray(), YlstVerh.ToArray());
        }

        //доверительные интервалы для новых наблюдений
        public Tuple<double[], double[], double[]> GetConfidenceIntervalsForNewObservationsKVAZ()
        {
            var points = 1000.0;
            var xMin = _x.d.Min();
            var xMax = _x.d.Max();
            var h = (xMax - xMin) / points;

            List<double> Xlst = new List<double>();
            List<double> YlstNiz = new List<double>();
            List<double> YlstVerh = new List<double>();


            //квазилинейность
            double S2 = TwoDimForKvaz.m_S2;
            double Sa = TwoDimForKvaz.m_Sa;
            double Sb = TwoDimForKvaz.m_Sb;

            double xAvg = _kvazX.Expectation;
            Func<double, double> sigma = v => Math.Sqrt(S2*(1+ 1d/ N) + Math.Pow(Sb * (v - xAvg), 2));
            for (double x = xMin; x <= xMax; x = x + h)
            {
                Xlst.Add(x);

                var y = kvaz_a + kvaz_b * Math.Log(x);

                var ElementaryPart = Kvantili.Student(alpha / 2d, N - 2) * sigma(Math.Log(x));

                YlstNiz.Add(y - ElementaryPart);
                YlstVerh.Add(y + ElementaryPart);
            }
            return Tuple.Create<double[], double[], double[]>(Xlst.ToArray(), YlstNiz.ToArray(), YlstVerh.ToArray());
        }

        public Tuple<double[], double[], double[]> GetToleranceLimitsKVAZ()
        {
            var points = 1000.0;
            var xMin = _x.d.Min();
            var xMax = _x.d.Max();
            var h = (xMax - xMin) / points;

            List<double> Xlst = new List<double>();
            List<double> YlstNiz = new List<double>();
            List<double> YlstVerh = new List<double>();


            //квазилинейность
            double S2 = TwoDimForKvaz.m_S2;
            double Sa = TwoDimForKvaz.m_Sa;
            double Sb = TwoDimForKvaz.m_Sb;
            var ElementaryPart =Kvantili.Student(alpha/2, N-2)*TwoDimForKvaz.m_sigmaS2;

            for (double x = xMin; x <= xMax; x = x + h)
            {
                Xlst.Add(x);

                var y = kvaz_a + kvaz_b * Math.Log(x);

                YlstNiz.Add(y - ElementaryPart);
                YlstVerh.Add(y + ElementaryPart);
            }
            return Tuple.Create<double[], double[], double[]>(Xlst.ToArray(), YlstNiz.ToArray(), YlstVerh.ToArray());
      
        }

        //моделирование регрессии
        public static void ModelRegression(double Xmin, double Xmax, int N, double a, double b, double sigmaEpsilon, string filename)
        {
            /*
            double m = (Xmax - Xmin) / 2d;
            double sigmaX = (Xmax - Xmin) / 6d;

            List<double> lstY = new List<double>();

            var x = Modelirovanie.Normik(m, sigmaX, N);
            var eps = Modelirovanie.Normik(0, sigmaEpsilon, N);
            
            for (int i = 0; i < N; i++)
            {
                var y = a + b * Math.Log(x[i]) + eps[i];
                lstY.Add(y);
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < N; i++)
                {
                    var line = String.Format("{0:0.0000}\t{1:0.0000}", Math.Round(x[i], 4), Math.Round(lstY[i], 4));

                    writer.WriteLine(line);
                }
            }
            */
            double[] x = new double[N];
            double[] y = new double[N];
            double[] ep = new double[N];
            double Mx = (Xmax - Xmin) / 2.0;
            double SigX = (Xmax - Xmin) / 6.0;
            //X Y
            
            x =  NormalLol(Mx, SigX, N);
            ep = NormalLol(0, sigmaEpsilon, N);
            for (int k = 0; k < N; k++)
            {
                
                y[k] = a + b * Math.Log(x[k]) + ep[k];
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < N; i++)
                {
                    var line = String.Format("{0:0.0000}\t{1:0.0000}", Math.Round(x[i], 4), Math.Round(y[i], 4));

                    writer.WriteLine(line);
                }
            }
        }

        static double[] NormalLol(double m, double s, int n)
        /* m, s параметры норм распр n размер*/
        {
            Random F = new Random();
            double[] Var = new double[n];
            int k = 0;
            while (k < n)
            {
                double p = 2 * F.NextDouble() - 1;
                double t = 2 * F.NextDouble() - 1;
                double c = p * p + t * t;
                if (c > 0 && c <= 1)
                {
                    Var[k] = p * Math.Sqrt(-2 * Math.Log(c) / c);
                    k += 1;
                    if (k < n)
                    {
                        Var[k] = t * Math.Sqrt(-2 * Math.Log(c) / c);
                        k += 1;
                    }
                }
            }
            for (k = 0; k < n; k++)
            {
                Var[k] = m + s * Var[k];
            }
            return Var;
        }

        double _fi;
        //МГК
        public void MGK()
        {
            double tan2fi = 2 * _koefLinearCorrelation * _x.Sigma * _y.Sigma;
            tan2fi /= (_x.Dispersia - _y.Dispersia);

             _fi = Math.Atan(tan2fi) / 2d;

            //поворот
            double cos = Math.Cos(_fi);
            double sin = Math.Sin(_fi);

            double[] newX = new double[N];
            double[] newY = new double[N];

            double[] oldX = _x.d;
            double[] oldY = _y.d;

            double xExp = _x.Expectation;
            double yExp = _y.Expectation;

            for (int i = 0; i < N; i++)
            {   
                //Центрирование
                double x = oldX[i] - xExp;
                double y = oldY[i] - yExp;

                newX[i] = x * cos + y * sin;
                newY[i] = -x * sin + y * cos;
            }

            _x.Setd(newX);
            _y.Setd(newY);
        }

        public void BackRotate(SeriesCollection sCol)
        {
            sCol.Clear();

            //поворот
            double cos = Math.Cos(-_fi);
            double sin = Math.Sin(-_fi);

            double[] newX = new double[N];
            double[] newY = new double[N];

            double[] oldX = _x.d;
            double[] oldY = _y.d;

            for (int i = 0; i < N; i++)
            {
                //Центрирование
                double x = oldX[i];
                double y = oldY[i];
                
                newX[i] = x * cos + y * sin;
                newY[i] = -x * sin + y * cos;

                double seriaXstart = _pointsOfSeries[0][0].x;
                double seriaYstart = _pointsOfSeries[0][0].y;
                PointD startRotatedPoint = new PointD(seriaXstart * cos + seriaYstart * sin,
                                                      -seriaXstart * sin + seriaYstart * cos);

                double seriaXend = _pointsOfSeries[0][1].x;
                double seriaYend = _pointsOfSeries[0][1].y;

                PointD endRotatedPoint = new PointD(seriaXend * cos + seriaYend * sin,
                                                   -seriaXend * sin + seriaYend * cos);

                Series s = new Series();
                s.Color = _colorsList[i];
                sCol.Add(s);
            }

            _x.Setd(newX);
            _y.Setd(newY);
        }
    }
    
    //новая выборка для корреляционного отношения
    class koefRatioService
    {
        public double x;
        public double[] y;

        public int mi { get { return y.Length; } }
        public double AverageY
        {
            get 
            {
                if (y.Length != 0)
                    return y.Average();
                else
                    return 0;
            }
        }
    }

    //для рангов 
    class RankElement
    {
        public double data;
        public double rank;

        public override string ToString()
        {
 	         return data+", rank = "+rank;
        }
    }
    
    //расширенный вывод рангов - для Спирмена
    class ExtendedRanks
    {
        public RankElement[] rankedArray;
        //инфо о связках
        //длинна списка - z
        //i-элемент списка - количество одинаковых значений (А по книге)
        public List<int> BundlesInfo = new List<int>(); 
    }
    
    // класс для ранжирования выборок
    static class RankWork
    { 
        //ранжирует выборку
        public static RankElement[] CalcRanksForArray(double[] array)
        {
            //сортируем
            array = array.OrderBy(x => x).ToArray();

            int Length = array.Length;
            RankElement[] rankArray = new RankElement[Length];
            
            double summator=0;       //сумма порядков одинаковых элементов
            double savedData=0;      //ячейка для элементов, которые повторяются
            int counter=1;           //счетчик одинаковых значений (элементов, которые вошли в сумматор)
            int startIndex =0;       //индекс с которого начали повторяться значения
            for (int i = 0; i < Length; i++)
            {
                rankArray[i] = new RankElement();       //инициализация

                //проверка не выходит ли след элемент за край
                //и если текущий и след элементы совпадают
                if ((i < Length - 1) && (array[i] == array[i + 1])) 
                {
                    summator+=(i+1);            //накапливаем сумму порядков одинаковых элементов
                    counter++;                  //инкрементируем счетчик
                    if (counter == 2)
                        startIndex = i;         //запоминаем начальный индекс повторений
                    savedData = array[i];       //запоминаем значение
                }
                else if (counter > 1)       //когда элементы перестали повторяться
                {
                    summator += (i + 1);
                    //подсчет значение ранга в связке
                    double r = summator/counter;

                    //заполнение
                    for (int j = 0; j < counter; j++)
                    {
                        rankArray[startIndex+j].data = savedData;
                        rankArray[startIndex + j].rank = r;
                    }


                    //сбрасываем все служебные переменные
                    savedData = 0;
                    summator = 0;
                    counter = 1;
                    startIndex = -1;
                    continue;
                }
                else
                {
                    rankArray[i].data = array[i];
                    rankArray[i].rank = i+1;
                }
            }
            return rankArray;
        }
        //расширенное ранжирование. возвращает еще информацию о связках.
        public static ExtendedRanks CalcExtendedRanksForArray(double[] array)
        {
            ExtendedRanks extRank = new ExtendedRanks();
            //сортируем
            array = array.OrderBy(x => x).ToArray();

            int Length = array.Length;
            RankElement[] rankArray = new RankElement[Length];
            extRank.rankedArray = rankArray;

            double summator = 0;       //сумма порядков одинаковых элементов
            double savedData = 0;      //ячейка для элементов, которые повторяются
            int counter = 1;           //счетчик одинаковых значений (элементов, которые вошли в сумматор)
            int startIndex = 0;       //индекс с которого начали повторяться значения
            for (int i = 0; i < Length; i++)
            {
                rankArray[i] = new RankElement();       //инициализация

                //проверка не выходит ли след элемент за край
                //и если текущий и след элементы совпадают
                if ((i < Length - 1) && (array[i] == array[i + 1]))
                {
                    summator += (i + 1);            //накапливаем сумму порядков одинаковых элементов
                    counter++;                  //инкрементируем счетчик
                    if (counter == 2)
                        startIndex = i;         //запоминаем начальный индекс повторений
                    savedData = array[i];       //запоминаем значение
                }
                else if (counter > 1)       //когда элементы перестали повторяться
                {
                    summator += (i + 1);
                    //подсчет значение ранга в связке
                    double r = summator / counter;

                    //добавим в список количество повторяющихся элементов в связке
                    extRank.BundlesInfo.Add(counter);

                    //заполнение
                    for (int j = 0; j < counter; j++)
                    {
                        rankArray[startIndex + j].data = savedData;
                        rankArray[startIndex + j].rank = r;
                    }


                    //сбрасываем все служебные переменные
                    savedData = 0;
                    summator = 0;
                    counter = 1;
                    startIndex = -1;
                    continue;
                }
                else
                {
                    rankArray[i].data = array[i];
                    rankArray[i].rank = i + 1;
                }
            }
            return extRank;
        }
    
    }

    //коэффициенты сообщений таблиц 2 на 2
    class FactorsConnectionsTables
    {
        //для подсчетов значений
        double N00;
        double N01;
        double N10;
        double N11;
        double N0;
        double N1;
        double M0;
        double M1;

        //следующие элементы вынесены с целью оптимизации
        int N;              //размер данных
        double[] xData;     //массив по иксу
        double[] yData;     //массив по игреку
        double XAverage;    //Среднее иксу
        double YAverage;    //Среднее по у

        double alpha;       //для гипотез

        //конструктор
        public FactorsConnectionsTables(STAT x, STAT y,double alpha)
        {
            //заполняем данные
            xData = x.d;
            yData = y.d;

            XAverage = x.Expectation;
            YAverage = y.Expectation;

            N = xData.Length;

            //вызов ф-и подсчета значения N00, N01, N10, N11, M
            Perform_Table_Links();
        }

        //служебная ф-я (занимается заполнением полей) - вызывается из конструктора
        private void Perform_Table_Links()
        {
            //связываем икс и у
            Tuple<double, double>[] lst = new Tuple<double, double>[N];

            for (int i = 0; i < N; i++)
                lst[i] = Tuple.Create(xData[i], yData[i]);

            //Заполняем поля
            N00 = lst.Count(value => value.Item1 <= XAverage && value.Item2 <= YAverage);
            N01 = lst.Count(value => value.Item1 <= XAverage && value.Item2 > YAverage);
            N10 = lst.Count(value => value.Item1 > XAverage && value.Item2 <= YAverage);
            N11 = lst.Count(value => value.Item1 > XAverage && value.Item2 > YAverage);

            N0 = N00 + N01;
            N1 = N10 + N11;
            M0 = N00 + N10;
            M1 = N01 + N11;
        }

        //индекс Фехнера
        public double Index_Fehnera()
        {
            double index_Fehnera = (N00 + N11 - N10 - N01) / (N00 + N11 + N10 + N01);
            
            return index_Fehnera;
        }

        //коэффициент Фи
        public double Coefficient_Splitting_Fi()
        {
            double Coef_Fi = (N00 * N11 - N01 * N10) / Math.Sqrt(N0 * N1 * M0 * M1);
            
            return Coef_Fi;
        }

        //Коэф Юла Q
        public double Coefficient_Link_of_Yula_Q()
        {
            double Q = (N00 * N11 - N01 * N10) / (N00 * N11 + N01 * N10);
            
            return Q;
        }
        
        //Коэф Юла Y
        public double Coefficient_Link_of_Yula_Y()
        {
            double Y = (Math.Sqrt(N00 * N11) - Math.Sqrt(N01 * N10)) / (Math.Sqrt(N00 * N11) + Math.Sqrt(N01 * N10));
            
            return Y;
        }

        //ПРОВЕРКИ НА ЗНАЧИМОСТЬ
        
        public string Coefficient_Link_of_Yula_Y_significance()
        {
            double Y = Coefficient_Link_of_Yula_Y();
            double S = 1 / 4.0 * (1 - Y * Y) * Math.Sqrt(1.0 / N00 + 1.0 / N01 + 1.0 / N10 + 1.0 / N11);
            double U = Y / S;
            double Uk = Kvantili.Normal(alpha / 2.0);
            string significance = "Y = " + Math.Round(Y, 4) + Environment.NewLine + "U(" + (1 - alpha / 2.0) + ")= " + Math.Round(Uk, 4) + Environment.NewLine;
            significance += "U = " + Math.Round(U, 4) + Environment.NewLine;
            if (Math.Abs(U) <= Uk)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }
        ////////////////////
        public string Coefficient_Link_of_Yula_Q_significance()
        {
            double Q = Coefficient_Link_of_Yula_Q();
            double S = 1 / 2.0 * (1 - Q * Q) * Math.Sqrt(1.0 / N00 + 1.0 / N01 + 1.0 / N10 + 1.0 / N11);
            double U = Q / S;
            double Uk = Kvantili.Normal(alpha / 2.0);
            string significance = "I(or Q) = " + Math.Round(Q, 4) + Environment.NewLine + "U(" + (1 - alpha / 2.0) + ")= " + Math.Round(Uk, 4) + Environment.NewLine;
            significance += "U = " + Math.Round(U, 4) + Environment.NewLine;
            if (Math.Abs(U) <= Uk)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }
        //////////////////////
        public string Coefficient_Splitting_Fi_significance()
        {
            double Coef_Fi = Coefficient_Splitting_Fi();
            double Hi2 = 0;
            double Hi2k = Kvantili.Hi2(1 - alpha, 1);
            string significance = "Ф = " + Math.Round(Coef_Fi, 4) + Environment.NewLine + "Hi^2(" + (1 - alpha) + ",1)= " + Math.Round(Hi2k, 4) + Environment.NewLine;
            if (N >= 40 && N00 > 5 && N01 > 5 && N11 > 5 && N10 > 5)
                Hi2 = N * Coef_Fi * Coef_Fi;
            else
                Hi2 = N * Math.Pow(N00 * N11 - N01 * N10 - 0.5, 2) / (N0 * N1 * M0 * M1);
            significance += "Hi^2 = " + Math.Round(Hi2, 4) + Environment.NewLine;
            if (Hi2 >= Hi2k)
                significance += "Оцінка значуща!";
            else
                significance += "Оцінка не значуща!";
            return significance;
        }
    }

   
}
