using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chart1._1
{
    public partial class FormAnomalii : Form
    {
        STAT _stat;
        MyForm _myform;
        double[] BeforeRemoveAnomals;
        bool flag = false;              //нажималась ли кнопка Удалить

        public FormAnomalii(STAT stat, MyForm myform)
        {
            InitializeComponent();

            _stat = stat;

            _myform = myform;

            int okrug =_myform.okrugl;

            decimal pow = (decimal)Math.Pow(10, -okrug);

            numericUpDownA.DecimalPlaces = okrug;

            numericUpDownB.DecimalPlaces = okrug;

            numericUpDownA.Increment = pow;

            numericUpDownB.Increment = pow;
        }

        private void comboBoxMetod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //-Не вибрано-
            //Перший спосіб
            //Другий спосіб
            //Третій спосіб

            Action SetUpDownBorders = () =>
                {
                    numericUpDownA.Value = (decimal)Math.Round(_stat.BorderA, _myform.okrugl);

                    numericUpDownB.Value = (decimal)Math.Round(_stat.BorderB, _myform.okrugl);
                };

            string selectedtext = comboBoxMetod.SelectedItem.ToString();

            if (selectedtext.Equals("-Не вибрано-"))
            {
                numericUpDownA.Value = 0;

                numericUpDownB.Value = 0;           
            }

            else if (selectedtext.Equals("Перший спосіб"))
            {
                _stat.AnomalFirt(AnomalOptions.OnlyFindBordersAandB);

                SetUpDownBorders();
            }

            else if (selectedtext.Equals("Другий спосіб"))
            {
                _stat.AnomalSecond(AnomalOptions.OnlyFindBordersAandB);

                SetUpDownBorders();
            }

            else if (selectedtext.Equals("Третій спосіб"))
            {
                _stat.AnomalThird(AnomalOptions.OnlyFindBordersAandB);

                SetUpDownBorders();
            }
        }

        private void FormAnomalii_Load(object sender, EventArgs e)
        { 
            BeforeRemoveAnomals = (double[])_stat.d.Clone();

            RefreshListBox();
        }

        void RefreshListBox()
        {
            listBox.Items.Clear();  

            foreach (double value in _stat.d)
                listBox.Items.Add(value);
       }

        void Remov()
        {
            double a = Convert.ToDouble(numericUpDownA.Value);

            double b = Convert.ToDouble(numericUpDownB.Value);

            _stat.RemoveAnomals(a, b);  //скорее всего эта ф-я должна быть в этом классе а не в STAT
            
            //!!!тут нужно предусмотреть вариант в котором все элементы удаляются
            _myform.UpdateMainForm();//это перестоит гистограмму
        }

        private void ButRemove_Click(object sender, EventArgs e)
        {
            Remov();

            RefreshListBox();

            comboBoxMetod.SelectedItem = comboBoxMetod.Items[0];

            flag = true;
        }

        private void numericUpDownA_ValueChanged(object sender, EventArgs e)
        {
            flag = false;
        }

        private void numericUpDownB_ValueChanged(object sender, EventArgs e)
        {
            flag = false;
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            if (!flag)
                Remov();

            this.Dispose();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            _stat.d = (double[])BeforeRemoveAnomals.Clone();

            _myform.UpdateMainForm();

            this.Dispose();
        }
    }
}
