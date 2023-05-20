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
    /// Interaction logic for DodajParameter.xaml
    /// </summary>
    public partial class DodajParameter : Window
    {
        public Node glavniNode { get; set; }
        public Alternativa alternativa { get; set; }

        public DodajParameter(Node parameter)
        {
            InitializeComponent();
            glavniNode = parameter;
            foreach (Node node in glavniNode.Listi)
            {
                Label label = new Label();
                label.Content = node.Ime;
                label.Width = 100;

                TextBox textBox = new TextBox();
                textBox.Text = node.Vrednost.ToString();
                textBox.Width = 400;

                notranjiStack.Children.Add(label);
                notranjiStack.Children.Add(textBox);
            }
        }



       // private void OkButton_Click(object sender, RoutedEventArgs e)
       // {
       //     DialogResult = true;
       //     Close();
       // }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void IzracunButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int steviloListov = 0;
                for (int y = 0; y < notranjiStack.Children.Count; y++)
                {
                    if (notranjiStack.Children[y] is TextBox)
                    {
                        TextBox textBox = notranjiStack.Children[y] as TextBox;

                        if ((glavniNode.Listi[steviloListov].Min <= Convert.ToInt32(textBox.Text)) && Convert.ToInt32(textBox.Text) <= glavniNode.Listi[steviloListov].Max)
                        {
                            glavniNode.Listi[steviloListov].Vrednost = float.Parse(textBox.Text);
                            steviloListov++;
                        }
                        else
                        {
                            throw new Exception();
                        }

                    }
                }

                alternativa = new Alternativa();
                alternativa.Vrednost = glavniNode.VrniVrednost();
                alternativa.Ime = NameTextBox.Text;

                MessageBox.Show(alternativa.Ime + " " + alternativa.Vrednost.ToString());

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vrednost ni bila pravilno vnešena!");
            }
            
        }
    }
}
