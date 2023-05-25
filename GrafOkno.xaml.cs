using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace SistemiProjekta_WPF
{
    /// <summary>
    /// Interaction logic for GrafOkno.xaml
    /// </summary>
    public partial class GrafOkno : Window
    {
        public List<string> Labels { get; set; }
        public ChartValues<ObservableValue> DataPoints { get; set; }

        public GrafOkno(ObservableCollection<Alternativa> objects)
        {
            InitializeComponent();

            Labels = new List<string>();
            DataPoints = new ChartValues<ObservableValue>();

            // Populate Labels and DataPoints from the objects
            foreach (Alternativa obj in objects)
            {
                Labels.Add(obj.Ime);
                DataPoints.Add(new ObservableValue(obj.Vrednost));
            }

            DataContext = this;
        }
    }
}
