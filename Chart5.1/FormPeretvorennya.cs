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
    public partial class FormPeretvorennya : Form
    {
        STAT _stat;

        MyForm _myform;

        double[] _BeforeActions;

        double[] _StartDataInTab;

        bool flag;

        public FormPeretvorennya(STAT Stat, MyForm myForm)
        {
            InitializeComponent();

            _stat = Stat;

            _myform = myForm;

            _BeforeActions = (double[])_stat.d.Clone();

            tabControl1_SelectedIndexChanged(this, EventArgs.Empty);
        }

        public FormPeretvorennya(STAT Stat, MyForm myForm, int SelectIndex)
            :this(Stat, myForm)
        {
            tabControl1.SelectedIndex = SelectIndex;
        }

        private void UpDownZsuv_ValueChanged(object sender, EventArgs e)
        {
            _stat.d = (double[])_BeforeActions.Clone();

            Zsuv(_stat.d, Convert.ToDouble(UpDownZsuv.Value));

            _myform.UpdateMainForm();
        }

        private void LogOsnUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (LogOsnUpDown.Value == 1)
                {
                    MessageBox.Show("Недопустиме значення!");

                    LogOsnUpDown.Value = 2;
                }

                DovilnaRadioButton.Checked = true;

                _stat.d = (double[])_BeforeActions.Clone();

                Logarifmirovat(_stat.d, Convert.ToDouble(LogOsnUpDown.Value));

            }

            catch {
                MessageBox.Show("Не вдалося провести перетворення");
                _stat.d = (double[])_BeforeActions.Clone();
            }
            _myform.UpdateMainForm();
        }

        private void ExpRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            LogOsnUpDown.Value = (decimal)Math.Round(Math.E, 2);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _stat.d = (double[])_BeforeActions.Clone();

            _myform.UpdateMainForm();

            this.Dispose();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void StepinUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _stat.d = (double[])_BeforeActions.Clone();

                PidnesennyaDoStepenya(_stat.d, Convert.ToDouble(StepinUpDown.Value));
            }

            catch
            {
                MessageBox.Show("Не вдалося провести перетворення");
                _stat.d = (double[])_BeforeActions.Clone();
            }
            _myform.UpdateMainForm();
        }

        void Zsuv(double[] d, double zsuv)
        {
            for (int i = 0; i < d.Length; i++)
                d[i] = d[i] + zsuv;
        }

        void Logarifmirovat(double[] d, double osnova)
        {
            double m = d.Min();
            if (m < 1)
                Zsuv(d, Math.Abs(m) + 1.5);

            for (int i = 0; i < d.Length; i++)
                d[i] = Math.Log(d[i], osnova);
        }

        void PidnesennyaDoStepenya(double[] d, double stepen)
        {
            for (int i = 0; i < d.Length; i++)
                d[i] = Math.Pow(d[i], stepen);   
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           _stat.d = (double[])_BeforeActions.Clone();
        }
    }
}
