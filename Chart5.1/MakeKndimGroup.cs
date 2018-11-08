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
    public partial class MakeKndimGroup : Form
    {
        List<Viborka>       _AllSamples;
        List<Viborka>       _SelectedSamples;
        List<GroupOfViborkas> _groups;

        GroupOfViborkas SelectedGroup;

        public MakeKndimGroup(List<Viborka> AllSamples, List<Viborka> SelectedSamples, List<GroupOfViborkas> groups)
        {
            InitializeComponent();

            _AllSamples      = AllSamples;
            _SelectedSamples = SelectedSamples;
            _groups = groups;

            AddGroup(SelectedSamples);
            OutAllSamples(AllSamples);
        }

        void AddGroup(List<Viborka> Samples)
        {
            int n = _groups.Count;
            string name = "Вибірка" + (n + 1);

            GroupsListBox.Items.Add(name);
            _groups.Add(new GroupOfViborkas(name, Samples));
        }

        void OutGroups()
        {
            GroupsListBox.Items.Clear();
            int length = _groups.Count;

            for (int i = 0; i < length; i++)
                GroupsListBox.Items.Add(_groups[i].name);

        }

        void OutAllSamples(List<Viborka> samples)
        {
            AllViborkiListBox.Items.Clear();

            for (int i = 0; i < samples.Count; i++)
                AllViborkiListBox.Items.Add(samples[i].Name);
        }



        private void GroupsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupsListBox.SelectedIndex == -1)
                return;

            SelectedVoborkiListBox.Items.Clear();

            var Selected = GroupsListBox.Items[GroupsListBox.SelectedIndex].ToString();
            SelectedGroup = _groups.Find(S => S.name == Selected);

            var SelectedViborki = SelectedGroup.listOfVoborki;
            
            for (int i = 0; i < SelectedViborki.Count; i++)
            {
                SelectedVoborkiListBox.Items.Add(SelectedViborki[i].Name);
            }
        }

        //додати
        private void button3_Click(object sender, EventArgs e)
        {
            var Selected = AllViborkiListBox.Items[AllViborkiListBox.SelectedIndex].ToString();
            var SelectedSample = _AllSamples.Find(S => S.Name == Selected);

            SelectedGroup.listOfVoborki.Add(SelectedSample);
            GroupsListBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        //додати всі
        private void button5_Click(object sender, EventArgs e)
        {
            string[] names = _AllSamples.Select(s => s.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                var Selected = names[i];
                var SelectedSample = _AllSamples.Find(S => S.Name == Selected);

                SelectedGroup.listOfVoborki.Add(SelectedSample);
            }
           
            GroupsListBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        //видалити
        private void button4_Click(object sender, EventArgs e)
        {
            int SelectedIndex = SelectedVoborkiListBox.SelectedIndex;
            SelectedGroup.listOfVoborki.RemoveAt(SelectedIndex);
            GroupsListBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        //видалити всі
        private void button6_Click(object sender, EventArgs e)
        {
            SelectedGroup.listOfVoborki.Clear();
            //GroupsListBox_SelectedIndexChanged(this, EventArgs.Empty);
            SelectedVoborkiListBox.Items.Clear();
        }

        //добавить группу
        private void button1_Click(object sender, EventArgs e)
        {
            AddGroup(new List<Viborka>());
            OutGroups();
        }

        //удалить группу
        private void button2_Click(object sender, EventArgs e)
        {
            int selectedindex = GroupsListBox.SelectedIndex;
            _groups.RemoveAt(selectedindex);


            OutGroups();
            SelectedVoborkiListBox.Items.Clear();
        }
    }
}
