using Chart1._1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chart5._1.KAverage
{
    class Visualization
    {
        public void GetMatrixOfScatterDiagrams(TableLayoutPanel tableLayout)
        {
            /*
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
            }*/
        }

    }
}
