using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart1._1
{
    enum TypeDistr
    {
        Exp,
        Normal,
        Ravn,
        ArcSin
    }

    public partial class ModelWindow : Form
    {
        TypeDistr _type;

        MyForm _MainForm;
        
        public ModelWindow(MyForm mainform)
        {
           InitializeComponent();

           _MainForm = mainform;
        }

        public ModelWindow(MyForm mainform, string NameDistr)
            :this(mainform)
        {
            comboBoxTypeDistr.Text = NameDistr;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();

            sf.Filter = "Текстовые файлы|*.txt";

            if (sf.ShowDialog()==DialogResult.OK)
            {
                PathTextBox.Text = sf.FileName;

                //каретка в конец строки
                PathTextBox.SelectionStart = PathTextBox.Text.Length;
            }
        }

        private void comboBoxTypeDistr_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedText = comboBoxTypeDistr.SelectedItem.ToString();

            if (SelectedText.Equals("Експоненціальний"))
            {
                _type = TypeDistr.Exp;

                Param1Name.Text = "lyambda: ";

                Activate(Param1Name, Param1TextBox);

                DisActivate(Param2Name, Param2TextBox);
            }
            else if (SelectedText.Equals("Рівномірний"))
            {
                _type = TypeDistr.Ravn;

                Param1Name.Text = "a:";
 
                Param2Name.Text = "b:";

                Activate(Param1Name, Param1TextBox, Param2Name, Param2TextBox);
            }
            else if (SelectedText.Equals("Нормальний"))
            {
                _type = TypeDistr.Normal;

                Param1Name.Text = "m:";

                Param2Name.Text = "sigma:";

                Activate(Param1Name, Param1TextBox, Param2Name, Param2TextBox);
            }

            else if (SelectedText.Equals("Арксинуса"))
            {
                _type = TypeDistr.ArcSin;

                Param1Name.Text = "a:";

                Activate(Param1Name, Param1TextBox);

                DisActivate(Param2Name, Param2TextBox);
            }
        }

        void Activate(params Control[] controls)
        {
            foreach (var c in controls)
                c.Enabled = true;
        }

        void DisActivate(params Control[] controls)
        {
            foreach (var c in controls)
                c.Enabled = false;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            int n = (int)UpDowmNumbder.Value;

            string file = PathTextBox.Text;

            if (_type == TypeDistr.Exp)
            {
                double l = Convert.ToDouble(Param1TextBox.Text);

                Modelirovanie.Exp(l, n, file);
            }

            else if (_type == TypeDistr.Normal)
            {
                double m = Convert.ToDouble(Param1TextBox.Text);

                double s = Convert.ToDouble(Param2TextBox.Text);

                Modelirovanie.Norm(m, s, n, file);
            }

            else if (_type == TypeDistr.Ravn)
            {
                double a = Convert.ToDouble(Param1TextBox.Text);

                double b = Convert.ToDouble(Param2TextBox.Text);

                Modelirovanie.Ravn(a, b, n, file);
            }

            else if (_type == TypeDistr.ArcSin)
            {
                double a = Convert.ToDouble(Param1TextBox.Text);

                Modelirovanie.ArcSin(a, n, file);
            }
            this.Dispose();
        }
    }
}
