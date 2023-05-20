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
using System.Windows.Shapes;

namespace SistemiProjekta_WPF
{
    /// <summary>
    /// Interaction logic for AddChildDialog.xaml
    /// </summary>
    public partial class AddChildDialog : Window
    {
        public AddChildDialog()
        {
            InitializeComponent();
            Izbira_Combobox.Items.Add(MyEnum.Linearna);
            Izbira_Combobox.Items.Add(MyEnum.Logaritemska);
            Izbira_Combobox.Items.Add(MyEnum.Tabelarična);
            Izbira_Combobox.Items.Add(MyEnum.Eksponentna);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = mySlider.Value;
            valueText.Text = value.ToString("0.00");
        }
    }
}
