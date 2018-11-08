using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart5._1
{
    public partial class ModelTwoDimRegressionWindow : Form
    {
        public event EventHandler<TwoDimRegressionModerService> ModelEvent;
        public ModelTwoDimRegressionWindow()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ModelTwoDimRegressionWindow_Load(object sender, EventArgs e)
        {

        }

        private void XMinNumeric_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.InitialDirectory = Environment.CurrentDirectory;
            d.AddExtension = true;
            d.Filter = "txt файлы|*.txt";

            if (d.ShowDialog()==DialogResult.OK)
            {
                FileTextBOx.Text = Text = d.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ModelEvent!=null)
            {
                var serv = new TwoDimRegressionModerService();
                serv.xMin = (double)XMinNumeric.Value;
                serv.xMax = (double)XMaxNumeric.Value;
                serv.N = (int)NNumeric.Value;
                serv.a = (double)ANumeric.Value;
                serv.b = (double)BNumeric.Value;
                serv.SigmaEpsilon= (double)SigmaEpsilonNumeric.Value;
                serv.FilePath = FileTextBOx.Text;

                ModelEvent(this, serv);
                this.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }

    public class TwoDimRegressionModerService 
        {
            public double xMax;
            public double xMin;
            public int N;
            public double a;
            public double b;
            public double SigmaEpsilon;
            public string FilePath;
        }

}
