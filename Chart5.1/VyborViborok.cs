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
    public partial class VyborViborok : Form
    {
        MyForm _myform;

        public VyborViborok(MyForm myform)
        {
            _myform = myform;

            InitializeComponent();

            comboBox1.Items.AddRange(_myform.viborki.Select(x => x.Name).ToArray());

            comboBox2.Items.AddRange(_myform.viborki.Select(x => x.Name).ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _myform.viborka1 = _myform.viborki.First(x => x.Name == (string)comboBox1.SelectedItem);

            _myform.viborka2 = _myform.viborki.First(x => x.Name == (string)comboBox2.SelectedItem);

            this.Dispose();
        }
    }
}
