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
    class VisualizationOFClasterization
    {
        public static void GetMatrixOfScatterDiagrams(TableLayoutPanel tableLayout,Claster[] clasters)
        {
            
            tableLayout.Controls.Clear();
            tableLayout.ColumnStyles.Clear();
            tableLayout.RowStyles.Clear();

            int n = tableLayout.ColumnCount = tableLayout.RowCount = clasters[0].Points[0].Length;
            int k = clasters.Length;

            for (int i = 0; i < n; i++)
            {
                RowStyle rs = new RowStyle(SizeType.Percent, 100f / n);
                ColumnStyle cs = new ColumnStyle(SizeType.Percent, 100f / n);

                tableLayout.RowStyles.Add(rs);
                tableLayout.ColumnStyles.Add(cs);
            }

            tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;

            for (int i = 0; i < k; i++)
                clasters[i].PrepareForVisualization();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Chart chart = new Chart();
                    chart.Dock = DockStyle.Fill;


                    ChartArea chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    //chart.ChartAreas[0].AxisX.Minimum = stat_i.Min;
                    //chart.ChartAreas[0].AxisX.Maximum = stat_i.Max;
                    chart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.00}";
                    chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

                   
                    if (i != j)
                    {
                        for (int l = 0; l < k; l++)
                        {
                            Series s = new Series();

                            s.ChartType = SeriesChartType.Point;
                            s.Points.DataBindXY(clasters[l].Dimentions[i], clasters[l].Dimentions[j]);
                            
                            chart.Series.Add(s);
                        }
                    }

                    tableLayout.Controls.Add(chart, j, i);
                }
            }
        }

    }
}
