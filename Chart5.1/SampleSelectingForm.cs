using Chart1._1;
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
    public partial class SampleSelectingForm : Form
    {
        List<Viborka> allSamples;
        List<Viborka> selectedSamples;

        public SampleSelectingForm(List<Viborka> AllSamples, List<Viborka> SelectedSamples, bool outAll)
        {
            InitializeComponent();

            allSamples = AllSamples;
            selectedSamples = SelectedSamples;

            if (outAll)
                SelectAllsmpl();

            OutSamplesOnListView();
        }

        //Вывести выбранные выборки и текущие выбранные выборки
        private void OutSamplesOnListView()
        {
            SelectedSamplesListBox.Items.Clear();
            allSamlesListBox.Items.Clear();

            for (int i = 0; i < selectedSamples.Count; i++)
                SelectedSamplesListBox.Items.Add(selectedSamples[i].Name);

            for (int i = 0; i < allSamples.Count; i++)
                allSamlesListBox.Items.Add(allSamples[i].Name);
            
        }

        private void addSelectedSampleClick(object sender, EventArgs e)//добавить
        {
            var Selected = allSamlesListBox.Items[allSamlesListBox.SelectedIndex].ToString();

            var SelectedSample = allSamples.Find(S => S.Name == Selected);
            selectedSamples.Add(SelectedSample);

            OutSamplesOnListView();
        }

        private void AddAllSamples(object sender, EventArgs e)//добавить все
        {
            for (int i = 0; i < allSamples.Count; i++)
            {
                var sample = allSamples[i];

                var exist = selectedSamples.Exists(s => s == sample);

                if(!exist)
                    selectedSamples.Add(sample);
            }

            OutSamplesOnListView();
        }

        private void Remove(object sender, EventArgs e)
        {
            int SelectedIndex = SelectedSamplesListBox.SelectedIndex;
            selectedSamples.RemoveAt(SelectedIndex);
            OutSamplesOnListView();
        }

        private void RemoveAll(object sender, EventArgs e)
        {
            selectedSamples.Clear();
            OutSamplesOnListView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void SelectAllsmpl()
        {
            AddAllSamples(this, EventArgs.Empty);
            OutSamplesOnListView();
        }
    }
}
