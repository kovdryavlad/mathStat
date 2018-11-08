using System;

namespace Disrtibutions
{
    
    public interface IDistribution
    {
        double f(double x);       //функция плотности распределения
        double F(double x);       //функция распределения
    }

    public interface IReproductable
    {
        double f(double x);       //функция плотности распределения
        double F(double x);       //функция распределения
        double DF(double x);      //для интервальных оценок
    }

    public abstract class OneDimentionalDistribution : IDistribution, IReproductable
    {
        public string Name { get; set; } 

        public abstract double f(double x);
        
        public abstract double F(double x);

        public abstract double DF(double x);

        protected double AutoDF(double x, double dFdt1, double Dt1, double dFdt2, double Dt2, double cov) 
        {
            return (dFdt1 * dFdt1 * Dt1 + dFdt2 * dFdt2 * Dt2 + 2 * dFdt1 * dFdt2 * cov);
        }
    }

    class Exponential: OneDimentionalDistribution
    {        
        public double _l;

        public double _Dl;

        public Exponential(double lyambda)
        {
            Name = "Експоненціальний";

            _l = lyambda;
        }

        public Exponential(double lyambda, double Dl)
            :this(lyambda)
        {
            _Dl = Dl;
        }

        //пошла чистая математика(программирование формул)

        public override double f(double x)
        {
            if (x < 0)
                return 0;
            else
                return _l * Math.Exp(-_l * x);
        }

        public override double F(double x)
        {
            if (x < 0)
                return 0;
            else
                return 1 - Math.Exp(-_l * x);
        }

        public override double DF(double x)
        {
            return x * x * Math.Exp(-2 * _l * x)*_Dl;
        }
    }

    class Arcsin : OneDimentionalDistribution
    {
        double _a;

        double _Da;

        public Arcsin(double a)
        {
            Name = "Арксинуса";

            _a = a;
        }

        /// <summary>Задание параметров для распределения Арксинуса</summary>
        /// <param name="a">Параметр а</param>
        /// <param name="Da">Дисперсия параметра а</param>
        public Arcsin(double a, double Da)
            :this(a)
        {
            _Da = Da;
        }

        public override double f(double x)
        {
            if (x >= -_a && x <= _a)
                return 1d / (Math.PI * Math.Sqrt(_a * _a - x * x));
            else
                return 0;
        }

        public override double F(double x)
        {
            if (x >= -_a && x <= _a)
                return 1d / 2 + 1d / Math.PI * Math.Asin(x / _a);
            else
                return 0;
        }

        public override double DF(double x)
        {
            return Math.Pow(-x / (Math.PI * _a * Math.Sqrt(_a * _a - x * x)), 2) * _Da;
        }
    }

    class Normal : OneDimentionalDistribution
	{
        double _m;

        double _s;

        double _Dm;

        double _Ds;

        public Normal(double m, double s)
        {
            Name = "Нормальний";

            _m = m;

            _s = s;
        }

        public Normal(double m, double Dm, double s, double Ds)
            :this(m, s)
        {
            _Dm = Dm;

            _Ds = Ds;
        }
        
        public override double f(double x)
        {
            return Math.Exp(-(Math.Pow(x - _m, 2) / (2 * _s * _s))) / (_s * Math.Sqrt(2 * Math.PI));
        }

        public override double F(double x) 
        {
            return Laplas((x - _m) / _s);
        }

        public override double DF(double x)
        {
            return dFdm(x) * dFdm(x) * _Dm + dFds(x) * dFds(x) * _Ds;
        }

        double dFdm (double x)
        {
            return -1d / (_s * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(x - _m, 2) / (2 * _s * _s));
        }

        double dFds(double x)
        {
            return (x - _m) / _s * dFdm(x);
        }
                            
        //Функция для формулы Лапласа Ф
        //она тут быть не должна
        private double Laplas(double u)
        {
            if (u < 0)
                return 1 - Laplas(-u);

            double t = 1d / (1 + 0.231649 * u);

            double b1 = 0.31938153;

            double b2 = -0.356563782;

            double b3 = 1.781477937;

            double b4 = -1.821255978;

            double b5 = 1.330274429;

            double result = 1 - 1d / Math.Sqrt(2 * Math.PI) * Math.Exp(-u * u / 2d) * (b1 * t + b2 * t * t + b3 * t * t * t + b4 * t * t * t * t + b5 * Math.Pow(t, 5));

            return result;
        }
	}

    class Laplasa : OneDimentionalDistribution
    {
        double _m;

        double _l;

        double _Dl;

        double _Dm;

        double _cov;

        public Laplasa(double m, double l)
        {
            Name = "Лапласа";

            _m = m;

            _l = l;
        }

        public Laplasa(double m, double Dm, double l, double Dl, double cov)
            :this(m,l)
        {
            _Dm = Dm;

            _Dl = Dl;

            _cov = cov;
        }

        //математика

        public override double f(double x)
        { 
            return _l / 2d * Math.Exp(-_l * Math.Abs(x - _m));
        }

        public override double F(double x)
        {
            double pow = _l * (x - _m);

            if (x <= _m)
                return 0.5 * Math.Exp(pow);
            else
                return 1 - 0.5 * Math.Exp(-pow);
        }

        public override double DF(double x)
        {
            return base.AutoDF(x, dFdl(x), _Dl, dFdm(x), _Dm, _cov);
        }

        double dFdl (double x)
        {
            double pow = _l * (x - _m);

            double a = 0.5 * (x - _m);

            if (x <= _m)
                return a * Math.Exp(pow);
            else
                return a * Math.Exp(-pow);
        }

        double dFdm (double x)
        {
            double pow = _l * (x - _m);

            double a = -_l / 2;

            if(x <= _m )
                return a * Math.Exp(pow);
            else
            return a * Math.Exp(-pow);
        }
    }

    class Reley : OneDimentionalDistribution
    {
        double _s;

        double _Ds;

        public Reley(double s)
        {
            Name = "Релея";

            _s = s;
        }

        public Reley(double s, double Ds)
            :this(s)
        {
            _Ds = Ds;
        }

        //математика
        //часть, которая используется и в f и в F, решил вынести отдельно. 
        double ExpPart(double x)
        {
            return Math.Exp(-x * x / (2 * _s * _s));
        }

        public override double f(double x)
        {
            if (x < 0 )
                return 0;
            else 
                return x / (_s * _s) * ExpPart(x);
        }

        public override double F(double x)
        {
            if (x < 0)
                return 0;
            else
                return 1 - ExpPart(x);
        }

        public override double DF(double x)
        { 
             return Math.Pow(-Math.Pow(x, 2) / Math.Pow(_s, 3) * ExpPart(x), 2) * _Ds;
        }
    }

    class Veibula : OneDimentionalDistribution
    {
        double _alpha;

        double _beta;

        double _DAlpha;

        double _DBeta;

        double _cov;

        public Veibula(double alpha, double beta)
        {
            Name = "Вейбула";

            _alpha = alpha;

            _beta = beta;
        }

        public Veibula(double alpha, double Dalpha, double beta, double Dbeta, double cov)
               :this(alpha, beta)
        {
            _DAlpha = Dalpha;

            _DBeta = Dbeta;

            _cov = cov;
        }

        //общая в производных и функциях f и F
        double CommonPart(double x)
        {
            return Math.Exp(-Math.Pow(x, _beta) / _alpha);
        }

        public override double f(double x)
        {
            if (x >= 0 && _alpha > 0 && _beta > 0)
                return _beta / _alpha * Math.Pow(x, _beta - 1) * CommonPart(x);
            else
                return 0;
        }

        public override double F(double x)
        {
            if (x >= 0 && _alpha > 0 && _beta > 0)
                return 1 - CommonPart(x);
            else
                return 0;
        }

        public override double DF(double x)
        {
            return base.AutoDF(x, dFdAlpha(x), _DAlpha, dFdBeta(x), _DBeta, _cov);
        }

        double dFdAlpha(double x)
        {
            return -Math.Pow(x, _beta) * CommonPart(x) / (_alpha * _alpha);
        }

        double dFdBeta(double x)
        {
            return Math.Pow(x, _beta) / _alpha * Math.Log(x) * CommonPart(x);
        }
    }

    class Ravn : OneDimentionalDistribution
    {
        public double _a;
        public double _b;
        public double _cov;

        public double _Da;
        public double _Db;


        public Ravn(double a, double b)
        {
            Name = "Рівномірний";

            _a =a;
            _b = b;
        }

        public Ravn(double a, double Da, double b, double Db, double covariation)
            :this(a,b)
        {
            _Da = Da;
            _Db = Db;

        }

        //пошла чистая математика(программирование формул)

        public override double f(double x)
        {
            if (x < _a)
                return 0;
            else if (x >= _b)
                return 0;
            else
                return 1d / (_b - _a);
        }

        public override double F(double x)
        {
            if (x < _a)
                return 0;
            else if (x >= _b)
                return 0;
            else
                return (x-_a)/(_b-_a);
        }

        public override double DF(double x)
        {
            double Value = 0;

            double denominator = Math.Pow(_b - _a, 4);

            Value += Math.Pow(x - _b, 2) / denominator* _Da;
            Value += Math.Pow(x - _a, 2) / denominator* _Db;
            Value -= 2 * (x - _b) * (x - _a) * _cov;

            return Value;
        }
    }



    public abstract class TwoDimentionalDistribution
    {
        public string Name { get; set; }

        public abstract double f(double x, double y);
    }

    //двумерный нормальный
    class TwoDimNormal:TwoDimentionalDistribution
    {
        //х среднее
        double _xSr;
        double _ySr;
        //сигмы
        double _SigmaX;
        double _SigmaY;
        //коэф. корреляции
        double _Rxy;

        /// <summary>
        /// Плотность двумерного нормального распределения
        /// </summary>
        /// <param name="xExpectation">х среднее</param>
        /// <param name="yExpectation">у среднее</param>
        /// <param name="SigmaX">Сигма х</param>
        /// <param name="SigmaY">Сигма н</param>
        /// <param name="Rxy">Коэффициент корреляции</param>
        public TwoDimNormal(double xExpectation, double yExpectation, double SigmaX, double SigmaY, double Rxy)
        {
            Name = "Нормальний двовимірний";

            _xSr = xExpectation;
            _ySr = yExpectation;
            _SigmaX = SigmaX;
            _SigmaY = SigmaY;
            _Rxy = Rxy;
        }

        public override double f(double x, double y)
        {
            double a = 1d/(2*Math.PI*_SigmaX*_SigmaY);
            double b = Math.Pow((x - _xSr) / _SigmaX, 2) + Math.Pow((y - _ySr) / _SigmaY, 2);

            return a * Math.Exp(-0.5 * b);
        }

        public double fLong(double x, double y)
        {
            double a = 1d / (2 * Math.PI * _SigmaX * _SigmaY*Math.Sqrt(1-_Rxy*_Rxy));
            double b = Math.Pow((x - _xSr) / _SigmaX, 2) + Math.Pow((y - _ySr) / _SigmaY, 2)-2*_Rxy*(x-_xSr)*(y-_ySr)/(_SigmaX*_SigmaY);

            return a * Math.Exp(-1d / (2 * 1 - _Rxy * _Rxy) * b);
        }
    }
}