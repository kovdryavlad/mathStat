using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chart1._1;

namespace Chart5._1
{
    class VariationalSeriesBuilder
    {
        STAT[] stats;
        int dimentions;
        double[] h;
        int[] M;
        Array array;
        int N;

        public VariationalSeriesBuilder(STAT[] stats)
        {
            this.stats = stats;

            //измерения
            dimentions = stats.Length;

            //заполнение массивов шагов и кол-ва вариант в одномерном случае по измерению
            h = new double[dimentions];
            M = new int[dimentions];

            for (int i = 0; i < dimentions; i++)
            {
                h[i] = stats[i].h;
                M[i] = stats[i].M;
            }

            //создание многомерного массива
            array = Array.CreateInstance(typeof(VarintInSeries), M);

            //кол-во наблюдений
            N = stats[0].d.Length;

            //по каждой строке
            for (int i = 0; i < N; i++)
            {
                //индексы многомерного массива
                int[] indexes = new int[dimentions];

                //по каждому признаку 
                //выясняем индексы варианты в многомерном ряде
                for (int j = 0; j < dimentions; j++)
                {
                    STAT currenStat = stats[j];
                    indexes[j] = (int)Math.Truncate((currenStat.d[i] - currenStat.Min) / h[j]);
                }

                //работа с самоц вариантой
                VarintInSeries CurrentVariant = array.GetValue(indexes) as VarintInSeries;

                if (CurrentVariant == null)
                    array.SetValue(new VarintInSeries() { n = 1, p = 1d/N }, indexes);
                else
                    CurrentVariant.Add(N);

                
            }
        }

        int[] IndexToCoordinates(int i)
        {
            var dims = Enumerable.Range(0, array.Rank)
                .Select(array.GetLength)
                .ToArray();

            Func<int, int, int> product = (i1, i2) => i1 * i2;

            return dims.Select((d, n) => (i / dims.Take(n).Aggregate(1, product)) % d).ToArray();
        }

        public string OutputAllVariationalSeries()
        {
            string res="";

            var i = 0;

            foreach (VarintInSeries item in array)
            {
                var coords = IndexToCoordinates(i++);

                VarintInSeries CurrentVariant = array.GetValue(coords) as VarintInSeries;
                if (CurrentVariant == null)
                    continue;

                                
                //Console.WriteLine(string.Join(", ", coords));
                Func<double, double> r = v => Math.Round(v, 4);

                res += "(";
                for (int j = 0; j < dimentions; j++)
                {
                    var bottomValue = coords[j] * h[j] + stats[j].Min;
                    var topValue = bottomValue+h[j];

                    res += r(bottomValue);
                    res += " - ";
                    res += r(topValue);

                    if (j!=dimentions-1)
                    res += ";     \t";
                }

                res += ")";

                res += string.Format("\t\tn = {0}\t\tp = {1:0.000}{2}", CurrentVariant.n, CurrentVariant.p, Environment.NewLine);
            }

            return res;
        }
    }

    //класс варианта
    class VarintInSeries
    {
        public int n;
        public double p;

        public void Add(int N)
        {
            n++;
            p += 1d / N;
        }
    }
}
