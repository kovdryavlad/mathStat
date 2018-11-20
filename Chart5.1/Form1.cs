using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using DialogWindowHelper;
using System.Threading;
using Chart5._1;
using Stat2;

namespace Chart1._1
{
    public partial class MyForm : Form
    {
        STAT Statistic;
        STAT2D TwoDimStat;
        STATND NDimStat;
        FactorsConnectionsTables fctables;
        FactorsConnectionsTablesNM fctablesNM;

        public int okrugl;

        //для многомерных
        public List<Viborka> viborki = new List<Viborka>();
        public List<Viborka> Selectedviborki = new List<Viborka>();
        public List<Viborka> SecondGroupOfSigns = new List<Viborka>(); //вторая половина признаков
        public List<GroupOfViborkas> KGroupsOfSigns= new List<GroupOfViborkas>();                   //к групп выборок

        Viborka CurrentViborka;

        public MyForm()
        {
            InitializeComponent();      //стандартный дизайнер

            Statistic = new STAT();

            Statistic.TestingKolmogorov += Statistic_TestingKolmogorov;

            ParallelCoordsChart.ChartAreas[0].AxisY.Maximum = 1;

            okrugl = 4;
        }

        void Statistic_TestingKolmogorov(object sender, KZKolmogorovEventArgs e)
        {
            MessageBox.Show(e.ResultOfTest+"\n"+e.Kz);
        }

        private void MyForm_Load(object sender, EventArgs e)
        {
            chart2.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chart2.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;

            chart1.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chart1.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;

            //округление по осям 
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0000}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0000}";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

            Statistic.Reproducting += Statistic_Reproducting;


            alphaforLaba2=0.05;
        }

        void Statistic_Reproducting(object sender, ReproductEventArgs e)
        {
            dataGridView2.Rows.Clear();

            tabcontrol.SelectedIndex = 1;   //выбираем вторую вкладку "Оценки параметров"

            for (int i = 0; i < e.NumberofParams; i++)
			{
                Param parametr = e.Params[i];

			    dataGridView2.Rows.Add(
                                        parametr.Name,
                                        Math.Round(parametr.Value, okrugl),
                                        Math.Round(parametr.Niz, okrugl),
                                        Math.Round(parametr.Verh, okrugl),
                                        Math.Round(parametr.Sigma, okrugl)
                                      );    
			}
            
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                double[] ddd = System.IO.File.ReadAllLines(filename)
                    .SelectMany(str => str.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(s =>
                    {
                        double a;
                        return double.TryParse(
                         s.Replace('.', ','),
                        out a) ? a : (double?)null;
                    })
                    .Where(val => val.HasValue)
                    .Select(val => val.Value)
                    .ToArray();
               
                string name = Path.GetFileName(filename);

                Statistic = new STAT();
                Statistic.Setd(ddd);

                AddViborka(name, Statistic);
                var v = viborki.Find(lvib => lvib.Name == name);

                OpenFunction(v);
            }
        }

        void Viborka_Click(object sender, EventArgs e)
        {
            Viborka n = viborki.First(x => x.Name == ((ToolStripMenuItem)sender).Text);

            CurrentViborka = n;
            
            Statistic = n.stat;
            Statistic.CommonPartOfSetd();
            //Statistic.CommonPartOfSetd();

            OpenFunction(n);
        }

        public void OpenFunction(Viborka v)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);
            
            DateTime start = DateTime.Now;

            logTextBox.Text = "";

            dataGridView2.Rows.Clear();

            Text = "MATH_STAT" + " - " + v.Name;

            //Statistic.Setd(v.data);

            Statistic.GetSeria1_Column(chart1.Series["Columns"]);
            //chart2.Series["StepLine_secondTime"].Color = Color.Violet;
            Statistic.GetSeria2_StepLine(chart2.Series["StepLine_secondTime"]);
            Statistic.GetSeria3_Points(chart2.Series["Points"]);

            Statistic.D_Calc_tochchnie_otsenki();
            Statistic.IntervalOcenki();

            ZapolnitDataGridView(dataGridView1);
            
            ChartDecoration();

            DateTime finish = DateTime.Now;
            logTextBox.Text += "Время: " + (finish - start).TotalMilliseconds.ToString();
        }
 
       private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UniversalModellingDistribution_Click(object sender, EventArgs e)
        {
            ModelWindow md = new ModelWindow(this, ((ToolStripMenuItem)sender).Text);

            md.ShowDialog();
        }
        
        void ZapolnitDataGridView(DataGridView d)
        {
            //Пусть будет сдесь
            StatusLabelNumberOfElements.Text = Statistic.d.Length.ToString();

            d.Rows.Clear();

            //имя - значение - низ - верх - сигма
            d.Rows.Add(
                       "Середнє арифметичне",
                       Math.Round(Statistic.Expectation, okrugl),
                       Math.Round(Statistic.Expectation_niz, okrugl),
                       Math.Round(Statistic.Expectation_verh, okrugl),
                       Math.Round(Statistic.Sigma_Expectation, okrugl)
                      );

            d.Rows.Add(
                       "Середнє кв. выдхилення",
                       Math.Round(Statistic.Sigma, okrugl),
                       Math.Round(Statistic.Sigma_niz, okrugl),
                       Math.Round(Statistic.Sigma_verh, okrugl),
                       Math.Round(Statistic.Sigma_Dispersia, okrugl)
                      );

            d.Rows.Add(
                       "Коеф. Асиметрії",
                       Math.Round(Statistic.A, okrugl),
                       Math.Round(Statistic.A_niz, okrugl),
                       Math.Round(Statistic.A_verh, okrugl),
                       Math.Round(Statistic.Sigma_A, okrugl)
                      );

            d.Rows.Add(
                       "Коеф. ексцесу",
                       Math.Round(Statistic.Excess, okrugl),
                       Math.Round(Statistic.Excess_niz, okrugl),
                       Math.Round(Statistic.Excess_verh, okrugl),
                       Math.Round(Statistic.Sigma_Excess, okrugl)
                      );

            d.Rows.Add(
                       "Коеф. Контрексцесу",
                        Math.Round(Statistic.KontrExcess, okrugl),
                        Math.Round(Statistic.KontrExcess_niz, okrugl),
                        Math.Round(Statistic.KontrExcess_verh, okrugl),
                        Math.Round(Statistic.Sigma_KontrExcess_zsun, okrugl)
                       );

            d.Rows.Add(
                       "Коеф. варіаціі Пірсона",
                       Math.Round(Statistic.W, okrugl),
                       Math.Round(Statistic.W_niz, okrugl),
                       Math.Round(Statistic.W_verh, okrugl),
                       Math.Round(Statistic.Sigma_W, okrugl)
                      );

            d.Rows.Add(
                       "MED",
                       Math.Round(Statistic.MED, okrugl)
                       );

            d.Rows.Add(
                       "MAD",
                       Math.Round(Statistic.MAD, okrugl)
                      );

            d.Rows.Add(
                       "Непараме. коеф. варіаціі Пірсона",
                       Math.Round(Statistic.NeparamW, okrugl)
                      );

            d.Rows.Add(
                       "Медіана Уолша",
                       Math.Round(Statistic.Uolsh, okrugl)
                      );


            d.Rows.Add(
                       "Усічене середнє",
                       Math.Round(Statistic.UsichSer, okrugl)
                      );
        }

        private void стандартизаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Statistic.Sdandartize();
            UpdateMainForm();
        }

        private void BackToInputData_Click(object sender, EventArgs e)
        {
            this.Statistic.RestoreInputData();
            UpdateMainForm();
        }

        private void прологарифмуватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPeretvorennya fp = new FormPeretvorennya(Statistic, this, 1);

            fp.Show();
        }

        public void UpdateMainForm()
        {
            this.Statistic.D_Calc_tochchnie_otsenki();
            this.Statistic.D_Calc_Sigmi_dlya_intervalnih_otsenok();


            Statistic.GetSeria1_Column(chart1.Series["Columns"]);
            Statistic.GetSeria2_StepLine(chart2.Series["StepLine_secondTime"]);
            Statistic.GetSeria3_Points(chart2.Series["Points"]);

            ChartDecoration();

            ZapolnitDataGridView(this.dataGridView1);
            ChartDecoration();
        }

        void ChartDecoration()
        {
            double decorator = 0.00;

            chart1.ChartAreas[0].AxisX.Interval = Math.Round(Statistic.h,okrugl);
            chart1.ChartAreas[0].AxisX.Minimum = Statistic.Min - decorator;
            chart1.ChartAreas[0].AxisX.Maximum = Statistic.Max + decorator;
            chart1.ChartAreas[0].AxisY.Maximum = Statistic.masYVid.Max();

            chart2.ChartAreas[0].AxisX.Interval = Statistic.h;
            chart2.ChartAreas[0].AxisX.Minimum = Statistic.Min - decorator;
            chart2.ChartAreas[0].AxisX.Maximum = Statistic.Max + decorator;
            chart2.ChartAreas[0].AxisY.Maximum = 1.004;
            chart2.ChartAreas[0].AxisY.Minimum = 0;

            
        }

        private void експоненціальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductExp(
                                   chart1.Series["Plotnost"],
                                   chart2.Series["Teoritichnaya"],
                                   chart2.Series["Niz"],
                                   chart2.Series["Verh"]
                                  );
        }

        private void ClearReproductRozpodil_Click(object sender, EventArgs e)
        {
            chart1.Series["Plotnost"].Points.Clear();

            chart2.Series["Teoritichnaya"].Points.Clear();

            chart2.Series["Niz"].Points.Clear();

            chart2.Series["Verh"].Points.Clear();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            //арксинус
            Statistic.ReproductArcSin(
                                      chart1.Series["Plotnost"],
                                      chart2.Series["Teoritichnaya"],
                                      chart2.Series["Niz"],
                                      chart2.Series["Verh"]
                                     );
        }

        private void нормальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductNormal(
                                      chart1.Series["Plotnost"],
                                      chart2.Series["Teoritichnaya"],
                                      chart2.Series["Niz"],
                                      chart2.Series["Verh"]
                                     );
        }

        private void лапласаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductLaplasa(
                                       chart1.Series["Plotnost"],
                                       chart2.Series["Teoritichnaya"],
                                       chart2.Series["Niz"],
                                       chart2.Series["Verh"]
                                      );
        }

        private void релеяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductReley(
                                     chart1.Series["Plotnost"],
                                     chart2.Series["Teoritichnaya"],
                                     chart2.Series["Niz"],
                                     chart2.Series["Verh"]
                                    );
        }

        private void колмогоровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Statistic.KZKolvogorov());
        }

        private void вилученняАномальнихДанихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAnomalii formAnomalii = new FormAnomalii(Statistic, this);

            formAnomalii.Show();
        }

        private void MyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
                открытьToolStripMenuItem_Click(this, EventArgs.Empty);
            else if (e.Control && e.KeyCode == Keys.D)
            {
                if (Statistic.d != null)
                    вилученняАномальнихДанихToolStripMenuItem_Click(this, EventArgs.Empty);
                else
                   MessageBox.Show("Вибірка пуста");
            }
            else if (e.Control && e.KeyCode == Keys.M)
                вікноМоделюванняToolStripMenuItem_Click(this, EventArgs.Empty);
            else if (e.Control && e.KeyCode == Keys.Q)
                выходToolStripMenuItem_Click(this, EventArgs.Empty);
            else if (e.Control && e.KeyCode == Keys.R)
                BackToInputData_Click(this, EventArgs.Empty);
        }

        private void вейбулаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductVeibula(
                                       chart1.Series["Plotnost"],
                                       chart2.Series["Teoritichnaya"],
                                       chart2.Series["Niz"],
                                       chart2.Series["Verh"]
                                      );
        }

        private void автоматичноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logTextBox.Text += Environment.NewLine;

            Statistic.TestingKolmogorov -= Statistic_TestingKolmogorov;

            Statistic.AutoReproduct(chart1.Series["Plotnost"],
                                    chart2.Series["Teoritichnaya"],
                                    chart2.Series["Niz"],
                                    chart2.Series["Verh"], 
                                    logTextBox
                                   );

            Statistic.TestingKolmogorov += Statistic_TestingKolmogorov;

            tabcontrol.SelectedIndex = 0;

            dataGridView2.Rows.Clear();
        }
        
        private void вікноМоделюванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelWindow modelWindow = new ModelWindow(this);

            modelWindow.ShowDialog();
        }

        private void звувToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPeretvorennya fp = new FormPeretvorennya(Statistic, this, 0);

            fp.Show();
        }

        private void піднесенняДоСтепеняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPeretvorennya fp = new FormPeretvorennya(Statistic, this, 2);

            fp.Show();
        }

        private void пірсонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistic.KZPirsona();

        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem zu = new ToolStripMenuItem();

            zu.Text = CurrentViborka.Name;

            zu.Click+= Viborka_Click;

            MenuViborki.DropDownItems.Remove(zu);

            viborki.Remove(CurrentViborka);
        }

        public Viborka viborka1;

        public Viborka viborka2;

        private void параметриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VyborViborok a = new VyborViborok(this);

            a.ShowDialog();
        }

        double alphaforLaba2;

        private void критерійБартлетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double S2 = 0;

            double chisl = 0;

            double znam = 0;

            for (int i = 0; i < viborki.Count; i++)
            {
                int Ni = viborki[i].data.Length;

                chisl += (Ni - 1) * viborki[i].stat.Dispersia;

                znam += (Ni - 1);
            }

            S2 = chisl / znam;

            string result = "Критерій Бартлетта\n";

            result += String.Format("S2 = {0}", Math.Round(S2, okrugl));

            double B = 0;

            double C = 0;

            double b = 0, c = 0;//промежуточные

            for (int i = 0; i < viborki.Count; i++)
            {
                int Ni = viborki[i].data.Length;

                B += (Ni - 1) * Math.Log(viborki[i].stat.Dispersia / S2);

                b += 1d / (Ni - 1);

                c += (Ni - 1);
            }

            B = -B;

            double skobki = b - 1d / c;

            C=skobki/(3*(viborki.Count-1))+1;

            result = String.Format("\nB = {0}", Math.Round(B, okrugl)); 

            result += String.Format("\nC = {0}", Math.Round(C, okrugl)); 

            double Hi = B/C;
              
            result += String.Format("\nHi = {0}", Math.Round(Hi, okrugl)); 

            double v = viborki.Count-1;

            result += String.Format("\nv = {0}", Math.Round(v, okrugl)); 

            double Hikv = Kvantili.Hi2(alphaforLaba2, v);

            result += String.Format("\nkv = {0}", Math.Round(Hikv, okrugl));
 
            if (Hi <= Hikv)
                result += "\nHi <= HiKv\nГоловна гіпотеза вірна. Sigma1 = ... = Sigman";
            else
                result += "\nf>fKv\nГоловна гіпотеза хибна.  Sigma1 != ... != Sigman";


            MessageBox.Show(result);
        }

        private void однофакторнийДисперсійнийАналізToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int k = viborki.Count;

            double N = 0;

            for (int i = 0; i < k; i++)
                N+=viborki[i].data.Length;

            string result = "Однофакторний дисперсійний аналіз\n";

            result += String.Format("\nN = {0}", Math.Round(N, okrugl));

            double XzahSer = 0;

            for (int i = 0; i < k; i++)
                XzahSer += viborki[i].data.Length * viborki[i].stat.Expectation;

            XzahSer /= N;

            result += String.Format("\nXzahSer = {0}", Math.Round(XzahSer, okrugl));

            double S2m = 0, S2v = 0;

            for (int i = 0; i < k; i++)
            {
                int N_i = viborki[i].data.Length;

                double Xsr_i = viborki[i].stat.Expectation;

                S2m += N_i* Math.Pow((Xsr_i- XzahSer),2);
            }

            S2m /= k - 1;

            result += String.Format("\nS2m = {0}", Math.Round(S2m, okrugl));

            for (int i = 0; i < k; i++)
                S2v += viborki[i].stat.Dispersia * (viborki[i].data.Length - 1);

            S2v /= N-k;

            result += String.Format("\nS2v = {0}", Math.Round(S2v, okrugl));

            double F = S2m / S2v;

            double v1 = k - 1;

            double v2 = N - k;

            double Fkv = Kvantili.Fishera(alphaforLaba2, v1, v2);

            result += String.Format("\nF = {0}", Math.Round(F, okrugl));

            result += String.Format("\nFkv = {0}", Math.Round(Fkv, okrugl));


            if (F < Fkv)
                result += "\nF<Fkv" + "\nГоловна гіпотеза вірна";
            else
                result += "\nF>Fkv" + "\nГоловна гіпотеза хибна";
            MessageBox.Show(result);
        }

        private void перевіркаЗбігуДисперсійToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            STAT d1 = new STAT();

            STAT d2 = new STAT();

            d1.Setd(viborka1.data);

            d2.Setd(viborka2.data);

            d1.CalcDisp();

            d2.CalcDisp();

            double S2x = d1.Sigma * d1.Sigma;

            double S2y = d2.Sigma * d2.Sigma;

            double f = 0;

            string result = "Перевірка збігу дисперсій\n";

            if (S2x >= S2y)
                f = S2x / S2y;
            else
                f = S2y / S2x;

            result += String.Format("f = {0}", Math.Round(f, 4));

            double fKv = Kvantili.Fishera(alphaforLaba2, viborka1.data.Length - 1, viborka2.data.Length - 2);

            result += String.Format("\nfkv = {0}", Math.Round(fKv, 4));

            if (f <= fKv)
                result += "\nf<=fKv\nГоловна гіпотеза вірна";
            else
                result += "\nf>fKv\nГоловна гіпотеза хибна";

            MessageBox.Show(result);
        }

        private void перевіркаЗбігуСередніхЗалежніToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int k = viborki.Count;

            int N1 = viborka1.data.Length;

            int N2 = viborka2.data.Length;

            if (N1 != N2)
            {
                MessageBox.Show("N1!=N2", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double[] z = new double[N1];

            for (int i = 0; i < N1; i++)
                z[i] = viborka1.data[i] - viborka2.data[i];

            STAT S = new STAT();

            S.Setd(z);

            S.CalcExpectation();

            S.CalcDisp();

            double t = S.Expectation * Math.Sqrt(N1) / S.Sigma;

            double tkv = Kvantili.Student(alphaforLaba2/2, N1 - 2);

            string result = "Перевірка збігу середніх. (Залежний вападок)\n";

            result += String.Format("\nt = {0}", Math.Round(t, okrugl));

            result += String.Format("\ntkv = {0}", Math.Round(tkv, okrugl));

            if (Math.Abs(t) > tkv)
                result += "\n|t|>tkv\nГоловну гіпотезу слід відхилити";
            else
                result += "\n|t|<tkv\nГоловну гіпотезу слід Прийняти";

            MessageBox.Show(result);
        }

        private void перевіркаЗбігуСередніхНезалToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int k = viborki.Count;

            int N1 = viborka1.data.Length;

            int N2 = viborka2.data.Length;

            int v = N1+N2-2;

            string result = "Перевірка збігу середніх(Незалежний випадок)\n";

            double t=0; 

            double S2x = viborka1.Dispersion/N1;

            double S2y = viborka2.Dispersion/N2;

            double Sr = (viborka1.expectation - viborka2.expectation) ;
            if (N1 + N2 > 25)
            {
                t = Sr/ Math.Sqrt(S2x + S2y);
            }
            else
            { 
                double chisl = (N1-1)*S2x+(N2-1)*S2y;

                double znamenatel =N1+N2-2;
                
                double Disp = chisl/znamenatel;

                t = Sr/Math.Sqrt(Disp)*Math.Sqrt((double)(N1*N2)/(N1+N2));
            }
                        
            result += String.Format("\nt = {0}", Math.Round(t,okrugl)); 

            double tkv = Kvantili.Student(alphaforLaba2/2, v);

            result += String.Format("\nkv = {0}", Math.Round(tkv,okrugl));
 
            if (Math.Abs(t) > tkv)
                result += "\n|t|>tkv\nСередні НЕ збігаються";
            else
                result += "\n|t|<tkv\nСередні збігаються";

            MessageBox.Show(result);
        }

        private void критерійВілкоксонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int N1 = viborka1.data.Length;

            int N2 = viborka2.data.Length;

            Element[] array = new Element[N1 + N2];

            for (int i = 0; i < N1; i++)
            {
                array[i] = new Element();

                array[i].data = viborka1.data[i];

                array[i].numberOfSample = 1;
            }

            for (int i = 0; i < N2; i++)
            {
                array[i + N1] = new Element();

                array[i + N1].data = viborka2.data[i];

                array[i + N1].numberOfSample = 2;
            }

            Array.Sort(array);

            for (int i = 0; i < array.Length; i++)
            {
                array[i].rang = i + 1;
            }

            int number = 0;
            int startindex = 0;
            int finalindex = 0;

            for (int i = 0; i < array.Length; i++)
            {
                number = i;

                int counter = 1;

                startindex = i;

                double sum = array[number].data;

                double sumrangs = array[number].rang;

                while ((number + 1) != array.Length && array[number].data == array[number + 1].data)
                {
                    sum += array[number + 1].data;

                    sumrangs += array[number + 1].rang;

                    counter++;

                    number++;
                }

                if (counter > 1)
                {
                    finalindex = startindex + counter;

                    double rang = sumrangs / (finalindex - startindex);

                    for (int j = startindex; j < startindex + counter; j++)
                    {
                        array[j].rang = rang;
                    }

                    i = startindex + counter - 1;
                }
            }
            //конец ф-и

            int N = array.Length;

            var sumran1 = array.Where(x => x.numberOfSample == 1).Select(x => x.rang).Sum();

            double E_W = N1 * (N + 1) / 2d;

            double D_W = N1 * N2 * (N + 1) / 12d;

            double w = (sumran1 - E_W) / Math.Sqrt(D_W);

            string result = "Критерій Вілкоксона\n";

            result += String.Format("\nW = {0}", Math.Round(sumran1, okrugl));

            result += String.Format("\nE_W = {0}", Math.Round(E_W, okrugl));

            result += String.Format("\nS_W = {0}", Math.Round(Math.Sqrt(D_W), okrugl));

            double kv = Kvantili.Normal(alphaforLaba2 / 2);

            result += String.Format("\nkv = {0}", Math.Round(kv, okrugl));

            result += String.Format("\nw = {0}", Math.Round(w, okrugl));

            if (Math.Abs(w)<=kv)
                result += "\n|w|<=kv\nГоловну гіпотезу слід прийняти";
            else
                result += "\n|w|>kv\nГоловну гіпотезу слід відхилити";

            MessageBox.Show(result);
        }

        private void критерійЗнаківToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int N1 = viborka1.data.Length;

            int N2 = viborka2.data.Length;

            if (N1!=N2)
            {
                MessageBox.Show("\nПомилка\nN1!=N2");
                return;
            }

            int N = N1;

            double[] z = new double[N];

            double[] U = new double[N];

            for (int i = 0; i < N; i++)
            {
                z[i] = viborka1.data[i] - viborka2.data[i];

                if (z[i] > 0)
                    U[i] = 1;
                else
                    U[i] = 0;
            }

            string result = "Критерій знаків: \n";
            
            double S = U.Sum();

            result += String.Format("S = {0}", S);

          
            if (N<=15)
            {
                double sum = 0;

                for (int i = 0; i < N-S; i++)
			    {
			        sum+=(double)factorial(i)/(factorial(N)*factorial(N-i));
			    }

                double alpha0 = Math.Pow(2,-N)*sum;

                if (alpha0<alphaforLaba2)
	            {
                    result+="\nalpha0 = " +alpha0;
                    result+="\nalpha = " +alphaforLaba2;
                    result+="\nalpha0 < alpha\nГоловну гіпотезу слід відхилити";
                    goto A;
	            }
            }
            else
            {
                double S_zvezd = (2 * S - 1 - N) / Math.Sqrt(N);

                double kv = Kvantili.Normal(alphaforLaba2 / 2);

                result += "\nS* = " + S_zvezd;
                result += "\nkv = " + kv;

                if (S_zvezd<kv)
                    result += "\nS* < kv\nГоловну гіпотезу слід прийняти";
                else 
                    result+="\nS* > kv\nГоловну гіпотезу слід відхилити";
                goto A;
            }

            A:;
            MessageBox.Show(result);
        }

        int factorial(int n)
        {
            int fact = 1;

            for (int i = 1; i <= n; i++)
            {
                fact *= i;
            }

            return fact;
        }

        private void видалитиДійснуВибіркуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n =viborki.IndexOf(CurrentViborka, 0);

            MenuViborki.DropDownItems.RemoveAt(n);

            viborki.Remove(CurrentViborka);

            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
            }

            for (int i = 0; i < chart2.Series.Count-1; i++)
            {
                chart2.Series[i].Points.Clear();
            }

            Text = "MATH_STAT";

            StatusLabelNumberOfElements.Text = "0";
        }

        private void зчитатиДвовимірнуВибіркуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string filename="";
            if (DialogWindows.GetOpenFileName(out filename, "Txt файлы|*.txt|Все файлы|*.*"))
            {
                очиститиToolStripMenuItem_Click(this, EventArgs.Empty);
           
                DimentionalTabControl.SelectedIndex = 1;
                TwoDimStat = new STAT2D(filename);

                //настроили вид чартов
                TwoDimChartDesign();

                var context = SynchronizationContext.Current;

                TwoDimStat.GetTwoDimGist(chart3.Series);
                TwoDimStat.GetKorrelationField(chart4.Series[0]);

                //построение шкалы
                BuildScale();
                //вывод параметров
                OutputParamsonGrid();
                //вывод кол-ва элементов
                this.StatusLabelNumberOfElements.Text = TwoDimStat.N.ToString();
                //альфа в текстбокс
                AlphaTextBox.Text = TwoDimStat.alpha.ToString();

                //сохраняем выборки

                filename = Path.GetFileName(filename);

                AddViborka(filename + "#0", TwoDimStat._x);
                AddViborka(filename + "#1", TwoDimStat._y);
            }
            else
                return;
        }

        void AddViborka(string name, STAT stat)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = name;
            menuItem.Click += Viborka_Click;

            MenuViborki.DropDownItems.AddRange(new[] { menuItem });
            viborki.Add(new Viborka(stat, name));

            TwoVibCombobox1.Items.Add(name);
            TwoVibCombobox2.Items.Add(name);
        }

        //выводим шкалу
        private void BuildScale()
        {
            var panel = ColorScaleLayoutPanel;
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();
            panel.Controls.Clear();

            //panel.ColumnCount = 2;
            int ColorN = TwoDimStat._NumberOfCOlors;
            double ColorH = TwoDimStat.ColorH;
            panel.RowCount = ColorN;

            for (int i = 0; i < ColorN; i++)
            {
                //RowStyle rs = new RowStyle(SizeType.Percent, (float)1d / ColorN);
               
                Panel p = new Panel();
                p.Width = 10;
                p.Height = 10;
                int v = i * 255 / ColorN;
                p.BackColor = Color.FromArgb(255, 255 - v, 255 - v, 255 - v);
                panel.Controls.Add(p, 0, i);

                Label l = new Label();
                String s = String.Format("{0:0.0000}-{1:0.0000}", Math.Round(i * ColorH, 4), Math.Round((i + 1) * ColorH, 4));
                l.Text = s;
                panel.Controls.Add(l, 1, i);
            }

        }

        private void OutputParamsonGrid()
        {
            Func<double,double> r = num =>Math.Round(num, 4);
            //очистить старое
            Stat2dDataGrid.Rows.Clear();

            Stat2dDataGrid.Rows.Add("Коеф. лінійної кореляції", r(TwoDimStat.GetKoefLinearCorrelation()), 
                                    r(TwoDimStat._koefLinearCorrelation_niz), r(TwoDimStat._koefLinearCorrelation_verh));
            TwoDimStat.GetKoefCorrelationRatio();
            //Stat2dDataGrid.Rows.Add("Кореляційне відношення", r(TwoDimStat.GetKoefCorrelationRatio()));
            //r(TwoDimStat._koefCorrelationRatio_niz), r(TwoDimStat._koefCorrelationRatio_verh));

            TwoDimStat.CalcKoefSpirmenaAndKendella();
            Stat2dDataGrid.Rows.Add("Коефіцієнт Спірмена", r(TwoDimStat._koefSpirmena), r(TwoDimStat._koefSpirmena_niz), r(TwoDimStat._koefSpirmena_verh));
            Stat2dDataGrid.Rows.Add("Коефіцієнт Кендалла", r(TwoDimStat._koefKendella),r(TwoDimStat._koefKendella_niz),r(TwoDimStat._koefKendella_verh));

            //коэф сообшений таблиц 2*2
            fctables = TwoDimStat.TableConnection2on2;
            Stat2dDataGrid.Rows.Add("Індекс Фехнера", r(fctables.Index_Fehnera()));
            Stat2dDataGrid.Rows.Add("Коефіцієнт сполучень Ф", r(fctables.Coefficient_Splitting_Fi()));
            Stat2dDataGrid.Rows.Add("Коефіцієнт Юла Q", r(fctables.Coefficient_Link_of_Yula_Q()));
            Stat2dDataGrid.Rows.Add("Коефіцієнт Юла Y", r(fctables.Coefficient_Link_of_Yula_Y()));

            //N*M
            //fctablesNM = TwoDimStat.TableConnectionNonM;    //перестало работать после МГК
            //Stat2dDataGrid.Rows.Add("Таблиця сполучень N * M");
            //Stat2dDataGrid.Rows.Add("Коефіцієнт сполучень Пірсона", r(fctablesNM.СoefficientConnectionsPirsona()));           
            ////Stat2dDataGrid.Rows.Add("Міра зв’язку Кендалла", r(fctablesNM.MeasureOfConnectionKendella()));
            //Stat2dDataGrid.Rows.Add("Статистика Стюарта", r(fctablesNM.StatStuarta()));

        }

        private void TwoDimChartDesign()
        {

            chart3.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chart3.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            //chart3.ChartAreas[0].AxisX.Minimum = TwoDimStat._x.Min;
            //chart3.ChartAreas[0].AxisX.Maximum = TwoDimStat._x.Max;
            //chart3.ChartAreas[0].AxisY.Minimum = TwoDimStat._y.Min;
            //chart3.ChartAreas[0].AxisY.Maximum = TwoDimStat._y.Max;
            chart3.ChartAreas[0].AxisX.Interval = TwoDimStat._x.h;
            chart3.ChartAreas[0].AxisY.Interval = TwoDimStat._y.h;
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0000}";
            chart3.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

            chart4.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chart4.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chart4.ChartAreas[0].AxisX.Minimum = TwoDimStat._x.Min;
            chart4.ChartAreas[0].AxisX.Maximum = TwoDimStat._x.Max;
            chart4.ChartAreas[0].AxisY.Minimum = TwoDimStat._y.Min;
            chart4.ChartAreas[0].AxisY.Maximum = TwoDimStat._y.Max;
            chart4.ChartAreas[0].AxisX.Interval = TwoDimStat._x.h;
            chart4.ChartAreas[0].AxisY.Interval = TwoDimStat._y.h;
            chart4.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0000}";
            chart4.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

            //chart4.ChartAreas[0].BackColor = Color.Cyan;

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void хіквадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwoDimStat.KrytteriyHi2();
        }

        private void перевіркаЗначущостіЛінійногоКоефіцієнтаКореляціїToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwoDimStat.CheckForSignificance_LinearKoef();
        }

        private void перевіркаЗначущостіКореляційногоВідношенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwoDimStat.CheckForSignificance_KorrelationRatio();
        }

        private void перевіркаЗначущостіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = fctables.Coefficient_Splitting_Fi_significance();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначущостіКоефіцієнтівЗвязкуЮлаQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = fctables.Coefficient_Link_of_Yula_Q_significance();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначущостіКоефіцієнтівЗвязкуЮлаYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = fctables.Coefficient_Link_of_Yula_Y_significance();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначущостіКоефіцієнтаСполученьПірсонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = fctablesNM.СoefficientConnectionsPirsona_significance();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначущостіМіриЗвToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string mes = fctablesNM.MeasureOfConnectionKendella_significance();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначущостіСтатистикиСтюартаToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string mes = fctablesNM.CheckSignificanceOfStuart();
            MessageBox.Show(mes);
        }

        private void рівномірнийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClearReproductRozpodil_Click(this, EventArgs.Empty);

            Statistic.ReproductRavn(
                                       chart1.Series["Plotnost"],
                                       chart2.Series["Teoritichnaya"],
                                       chart2.Series["Niz"],
                                       chart2.Series["Verh"]
                                      );
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var zu = Convert.ToDouble(AlphaTextBox.Text.Replace(".",",")) ;

            Func<double, double> normilize = (x) =>
            {
                if (x < 0)
                    return 0;
                else if (x > 1)
                    return 1;
                else
                    return x;
            };

            TwoDimStat.alpha = normilize(zu);

            OutputParamsonGrid();
        }

        private void перевыркаНезалежностіХТаУToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = fctablesNM.IndependenceHypothesis();
            MessageBox.Show(mes);
        }

        private void перевіркаЗначенняДисперсіїКРитеріійБартлеттаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mes = TwoDimStat.GetKriteriaBartlet();
            MessageBox.Show(mes);
        }

        double round(double value)
        {
            return Math.Round(value, 4);
        }

        private void очиститиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart4.Series["Линія регресії"].Points.Clear();
            TwoDimJournal.Text = "";
            chart4.Series["ВТМ"].Points.Clear();
            chart4.Series["НТМ"].Points.Clear();

            chart4.Series["ВДІНС"].Points.Clear();
            chart4.Series["НДІНС"].Points.Clear();

            chart4.Series["ВІЛР"].Points.Clear();
            chart4.Series["НІЛР"].Points.Clear();

        }

        void Mess2Journal(string s)
        {
            TwoDimJournal.Text += s + Environment.NewLine;
        }



        private void відтворитиЛінійнуРегресіюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            очиститиToolStripMenuItem_Click(this, EventArgs.Empty);
            var points = chart4.Series["Линія регресії"].Points;

            points.Clear();

            var abTuple = TwoDimStat.GetABForLinearRegressionOLS();
            Mess2Journal("Відтворення лінійної регресії за МНК");
            Mess2Journal("a = " + round(abTuple.Item1));
            Mess2Journal("b = " + round(abTuple.Item2));


            double min = TwoDimStat._x.Min;
            double max = TwoDimStat._x.Max;

            points.AddXY(min, abTuple.Item1 + min * abTuple.Item2);
            points.AddXY(max, abTuple.Item1 + max * abTuple.Item2);

            ShowAbIntervals();

            OutputCoefOfDetermination();
        }

        private void відтворитиЛінійнуРегресіюМетодТейлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            очиститиToolStripMenuItem_Click(this, EventArgs.Empty);
            var points = chart4.Series["Линія регресії"].Points;

            points.Clear();

            var abTuple = TwoDimStat.GetABForLinearRegressionTeil();
            Mess2Journal("Відтворення лінійної регресії за методом Тейла");
            Mess2Journal("a = " + round(abTuple.Item1));
            Mess2Journal("b = " + round(abTuple.Item2));


            double min = TwoDimStat._x.Min;
            double max = TwoDimStat._x.Max;

            points.AddXY(min, abTuple.Item1 + min * abTuple.Item2);
            points.AddXY(max, abTuple.Item1 + max * abTuple.Item2);

            ShowAbIntervals();

            OutputCoefOfDetermination();
        }

        private void OutputCoefOfDetermination()
        {
            Mess2Journal("Коефіцієнт детермінації: " + round(TwoDimStat.GetCoefDetermination())+"%");
        }

        void ShowAbIntervals() 
        {
            //(сообщение для месседж бокса - а нижнее - а верхнее - b нижнее - b верхнее)
            var resultTuble = TwoDimStat.GetIntervalsToAB();


            Mess2Journal("Інтервальні оцінки параметрів");
            Mess2Journal("a є [" + round(resultTuble.Item1) + " , " + round(resultTuble.Item2) + "]");
            Mess2Journal("b є [" + round(resultTuble.Item3) + " , " + round(resultTuble.Item4) + "]");
        }

        private void толерантніМежіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //(x[] одинаковые для нижнего и верхнего пределов, y[] для нижней границы,y[] для верхней границы)
            var resultTuple = TwoDimStat.GetToleranceLimits();

            //чистим на всякий случай то, что там было
            chart4.Series["ВТМ"].Points.Clear();
            chart4.Series["НТМ"].Points.Clear();

            chart4.Series["НТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item2);         
            chart4.Series["ВТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item3);
        }

        private void довірчіІнтервалиНовихСпостереженьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart4.Series["ВДІНС"].Points.Clear();
            chart4.Series["НДІНС"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetConfidenceIntervalsForNewObservations();
            chart4.Series["НДІНС"].Points.DataBindXY(ResultTuple.Item1,ResultTuple.Item2);
            chart4.Series["ВДІНС"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        }

        private void довірчіІнтервалиЛініїРегресіїToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart4.Series["ВІЛР"].Points.Clear();
            chart4.Series["НІЛР"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetConfidenceIntervalsForRegressionLine();
            chart4.Series["НІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item2);
            chart4.Series["ВІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        }

        private void перевіркаЗначущостіПараметрівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mes = TwoDimStat.CheckSugnificanceOfAB();
            MessageBox.Show(mes);
        }

        private void провестиПеревіркуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double a = 0, b = 0;
            try
            {
                a = Convert.ToDouble(AForLinearTextBox.Text.Replace('.', ','));
                b = Convert.ToDouble(BForLinearTextBox.Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show("Помилка зчитування а та/або b");

                return;
            }

            var mes = TwoDimStat.CheckSugnificanceOfParametrsLinearRegression(a, b);

            MessageBox.Show(mes);
        }

        private void gthtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mes = TwoDimStat.ValidationOfAdequacyRegression();

            MessageBox.Show(mes);
        }

        private void відтворитиПараболічнуРегресіюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            очиститиToolStripMenuItem_Click(this, EventArgs.Empty);
            var points = chart4.Series["Линія регресії"].Points;

            points.Clear();


            var abcTuple = TwoDimStat.CalcParabolicRegresParametrsABC();
            Mess2Journal("Відтворення параболічної регресії:");
            Mess2Journal("a = " + round(abcTuple.Item1));
            Mess2Journal("b = " + round(abcTuple.Item2));
            Mess2Journal("c = " + round(abcTuple.Item3));

            var ParabolicLineTuple = TwoDimStat.GetParabolicRegressionLine();

            //строим саму линию
            points.DataBindXY(ParabolicLineTuple.Item1, ParabolicLineTuple.Item2);

            var intervalsABC = TwoDimStat.GetIntervalsForABC();

            Mess2Journal("Інтервальне оцінювання параметрів");
            Mess2Journal(round(intervalsABC.Item1) + "<= a <= " + round(intervalsABC.Item2));
            Mess2Journal(round(intervalsABC.Item3) + "<= b <= " + round(intervalsABC.Item4));
            Mess2Journal(round(intervalsABC.Item5) + "<= c <= " + round(intervalsABC.Item6));

            var Determ = TwoDimStat.GetParabolicDetermianation();
            Mess2Journal("Коефіцієнт детермінації:" + round(Determ));
            
        }

        private void параболічнаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void перевіокаЗначущостіПараметрівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mess = TwoDimStat.ChekSignificanseABC();
            MessageBox.Show(mess);
        }

        private void провестиПеревіркуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            double a = 0;
            try
            {
                a = Convert.ToDouble(PaTextBox.Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show("Помилка зчитування а");

                return;
            }

            var mes = TwoDimStat.CheckEqualityParabolicParametrA(a);

            MessageBox.Show(mes);
        }

        private void провестиПеревіркуToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            double b = 0;
            try
            {
                b = Convert.ToDouble(PbTextBox.Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show("Помилка зчитування а");

                return;
            }

            var mes = TwoDimStat.CheckEqualityParabolicParametrB(b);

            MessageBox.Show(mes);
        }

        private void провестиПеревіркуToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            double c = 0;
            try
            {
                c = Convert.ToDouble(PcTextBox.Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show("Помилка зчитування а");

                return;
            }

            var mes = TwoDimStat.CheckEqualityParabolicParametrC(c);

            MessageBox.Show(mes);
        }

        private void толерантніМежіToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //(x[] одинаковые для нижнего и верхнего пределов, y[] для нижней границы,y[] для верхней границы)
            var resultTuple = TwoDimStat.GetTolerantParablocLimits();

            //чистим на всякий случай то, что там было
            chart4.Series["ВТМ"].Points.Clear();
            chart4.Series["НТМ"].Points.Clear();

            chart4.Series["НТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item2);
            chart4.Series["ВТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item3);
        }

        private void лінійToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void довірчіІнтервалиЛініїРегресіїToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            chart4.Series["ВІЛР"].Points.Clear();
            chart4.Series["НІЛР"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetParabolicConfidenceIntervals();
            chart4.Series["НІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item2);
            chart4.Series["ВІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        }

        private void довірчіІнтервалиНовихСпостереженьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            chart4.Series["ВДІНС"].Points.Clear();
            chart4.Series["НДІНС"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetParabolicIntervalsForNewObservations();
            chart4.Series["НДІНС"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item2);
            chart4.Series["ВДІНС"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        
        }

        private void перевыркаАдекватностіВідтвореноїРегресіїToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mess = TwoDimStat.ParabolicValidationOfAdequacyRegression();

            MessageBox.Show(mess);
        }

        private void відтворитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            очиститиToolStripMenuItem_Click(this, EventArgs.Empty);

            var t = TwoDimStat.GetKvazRegressionParams();

            Mess2Journal("Відтворення квазілінійної регресії");
            Mess2Journal("a = " + round(t.Item1));
            Mess2Journal("b = " + round(t.Item2));

            var intTpl = TwoDimStat.GetIntervalsForABKvaz();

            Mess2Journal("Інтервальні оцінки параметрів");
            Mess2Journal("a є [" + round(intTpl.Item1) + " , " + round(intTpl.Item2) + "]");
            Mess2Journal("b є [" + round(intTpl.Item3) + " , " + round(intTpl.Item4) + "]");


            //нужно выполнить сдвиг по иксу, чтобы не было отрицательных значений
            TwoDimStat.ChangeCorrelationField(chart4.Series[0]);
            TwoDimChartDesign();
            
            var context = SynchronizationContext.Current;
            TwoDimStat.GetTwoDimGist(chart3.Series);


            var RegresT = TwoDimStat.GetKvazRegressionLine();
            chart4.Series["Линія регресії"].Points.DataBindXY(RegresT.Item1, RegresT.Item2);
            

        }

        private void довірчіІнтервалиЛініїРегресіїToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            chart4.Series["ВІЛР"].Points.Clear();
            chart4.Series["НІЛР"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetConfidenceIntervalsForRegressionLineKVAZ();
            chart4.Series["НІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item2);
            chart4.Series["ВІЛР"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        
        }

        private void довірчіІнтервалиНовихПередбаченьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart4.Series["ВДІНС"].Points.Clear();
            chart4.Series["НДІНС"].Points.Clear();

            //(x, y_niz, y_verh)
            var ResultTuple = TwoDimStat.GetConfidenceIntervalsForNewObservationsKVAZ();
            chart4.Series["НДІНС"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item2);
            chart4.Series["ВДІНС"].Points.DataBindXY(ResultTuple.Item1, ResultTuple.Item3);
        
        }

        private void толерантніМежіToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //(x[] одинаковые для нижнего и верхнего пределов, y[] для нижней границы,y[] для верхней границы)
            var resultTuple = TwoDimStat.GetToleranceLimitsKVAZ();

            //чистим на всякий случай то, что там было
            chart4.Series["ВТМ"].Points.Clear();
            chart4.Series["НТМ"].Points.Clear();

            chart4.Series["НТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item2);
            chart4.Series["ВТМ"].Points.DataBindXY(resultTuple.Item1, resultTuple.Item3);
        
        }

        
        private void моделюванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelTwoDimRegressionWindow w = new ModelTwoDimRegressionWindow();
            w.ModelEvent += w_ModelEvent;
            w.ShowDialog();

        }

        void w_ModelEvent(object sender, TwoDimRegressionModerService e)
        {
            TwoDimModel cl = new TwoDimModel();
            var fileName = e.FilePath;

            //cl.ModelKvaziLinear(20, 50, 500, 2, 4, 0.6, fileName);
            cl.ModelKvaziLinear(e.xMin, e.xMax, e.N, e.a, e.b, e.SigmaEpsilon, fileName);

            try
            {
                очиститиToolStripMenuItem_Click(this, EventArgs.Empty);

                DimentionalTabControl.SelectedIndex = 1;
                TwoDimStat = new STAT2D(fileName);

                //настроили вид чартов
                TwoDimChartDesign();

                var context = SynchronizationContext.Current;

                TwoDimStat.GetTwoDimGist(chart3.Series);
                TwoDimStat.GetKorrelationField(chart4.Series[0]);

                //построение шкалы
                BuildScale();
                //вывод параметров
                OutputParamsonGrid();
                //вывод кол-ва элементов
                this.StatusLabelNumberOfElements.Text = TwoDimStat.N.ToString();
                //альфа в текстбокс
                AlphaTextBox.Text = TwoDimStat.alpha.ToString();

                //сохраняем выборки
                ToolStripMenuItem zu1 = new ToolStripMenuItem();
                ToolStripMenuItem zu2 = new ToolStripMenuItem();

                zu1.Text = fileName + " #0";
                zu1.Click += Viborka_Click;
                MenuViborki.DropDownItems.AddRange(new[] { zu1 });

                zu2.Text = fileName + " #1";
                zu2.Click += Viborka_Click;

                MenuViborki.DropDownItems.AddRange(new[] { zu2 });

                viborki.Add(new Viborka((double[])TwoDimStat._x.d.Clone(), fileName + " #0"));
                viborki.Add(new Viborka((double[])TwoDimStat._y.d.Clone(), fileName + " #1"));
            }
            catch { w_ModelEvent(this, e); }
        
        }

        private void моделюванняToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ModelTwoDimRegressionWindowParabol w = new ModelTwoDimRegressionWindowParabol();
            w.ModelEvent +=w_ModelEventParab;
            w.ShowDialog();
        }
        void w_ModelEventParab(object sender, TwoDimRegressionModerParabService e)
        {
            TwoDimModel cl = new TwoDimModel();
            var fileName = e.FilePath;

            //cl.ModelKvaziLinear(20, 50, 500, 2, 4, 0.6, fileName);
            cl.ModelParabolic(e.xMin, e.xMax, e.N, e.a, e.b, e.c, e.SigmaEpsilon, fileName);

            try
            {
                очиститиToolStripMenuItem_Click(this, EventArgs.Empty);

                DimentionalTabControl.SelectedIndex = 1;
                TwoDimStat = new STAT2D(fileName);

                //настроили вид чартов
                TwoDimChartDesign();

                var context = SynchronizationContext.Current;

                TwoDimStat.GetTwoDimGist(chart3.Series);
                TwoDimStat.GetKorrelationField(chart4.Series[0]);

                //построение шкалы
                BuildScale();
                //вывод параметров
                OutputParamsonGrid();
                //вывод кол-ва элементов
                this.StatusLabelNumberOfElements.Text = TwoDimStat.N.ToString();
                //альфа в текстбокс
                AlphaTextBox.Text = TwoDimStat.alpha.ToString();

                //сохраняем выборки
                ToolStripMenuItem zu1 = new ToolStripMenuItem();
                ToolStripMenuItem zu2 = new ToolStripMenuItem();

                zu1.Text = fileName + " #0";
                zu1.Click += Viborka_Click;
                MenuViborki.DropDownItems.AddRange(new[] { zu1 });

                zu2.Text = fileName + " #1";
                zu2.Click += Viborka_Click;

                MenuViborki.DropDownItems.AddRange(new[] { zu2 });

                viborki.Add(new Viborka((double[])TwoDimStat._x.d.Clone(), fileName + " #0"));
                viborki.Add(new Viborka((double[])TwoDimStat._y.d.Clone(), fileName + " #1"));
            }
            catch { w_ModelEventParab(this, e); }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

            очиститиToolStripMenuItem_Click(this, EventArgs.Empty);

            DimentionalTabControl.SelectedIndex = 1;


            double[] x = viborki.First(v => v.Name == TwoVibCombobox1.Items[TwoVibCombobox1.SelectedIndex].ToString()).stat.d;
            double[] y = viborki.First(v => v.Name == TwoVibCombobox2.Items[TwoVibCombobox2.SelectedIndex].ToString()).stat.d;


            TwoDimStat = new STAT2D(x,y);

            //настроили вид чартов
            TwoDimChartDesign();

            var context = SynchronizationContext.Current;

            TwoDimStat.GetTwoDimGist(chart3.Series);
            TwoDimStat.GetKorrelationField(chart4.Series[0]);

            //построение шкалы
            BuildScale();
            //вывод параметров
            OutputParamsonGrid();
            //вывод кол-ва элементов
            this.StatusLabelNumberOfElements.Text = TwoDimStat.N.ToString();
            //альфа в текстбокс
            AlphaTextBox.Text = TwoDimStat.alpha.ToString();

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void зчитатиЬагатовимірнуВибіркуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Все файлы|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    var r = FileReader.ReadFile(ofd.FileName);

                    var r2 = Stat2.DoubleConverter.ConvertToDoubleValuesInColumns(r);
                    r2 = Stat2.DoubleConverter.RegroupBySamples(r2);

                    for (int i = 0; i < r2.Length; i++)
                    {
                        STAT S = new STAT();
                        S.Setd((double[])r2[i].Clone());

                        AddViborka(Path.GetFileName(ofd.FileName) + "#" + i, S);
                        
                    }


                    WorkWithNDimSTAT(true);
                }
                catch(Exception ex)
                {
                    var m = ex.Message;
                    MessageBox.Show("Помилка читання файлу");
                }

            }
            
        }

        private void WorkWithNDimSTAT(bool workwithall)
        {
            DimentionalTabControl.SelectedIndex= 2;
            tabControl2.SelectedIndex = 1;

            SampleSelectingForm form = new SampleSelectingForm(viborki, Selectedviborki, workwithall);
            form.ShowDialog();

            var neededSamples = Selectedviborki.Select(sv => sv.stat).ToArray();
            if (neededSamples.Length == 0)
            {
                MessageBox.Show("Не вибрано вибірки для багатовимірного аналізу");

                NdimLogTextBox.Text = "";
                
                return;
            }

            NDimStat = new STATND(neededSamples);

            //матрица диаграмм разбросса
            NDimStat.GetMatrixOfScatterDiagrams(MatrixOfScatterDiagramsTableLayout);

            FillNDimLoggers();
        }

        private void менеджерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkWithNDimSTAT(false);
        }

        void Add2Nlog(object element)
        {
            NdimLogTextBox.Text += element.ToString() + Environment.NewLine;
        }

        private void FillNDimLoggers()
        {
            NdimLogTextBox.Text = "";
            VariationalServiceTextBox.Text = NDimStat.GetAllVariationalSeries();


            //вывод средних
            Add2Nlog("Оцінки середніх:");
            Add2Nlog(NDimStat.GetExpectationsString());
            
            //вывод средне квадратических
            Add2Nlog("Оцінки середньоквадратичних:");
            Add2Nlog(NDimStat.GetSigmasString());

            //дисперсионно-корвариационная
            var DC = NDimStat.GetDC();
            Add2Nlog("DC:");
            Add2Nlog(DC);

            //ковариационная матрица
            var R = NDimStat.GetR();
            Add2Nlog("R:");
            Add2Nlog(R);

            //частичне коэффициенты корреляции
            DateTime t1 = DateTime.Now;
            var PartialR = NDimStat.GetAllPartialCoefsCorrelation();
            DateTime t2 = DateTime.Now;
            Add2Nlog("PartialR:");
            Add2Nlog(PartialR);
            //Add2Nlog("Время подсчета частичных коеф: "+ (t2-t1).TotalMilliseconds);

            //множественные коэффициенты корреляции
            DateTime t3 = DateTime.Now;
            var MultipleR = NDimStat.GetMultipleCoefsCorrelation();
            DateTime t4 = DateTime.Now;
            Add2Nlog("MultipleR:");
            Add2Nlog(MultipleR);
            //Add2Nlog("Время подсчета множественных коэфф. корреляции: " + (t4 - t3).TotalMilliseconds);

            Add2Nlog("");
            Add2Nlog("Знаущість множинних коефіцієнтів кореляції");
            Add2Nlog(NDimStat.SignificanceOfALLMultipleR());

            //if (NDimStat.N == 3)
            //Add2Nlog(NdimRegression.FindParamsForThreeDim(NDimStat.GetStat(0), NDimStat.GetStat(1), NDimStat.GetStat(2)));


            //Add2Nlog(NdimRegression.FindParamsForNDim(NDimStat, new[] { 0, 1 }, 2, 0.05));
        }

        private void iГрупаОзнакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SampleSelectingForm form = new SampleSelectingForm(viborki, Selectedviborki, false);
            form.ShowDialog();
        }

        private void iIГрупаОзнакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SampleSelectingForm form = new SampleSelectingForm(viborki, SecondGroupOfSigns, false);
            form.ShowDialog();
        }

        //тут диаграммы
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text = tabControl3.SelectedTab.Text;

            if (text == "Паралельні координати")
                BuildParallelCoordsChart();

            else if (text == "Бульбашкова діаграма")
                BuildBoubleDiagram();

            
        }

        private void BuildParallelCoordsChart()
        {
            //если Не прошли проверку
            if (!NDimStat.CheckingLengthOfStats())
            {
                MessageBox.Show("Вибраны ознаки мають різну довжину");
                return;
            }

            DateTime t1 = DateTime.Now;
            NDimStat.BuildparallelCoordsChart(ParallelCoordsChart.Series);
            ParallelCoordsChart.ChartAreas[0].AxisX.Minimum = 0;
            ParallelCoordsChart.ChartAreas[0].AxisX.Maximum = Selectedviborki.Count-1;
            DateTime t2 = DateTime.Now;
            //MessageBox.Show((t2 - t1).TotalSeconds.ToString());
        }

        private void BuildBoubleDiagram()
        {
            //проверка размерности
            if (!NDimStat.CheckingBeforeBoubleBuild())
            {
                MessageBox.Show("Для візуалізації бульбашковою діаграмою кількість ознак повинна дорівнювати 3!");
                return;
            }

            //проверка длинны
            if (!NDimStat.CheckingLengthOfStats())
            {
                MessageBox.Show("Вибраны ознаки мають різну довжину");
                return;
            }

            
            NDimStat.GetBoubleDiagram(BoubleChart.Series[0]);
            
            //настройка вида
            BoubleChart.ChartAreas[0].AxisX.Minimum = NDimStat.GetStat(0).Min - 20;
            BoubleChart.ChartAreas[0].AxisX.Maximum = NDimStat.GetStat(0).Max + 20;
            
            BoubleChart.ChartAreas[0].AxisY.Minimum = NDimStat.GetStat(1).Min - 20;
            BoubleChart.ChartAreas[0].AxisY.Maximum = NDimStat.GetStat(1).Max + 20;
        }

        double alphavalue = 0.05;

        private void перевіркаСпвпаданняЛвохToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STAT[] firstGroup = Selectedviborki.Select(val => val.stat).ToArray();
            STAT[] SecondGroup = SecondGroupOfSigns.Select(val => val.stat).ToArray();

            STATND XstatND = new STATND(firstGroup); 
            STATND YstatND = new STATND(SecondGroup);
            var mess = NdimHypothesis.ComparingTwoNdimAverage(XstatND, YstatND, alphavalue);

            MessageBox.Show(mess);
        }

        private void формуванняKГрупОзнакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MakeKndimGroup(viborki, Selectedviborki, KGroupsOfSigns);
            form.ShowDialog();
        }

        private void ПеревіркаСпівпаданняkБагатовимірнихСередніхПриНеСпівпаданніDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var statnds = GetArrayOfStatND();

            var mess = NdimHypothesis.ComparingKNdimAverage(statnds, alphavalue);
            MessageBox.Show(mess);
        }

        private STATND[] GetArrayOfStatND()
        {
            if (KGroupsOfSigns.Count == 0)
                формуванняKГрупОзнакToolStripMenuItem_Click(this, EventArgs.Empty);

            int n = KGroupsOfSigns.Count;
            STATND[] sTATNDs = new STATND[n];

            for (int i = 0; i < n; i++)
            {
                STAT[] s = KGroupsOfSigns[i].listOfVoborki.Select(v => v.stat).ToArray();

                sTATNDs[i] = new STATND(s);
            }

            return sTATNDs;
        }

        private void перевіркаСпівпаданняKNвимірнихDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var statnds = GetArrayOfStatND();

            var mess = NdimHypothesis.ComparingKNdimDC(statnds, alphavalue);
            MessageBox.Show(mess);
        }

        private void стандартизуватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STAT[] stats = Selectedviborki.Select(v => v.stat).ToArray();

            int length = stats.Length;
            for (int i = 0; i < length; i++)
            {
                stats[i].Sdandartize();
                stats[i].CalcMainParams();
            }
            //перезаполнение выведенного
            NDimStat.FillRandDC();
            NDimStat.GetMatrixOfScatterDiagrams(MatrixOfScatterDiagramsTableLayout);

            FillNDimLoggers();
        }

        private void частковийКоефыцієнтКореляціїToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void SunbmitButton_Click(object sender, EventArgs e)
        {
            try
            {

                int i = Convert.ToInt32(ItextBox.Text);
                int j = Convert.ToInt32(JtextBox.Text);

                Add2Nlog(NDimStat.GetPartialCoefCorrelationInfo(i, j));
            }

            catch
            {
                MessageBox.Show("Помилка виконання");
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            int yIndex=0;
            int[] xIndexes= new int[0];
            try
            {
                string xIndexesstr = XIndexesTextBox.Text;
                yIndex = Convert.ToInt32(YIndexTextBox.Text);

                xIndexes = xIndexesstr.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(v => Convert.ToInt32(v)).ToArray();
            }
            catch
            {
                MessageBox.Show("Помилка конвертування");
            }
            Add2Nlog(NdimRegression.FindParamsForNDim(NDimStat, xIndexes, yIndex, 0.05, DiagnosticDiagramChart));
        }

        private void мНКДля3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int yIndex = 0;
            int[] xIndexes = new int[0];
            try
            {
                string xIndexesstr = XIndexesTextBox.Text;
                yIndex = Convert.ToInt32(YIndexTextBox.Text);

                xIndexes = xIndexesstr.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(v => Convert.ToInt32(v)).ToArray();
            }
            catch
            {
                MessageBox.Show("Помилка конвертування");
            }
            Add2Nlog(NdimRegression.FindParamsForThreeDim(NDimStat.GetStat(xIndexes[0]), NDimStat.GetStat(xIndexes[1]), NDimStat.GetStat(yIndex)));
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string filename = NdimFilenameTextBox.Text;
            if (filename == "")
            {
                MessageBox.Show("Не вказано ім'я файллу для збереження");
                return;
            }

            try
            {
                NDimStat.SaveInFile(filename);
            }

            catch{
                MessageBox.Show("При збереженні файлу виникли помилки");
                return;
            }
        }

        private void мГКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwoDimStat.MGK();

            //очиститиToolStripMenuItem_Click(this, EventArgs.Empty);

            DimentionalTabControl.SelectedIndex = 1;

            TwoDimStat.GetTwoDimGist(chart3.Series);

            TwoDimStat.BackRotate(chart3.Series);
            TwoDimStat.GetKorrelationField(chart4.Series[0]);

            //построение шкалы
            BuildScale();
            
            //вывод параметров
            OutputParamsonGrid();
            
            //вывод кол-ва элементов
            this.StatusLabelNumberOfElements.Text = TwoDimStat.N.ToString();
            
            //альфа в текстбокс
            AlphaTextBox.Text = TwoDimStat.alpha.ToString();

            //настроили вид чартов
            TwoDimChartDesign();
        }
    }

    public class GroupOfViborkas
    {
        public string name;
        public List<Viborka> listOfVoborki;

        public GroupOfViborkas(string name, List<Viborka> viborki)
        {
            this.name = name;
            this.listOfVoborki = viborki;
        }
    }
}
