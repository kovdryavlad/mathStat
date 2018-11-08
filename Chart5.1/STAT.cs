using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Globalization;
using DialogWindowHelper;

namespace Chart1._1
{
    public partial class STAT
    {   //это начало логики//
        public double[] InputData;      //входные дныые для восстановления
        public double[] d;              //массив для изменения
        public int M;                   //количество столбцов
        public double h;                //шаг
        public double[] masX;
        double[] masY;
        public double[] masYVid;
        public double Max;
        public double Min;
        //это все было для первой гистограммы
        
        //для второй гистограммы серия 1 
        double[] EmpiricalF;            //эмпирическая ф-я распределения
        
        //для второй гистограммы серия 2 
        double[] Seria3X;
        double[] Seria3Y;

        public Series FirstGistSeria_Column;
        public Series SecondGistSeria1_StepLine;
        public Series SecondtGistSeria2_Point;

        public delegate void Otsenki();         //Описание делегата, как типа, для подсчета оценок 
                                                //сигнатура void(void)

        public void Setd(double[] input)
        {
            d = input;

            CommonPartOfSetd();
        }

        //для использования в двумерных данных
        public void Setd2Stat2d(double[] input)
        {
            d = input;
            CalcMainParams();
        }

        public void CalcMainParams()
        {
            CalcExpectation();
            CalcDisp();
            SetM();
            Seth();
            Min = d.Min();
            Max = d.Max();
            FillIntervalsForExpectation();
            FillIntervalsForSigmas();
        }

        public void Setd(string filename)
        {
            d = System.IO.File.ReadAllLines(filename)
                    .SelectMany(str => str.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(s =>
                    {
                        double a;
                        return double.TryParse(
                         s.Replace('.', ','),
                        out a) ? a : (double?)null;
                    })
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .ToArray();

            CommonPartOfSetd();
        }

        
        public void CommonPartOfSetd()
        {
            InputData = (double[])d.Clone();
            
            //добавил пятого мая
            _FTeor = new double[d.Length];
        }

        public void RestoreInputData()
        {
            d = (double[])InputData.Clone();
        }

        private void SetM()
        {
            double _m;
            if (d.Length >= 100)
                _m = Math.Truncate(Math.Pow((double)d.Length, 1d / 3));
            else
                _m = Math.Truncate(Math.Pow((double)d.Length, 1d / 2));
            
            if (_m % 2 == 0)
                _m--;
            
            M = (int)_m;
        }

        public void Seth(int m)
        {
            int temp = -1;
            for (int i = 0; i < d.Length; i++)
            {
                if (double.IsNaN(d[i]))
                {
                    temp = i;
                }
            }
            
            this.Max = d.Max();
            this.Min = d.Min();
            this.h = (Max - Min) / m;
            h = Math.Round(h + 0.00009, 4);

        }

        private void Seth()
        {
            this.Max = d.Max();
            this.Min = d.Min();
            this.h = (Max- Min) / M;
            h = Math.Round(h+0.0001,4);
        }

        private void FormDataSeria1()
        {

            SetM();
            Seth();

            masX = new double[M];
            masY = new Double[M];
            masYVid = new double[M];

            var array = d;
            var n = d.Length;
            var step = h;

            for (int i = 0; i < n; i++)
            {
                int index = (int)Math.Truncate((array[i]-Min) / step);
                masY[index]++;
            }

            // masY[M - 1]++;//добавляем максимальный элемент 
            var Mlen = M;
            for (int i = 0; i < Mlen; i++)
                masX[i] = Min + (i+0.5) * step;

            //для перевода количества вхождений в частоту
            for (int i = 0; i < masY.Length; i++)
                masYVid[i] = masY[i] / d.Length;
        }

        private void Designer_Seria1(Series s)
        {
            //инициализация
            //s = new Series();

            s.Points.Clear();
            //FirstGistSeria_Column.Name = "Chart1Series1";
            s.CustomProperties = "PointWidth=1";
            s.BorderColor = Color.Black;
            s.BorderWidth = 1;
            //s.Color = Color.Orange;
        }

        public Series GetSeria1_Column(Series s)
        {
            Designer_Seria1(s);
            FormDataSeria1();

            s.Points.DataBindXY(masX, masYVid);

            return FirstGistSeria_Column;
        }

        //для второго чарта
        //2.1
        private void Designer_Seria2(Series s)
        {
            //s = new Series();
            s.Points.Clear();
            s.CustomProperties = "PointWidth=1";
            s.BorderWidth = 0;
            s.Color = Color.Transparent;
            s.BackImage = Environment.CurrentDirectory + @"\line.png";
           
            //s.BackImage = @"\line.png";
            s.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Unscaled;
        }

        public void FormDataSeria2()
        {
            double _const = 0.000000;   ///костыленок, чтобы приподнять линию
            ///
            EmpiricalF = new double[M];
            EmpiricalF[0] = masYVid[0];

            for (int i = 1; i < M; i++)
                EmpiricalF[i] = masYVid[i] + EmpiricalF[i - 1] + _const;

        }

        public Series GetSeria2_StepLine(Series s)
        {
            Designer_Seria2(s);
            FormDataSeria2();

            s.Points.DataBindXY(masX, EmpiricalF);
            
            return SecondGistSeria1_StepLine;
        }

        //2.2
        private void Designer_Seria3(Series s)
        {
            s.Points.Clear();
            //s = new Series();
            //SecondtGistSeria2_Point.Name = "Chart2Series2";
            //SecondtGistSeria2_Point.Color = Color.Blue;
            s.ChartType = SeriesChartType.Point;
        }

        public void FormDataSeria3()
        {
            var arr = d.OrderBy(v=>v).ToArray();

            List<double> pointsX = new List<double>();
            List<double> pointskolvoY = new List<double>();//называется как будто там лежит количество повторов каждого 
            //числа, но на деле оно делиться на длинну d.Length потому double

            int _number = 0; //счетчик которой говорит до какого элемента дошли
            int vhod;

            var len = arr.Length;
            for (; _number < len; _number++)
            {
                //pointsX.Add(d[_number]);
                vhod = 1;

                if (_number != len - 1)//чтобы исключить возможность входа в цикл с последним элементом _number == d.Length-1
                {
                    while ((_number + 1) != len && arr[_number + 1] == arr[_number])
                    {
                        vhod++;
                        _number++;
                    }
                }

                //изменения для того, чтобы массивы Seria3X и Seria3Y были
                //такой же длинны как и входной массив
                int j = 0;
                
                do
                {
                    j++;
                    pointsX.Add(arr[_number]);
                    pointskolvoY.Add((double)vhod /len);    
                
                } while (j!=vhod);
                
            }

            Seria3X = pointsX.ToArray();
            Seria3Y = pointskolvoY.ToArray();

            for (int i = 1; i < Seria3Y.Length; i++)
            {
                //изменено в мае
                
                if (Seria3X[i]==Seria3X[i-1])
                    Seria3Y[i] = Seria3Y[i - 1];
                else 
                    Seria3Y[i] += Seria3Y[i - 1];

                //Seria3Y[i] = Math.Round(Seria3Y[i], 4);
            }
        }

        public Series GetSeria3_Points(Series s)
        {
            Designer_Seria3(s);
            FormDataSeria3();

            s.Points.DataBindXY(Seria3X, Seria3Y);

            return SecondtGistSeria2_Point;
        }

        //когда графики построены
        
        //точечные оценки

        public double Expectation;  //Мат ожидание
        public double Dispersia;    //дисперсия
        double Dispersia_zsun;      //дисперсия  сдвинутая
        public double Sigma;        //среднее квадр. отклонение
        double Sigma_zsun;          //среднее квадр. отклонение сдвинутое
        double A_zsun;              //коэффициент ассиметрии сдвинутый
        public double A;            //коэффициент ассиметрии не сдвин
        public double MED;          //медиана
        public double MAD;          //медиана абсолютных отклонений
        public double Excess;       //коэффициент екцесса
        public double Excess_zsun;  //коэффициент екцесса сдвинутый
        public double KontrExcess;  //коэффициент контрексцесса
        public double W;            //коэффициент вариации Пирсона
        public double NeparamW;     //Непараметрический коэффициент вариации Пирсона
        public double Uolsh;        //Медиана средних Уолша
        public double UsichSer;     //Усеченное среднее 

        public Otsenki D_Calc_tochchnie_otsenki;    //делегат, для точечных оценок
        
        public void CalcExpectation() // математическое ожидание
        {
            double _result = 0;
            for (int i = 0; i < d.Length; i++)
                _result += d[i];
            Expectation = _result / d.Length;
        }
        
        public void CalcSDisp_zsun() // дисперсия сдвинутая
        {
            double S = 0;
            for (int i = 0; i < d.Length; i++)
                S += d[i] * d[i] - Expectation * Expectation;

            S /= d.Length;

            Dispersia_zsun = S;
            Sigma_zsun = Math.Sqrt(S);
        }

        public static double ArrayMediana(double[] mas)
        {
            int _n = mas.Length;
            double _med;
            int _sr = (int)_n / 2; //середина массива 
            if (_n % 2 == 0)
                _med = (mas[_sr - 1] + mas[_sr]) / 2;
            else
                _med = mas[_sr];


            return _med;
        }

        public void CalcMEDnMAD()
        {
            MED = ArrayMediana(d);

            double[] _arrForMad = new double[d.Length];
            for (int i = 0; i < d.Length; i++)
                _arrForMad[i] = Math.Abs(d[i] - MED);

            Array.Sort(_arrForMad);
            MAD = 1.483 * ArrayMediana(_arrForMad);
        }

        public void CalcDisp() //Дисперсия
        {
            this.CalcSDisp_zsun();

            double S = 0;
            for (int i = 0; i < d.Length; i++)
                S += Math.Pow((d[i] - Expectation), 2);

            S /= d.Length - 1;

            Dispersia = S;
            Sigma = Math.Sqrt(S);
        }

        void CalcA_zsun()
        {
            double _a_zs = 0;
            for (int i = 0; i < d.Length; i++)
                _a_zs += Math.Pow((d[i] - Expectation), 3);
            A_zsun = _a_zs / (d.Length * Math.Pow(Sigma_zsun, 3));
        }

        public void CalcA()
        {
            this.CalcA_zsun();
            A = Math.Sqrt(d.Length * (d.Length - 1)) / (d.Length - 2) * A_zsun;
        }

        public void CalcExcess_zsun()
        {
            double _sum = 0;
            for (int i = 0; i < d.Length; i++)
                _sum+= Math.Pow((d[i] - Expectation), 4);

            Excess_zsun = _sum / (d.Length * Math.Pow(Sigma_zsun, 4));
        }

        public void CalcExcess()
        {
            CalcExcess_zsun();

            double a = (double)(d.Length * d.Length - 1) / ((d.Length - 2) * (d.Length - 3));
            double b = ((Excess_zsun - 3) + 6d / (d.Length + 1));
            
            Excess = a*b;
        }

        public void CalcKontrExcess()
        {
            KontrExcess = 1 / Math.Sqrt(Math.Abs(Excess));
        }

        public void CalcW()
        {
            W = Sigma/Expectation;
        }

        public void CalcNeparamW()
        {
            NeparamW = MAD / MED;    
        }

        public void CalcUolsh()
        {
            if (d.Length>5000)
            {
                Uolsh = double.NaN;
                return;
            }

            List<double> list2Uolsh = new List<double>();
           
            for (int i = 0; i < d.Length; i++)
                for (int j = i; j < d.Length; j++)
                    list2Uolsh.Add((d[i] + d[j])/2);

            var UolshArray = list2Uolsh.ToArray();

            Array.Sort(UolshArray);

            Uolsh = ArrayMediana(UolshArray);
        }

        double alphaForUsichSer=0;         //коеффициент усечения

        public void CalcUsichSer()
        {
            if (alphaForUsichSer > 0.5)
            {
                UsichSer = MED;
                return;
            }

            int k = (int)Math.Truncate(alphaForUsichSer * d.Length);

            double sum = 0;

            for (int i = k; i < d.Length-k; i++)
                sum += d[i];
            
            UsichSer = sum/(d.Length-2*k);
        }

        //закончились точечные оценки

        //перед интервальными оценками
        /*
        private double Beta(int k)//для получения бета
        {
            double _beta=0;
            
            if(k%2!=0)
            {
                double a = CentralMoment(3)*CentralMoment(k-1+3);
                double b = Math.Pow(CentralMoment(2), (k-1)/2d+3);
                _beta = a / b;
            }
            else
	        {
                double a = CentralMoment(k + 2);
                double b = Math.Pow(CentralMoment(2),k/2d+1);
                _beta = a / b;
	        }
          
            //throw new Exception("Смотри метод подсчета Бета\n Он не реализован до конца... Спрашивай Лолиту");
            return _beta;
        }
        */
        ///<конец "для интервальных оценок" 

        //интервальные оценки 
        
        //Сигмы для интервальных оценок
        ///< префикс - Sigma_ 
        public double Sigma_Expectation;       //для мат ожидания
        public double Sigma_Dispersia;         //для дисперсии
        public double Sigma_A;                 //для коэф. асиметрии
        public double Sigma_Excess;            //для коэф. ексцесса
        public double Sigma_KontrExcess_zsun;  //для коэф. контр-ексцесса
        public double Sigma_W;                 //для коэф. вариации Пирсона
        
        public Otsenki D_Calc_Sigmi_dlya_intervalnih_otsenok;     //в этот делегат будут добавлены все методы(в конструкторе),
                                                                  //которые считают интервальные оценки

        public void CalcSigma_Expectation()
        {
            Sigma_Expectation = Sigma / Math.Sqrt(d.Length);
        }

        public void CalcSigma_Dispersia()
        {
            Sigma_Dispersia = Sigma / Math.Sqrt(2*d.Length);
        }
      
        public void CalcSigma_KontrExcess_zsun()
        {
            double a = Math.Sqrt((Math.Abs(Excess_zsun) / (29d * d.Length)));
            double b = Math.Pow(Math.Pow(Math.Abs(Excess_zsun*Excess_zsun-1),3),1d/4);
            Sigma_KontrExcess_zsun = a * b;
        }

        public void CalcSigma_W()
        {
            double a = 1 + 2 * W * W;
            double b = 2 * d.Length;
            Sigma_W=W*Math.Sqrt(a/b);
        }

        public void CalcSigma_A()
        {
            double a = 6d / d.Length;
            double b = 1 - 12d /( 2 * d.Length + 7);
            Sigma_A = Math.Sqrt(a * b);
        }

        public void CalcSigma_Excess()
        {
            double a = 24d / d.Length;
            double b = 1 - 225d / (15 * d.Length + 124);
            Sigma_Excess = Math.Sqrt(a * b);
        }


        //Оценки
        //формат названия переменных - Параментр_нижняя тета,Параментр_верхняя тета
        public double Expectation_niz;
        public double Expectation_verh;

        public double Sigma_niz;
        public double Sigma_verh;

        public double A_niz;
        public double A_verh;

        public double Excess_niz;
        public double Excess_verh;

        public double KontrExcess_niz;
        public double KontrExcess_verh;

        public double W_niz;
        public double W_verh;
        
        public double gamma;

        public double _kvantil;

        //интервальные оценки среднего //для StatNd
        public void FillIntervalsForExpectation()
        {
            CalcSigma_Expectation();

            _kvantil = AutoKvantilToIntervalAppraisal();

            Expectation_niz = Expectation - _kvantil * Sigma_Expectation;
            Expectation_verh = Expectation + _kvantil * Sigma_Expectation;
        }

        //интервальные оценки среднего //для StatNd
        public void FillIntervalsForSigmas()
        {
            CalcSigma_Dispersia();

            _kvantil = AutoKvantilToIntervalAppraisal();

            Sigma_niz = Sigma - _kvantil * Sigma_Dispersia;
            Sigma_verh = Sigma + _kvantil * Sigma_Dispersia;
        }

        public void IntervalOcenki()
        {
            D_Calc_Sigmi_dlya_intervalnih_otsenok();

            _kvantil = AutoKvantilToIntervalAppraisal();

            //ищем значение квантиля
           

            Expectation_niz = Expectation - _kvantil * Sigma_Expectation;
            Expectation_verh = Expectation + _kvantil * Sigma_Expectation;

            Sigma_niz = Sigma - _kvantil * Sigma_Dispersia;
            Sigma_verh = Sigma + _kvantil * Sigma_Dispersia;

            A_niz = A - _kvantil * Sigma_A;
            A_verh = A + _kvantil * Sigma_A;

            Excess_niz = Excess - _kvantil * Sigma_Excess;
            Excess_verh = Excess + _kvantil * Sigma_Excess;

            KontrExcess_niz = KontrExcess - _kvantil * Sigma_KontrExcess_zsun;
            KontrExcess_verh = KontrExcess + _kvantil * Sigma_KontrExcess_zsun;

            W_niz = W - _kvantil * Sigma_W;
            W_verh = W + _kvantil * Sigma_W;
        }

        private double AutoKvantilToIntervalAppraisal()
        {
            double alpha = 1 - gamma;

            if (d.Length > 60)
               return Kvantili.Normal(alpha / 2);
            else
                return Kvantili.Student(alpha / 2, d.Length - 1);
        }
        public STAT()
        {
            //точечные оценки
            D_Calc_tochchnie_otsenki += CalcExpectation;
            D_Calc_tochchnie_otsenki += CalcMEDnMAD;
            D_Calc_tochchnie_otsenki += CalcDisp;
            D_Calc_tochchnie_otsenki += CalcA;
            D_Calc_tochchnie_otsenki += CalcExcess;
            D_Calc_tochchnie_otsenki += CalcKontrExcess;
            D_Calc_tochchnie_otsenki += CalcW;
            D_Calc_tochchnie_otsenki += CalcNeparamW;
            D_Calc_tochchnie_otsenki += CalcUolsh;
            D_Calc_tochchnie_otsenki += CalcUsichSer;

            //сигмы для интервальных
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_Expectation;
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_Dispersia;
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_A;
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_Excess;
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_KontrExcess_zsun;
            D_Calc_Sigmi_dlya_intervalnih_otsenok += CalcSigma_W;

            //для воссоздания распределений
            ReproductFunctions = new List<Action<Series, Series, Series, Series>>();
            ReproductFunctions.Add(ReproductExp);
            ReproductFunctions.Add(ReproductArcSin);
            ReproductFunctions.Add(ReproductNormal);
            ReproductFunctions.Add(ReproductLaplasa);
            ReproductFunctions.Add(ReproductReley);
            ReproductFunctions.Add(ReproductVeibula);

            //некоторые параметры
            gamma = 0.9;
        }
    }

}
