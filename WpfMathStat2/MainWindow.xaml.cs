using Microsoft.Win32;
using Stat2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfMathStat2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Все файлы|*.*";

            if (ofd.ShowDialog() == true)
            {
                var r = FileReader.ReadFile(ofd.FileName);

                var r2 = Stat2.DoubleConverter.ConvertToDoubleValuesInColumns(r);
                r2 = Stat2.DoubleConverter.RegroupBySamples(r2);

                List<Sample> samples = new List<Sample>();
                for (int i = 0; i < r2.Length; i++)
                {
                    samples.Add(new Sample(ofd.FileName + "#" + i, r2[i]));    
                }
                SamlesList.ItemsSource = samples;
            }
        }

        class Sample
        {
            public string Name{get;set;}
            public double[] data;
            public int Count { get { return data.Length; } }

            public Sample(string name, double[] data)
            {
                this.Name = name;
                this.data = data;
            }
        }
    }
}
