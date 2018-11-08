using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    public class KZKolmogorovEventArgs : EventArgs
    {
        double _DNp,
               _DNm,
               _z,
               _k,
               _Kz,
               _Pz,
               _alpha;

        string _NameofCriteria = "Колмогорова",
               _resultOfTest;

        public KZKolmogorovEventArgs(double alpha, double DNp, double DNm, double z, 
                                     double k, double Kz, double Pz, string result)
        {
            _alpha = alpha;

            _DNm = DNm;

            _DNp = DNp;

            _z = z;

            _k = k;

            _Kz = Kz;

            _Pz = Pz;

            _resultOfTest = result;
        }

        public double Alpha { get { return _alpha; } }

        public double DNp { get { return _DNp; } }

        public double DNm { get { return _DNm; } }

        public double z { get { return _z; } }

        public double k { get { return _k; } }

        public double Kz { get { return _Kz; } }

        public double Pz { get { return _Pz; } }

        public string ResultOfTest { get { return _resultOfTest; } }
    }

    public class ReproductEventArgs : EventArgs
    {
        int _NumberParams;
        
        public List<Param> Params = new List<Param>();

        public ReproductEventArgs(Param param1)
        {
            Params.Add(param1);

            _NumberParams = 1;
        }

        public ReproductEventArgs(Param param1, Param param2)
        :this(param1)
        {
            Params.Add(param2);

            _NumberParams++;
        }

        public int NumberofParams { get { return _NumberParams; } }
    }

    public class Param
    {
        string _name;

        double _value;

        double _sigma;

        double _niz;

        double _verh;

        public Param(string name, double value, double sigma, double kvantil)
        {
            _name = name;

            _value = value;

            _sigma = sigma;

            _niz = value - kvantil * _sigma;

            _verh = value + kvantil * _sigma;
        }

        public String Name { get { return _name; } }

        public double Value { get { return _value; } }

        public double Niz { get { return _niz; } }

        public double Verh { get { return _verh; } }

        public double Sigma { get { return _sigma; } }
    }

    public class Viborka
    {
        public double[] data;

        public string Name { get; set; }

        public STAT stat = new STAT();

        public double expectation;

        public double Dispersion;

        public Viborka(double[] array, string name)
        {
            data = array;

            Name = name;

            stat.Setd(data);

            stat.CalcExpectation();

            stat.CalcDisp();

            expectation = stat.Expectation;

            Dispersion = stat.Dispersia;
        }

        public Viborka(STAT stat, string name)
        {
            this.stat = stat;

            data = stat.d;

            //stat.FillDBeforeSorting();

            Name = name;
            
            expectation = stat.Expectation;

            Dispersion = stat.Dispersia;
        }

    }

    public class Element : IComparable
    {
        public double data;
        public double rang;
        public int numberOfSample;

        public int CompareTo(object secondel)
        {
            Element el = secondel as Element;

            return data.CompareTo(el.data);
        }
    }

}
