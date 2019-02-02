using Chart1._1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chart5._1.TimeData
{
    class TimeDataAnalizer
    {
        STAT m_stat = new STAT();

        public void Read() {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                m_stat.Setd(ofd.FileName);
            }
        }


        Chart m_chart;
        internal void ouyputOnChart(Chart chart)
        {
            m_chart = chart;
            RefreshChart();
        }

        public Func<int, double> AvtoCovatation_New()
        {
            var data = m_stat.d;

            return (t) =>
            {
                int N = data.Length;
                double m = m_stat.Expectation;
                double sum = 0;

                for (int i = 0; i < N - t; i++)
                    sum += (data[i] - m) * (data[i + t] - m);

                return sum / (N - t);

            };
        }

        public Func<int, double> AvtoCorelation_New()
        {
            return (t) =>
            {
                var y = AvtoCovatation_New();
                return y(t) / y(0);

            };
        }

        internal void SMA(int n)
        {
            var d = m_stat.d;

            int N = d.Length;
            var SMA = d.Take(n).Average();
            var x = d.ToArray();
            List<double> result = new List<double>();

            int i = n;

            List<double> d_new = new List<double>();

            while (true)
            {
                d_new.Add(SMA);
                if (i == N)
                    break;

                SMA = SMA + (x[i] - x[i - n]) / n;
                i++;
            }

            m_stat.d = d_new.ToArray();
            
            RefreshChart();
        }

        private void RefreshChart()
        {
            m_chart.Series[0].Points.Clear();
            m_chart.Series[1].Points.Clear();


            var data = m_stat.d;

           m_chart.Series[0].Points.DataBindXY(Enumerable.Range(1, data.Length).ToArray(), data);
           m_chart.Series[1].Points.DataBindXY(Enumerable.Range(1, data.Length).ToArray(), data);

            //chart Design
            m_chart.ChartAreas[0].AxisY.Minimum = data.Min();
            m_chart.ChartAreas[0].AxisY.Maximum = data.Max();

        }

        internal void WMA(int n)
        {
            int N = m_stat.d.Length;
            var x = m_stat.d.ToArray();
            List<double> d_new = new List<double>();
            
            for (int i = n; i < N; i++)
            {
                    d_new.Add(x.Skip(i - n).Take(n).
                  Select((e, j) => new { index = j, elem = e }).
                  Sum(e => e.elem * (e.index + 1)) * 2d / (n * (n + 1)));
            }

            m_stat.d = d_new.ToArray();

            RefreshChart();
        }

        public void SmotheEMA_1(int n, double alpha)
        {
            var EMA = m_stat.d.Take(n).Average();

            SmotheEMA(n, alpha, EMA);
        }

        public void SmotheEMA_2(int n, double alpha)
        {
            var EMA = m_stat.d[0];

            SmotheEMA(n, alpha, EMA);
        }

        private void SmotheEMA(int n, double alpha, double EMA)
        {
            var d = m_stat.d;
            int N = d.Length;

            List<double> res = new List<double>();
            res.Add(EMA);


            for (int i = n; i < N; i++)
            {
                EMA = alpha * d[i] + (1 - alpha) * EMA;
                res.Add(EMA);
            }

            m_stat.d = res.ToArray();
            RefreshChart();
        }

        public void SmotheDMA_1(int n, double alpha)
        {
            var beginState = m_stat.d.Take(n).Average();

            SmotheDMA(n, alpha, beginState);
        }
        public void SmotheDMA_2(int n, double alpha)
        {
            var beginState = m_stat.d[0];

            SmotheDMA(n, alpha, beginState);
        }

        private void SmotheDMA(int n, double alpha, double BS)
        {
            var d = m_stat.d;
            int N = m_stat.d.Length;
            var res = new List<double>();

            var EMA = BS;
            var DMA = BS;

            res.Add(EMA);

            for (int i = n; i < N; i++)
            {
                EMA = alpha * d[i] + (1 - alpha) * EMA;
                DMA = alpha * EMA + (1 - alpha) * DMA;
                res.Add(DMA);
            }

            m_stat.d = res.ToArray();
            RefreshChart();
        }

        public void RemoveTrendByOSM()
        {
            int n = m_stat.d.Length;
            var x = m_stat.d;

            double b = (n * x.Select((e, i) => e * i).Sum() - x.Sum() * x.Select((e, i) => i).Sum()) /
                (n * x.Select((e, i) => i * i).Sum() - x.Select((e, i) => i).Sum() * x.Select((e, i) => i).Sum()),
                a = (x.Sum() - b * x.Select((e, i) => i).Sum()) / n;


            m_stat.d = m_stat.d.Select((s, i) => s - (a + b * i)).ToArray();

            RefreshChart();
        }

        public void Reconstruct(int M, IEnumerable<int> indexes)
        {
            m_stat.d = CaterpillarMethodHelper.Reconstruction_ForRec(m_stat, M, indexes).ToArray();
            RefreshChart();
        }
    }
}
