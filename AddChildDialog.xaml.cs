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
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)(checkBox.IsChecked) == false) 
            {
                Izbira_Combobox.Visibility = Visibility.Collapsed;
                Izbira_Label.Visibility = Visibility.Collapsed;
                MinLabel.Visibility = Visibility.Collapsed;
                MaxLabel.Visibility = Visibility.Collapsed;
                MinTextBox.Visibility = Visibility.Collapsed;
                MaxTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                Izbira_Combobox.Visibility = Visibility.Visible;
                Izbira_Label.Visibility = Visibility.Visible;
                MinLabel.Visibility = Visibility.Visible;
                MaxLabel.Visibility = Visibility.Visible;
                MinTextBox.Visibility= Visibility.Visible;
                MaxTextBox.Visibility= Visibility.Visible;
            }
        }


    }
}
