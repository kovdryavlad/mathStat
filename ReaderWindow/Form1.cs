using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stat2;

namespace ReaderWindow
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Все файлы|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var r = FileReader.ReadFile(ofd.FileName);

                var r2 = Stat2.DoubleConverter.ConvertToDoubleValuesInColumns(r);
                r2 = Stat2.DoubleConverter.RegroupBySamples(r2);

            }
        }
    }
}
