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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SistemiProjekta_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Node rootNode = new Node("Ocenjevanje", 0, 1);
        public MainWindow()
        {
            InitializeComponent();
           

       
            //rootNode.Otroci.Add(new Node("Child 1", 0, 1f));
            //rootNode.Otroci.Add(new Node("Child 2", 0, 1f));
            //rootNode.Otroci[1].Otroci.Add(new Node("Grandchild", 3, 0.2f));

            Drevo.DataContext = rootNode;
            

        }



        //private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    Node selectedNode = e.NewValue as Node;

        //    if (selectedNode != null)
        //    {
        //        AddChildDialog dialog = new AddChildDialog();
        //        if (dialog.ShowDialog() == true)
        //        {
        //            float value = float.Parse(dialog.ValueTextBox.Text);
        //            float weight = float.Parse(dialog.WeightTextBox.Text);
        //            string name = dialog.NameTextBox.Text;

        //            selectedNode.Otroci.Add(new Node(name, value, weight));
        //        }
        //    }
        //}

        private void AddChildButton_Click(object sender, RoutedEventArgs e)
        {
            Node selectedNode = Drevo.SelectedItem as Node;
            
            if (selectedNode == null)
            {
                var x = Drevo.SelectedItem as TreeViewItem;
                selectedNode = x.DataContext as Node;
            }
            
            if (selectedNode != null)
            {
                AddChildDialog dialog = new AddChildDialog();
                if (dialog.ShowDialog() == true)
                {
                    float value = float.Parse(dialog.ValueTextBox.Text);
                    float weight = float.Parse(dialog.WeightTextBox.Text);
                    string name = dialog.NameTextBox.Text;

                    selectedNode.Otroci.Add(new Node(name, value, weight));
                }
            }
        }

        private void Izracunaj_Button_Click(object sender, RoutedEventArgs e)
        {
            vrednost.Text = rootNode.VrniVrednost().ToString();
        }
    }

    public class Node
    {
        public float Vrednost { get; set; }
        public float Utez { get; set; }
        public string Ime { get; set; }
        public ObservableCollection<Node> Otroci { get; set; }

        public Node(string ime, float value, float utez)
        {
            Ime = ime;
            Vrednost = value;
            Utez = utez;
            Otroci = new ObservableCollection<Node>();
        }

        public Node()
        {
            Otroci = new ObservableCollection<Node>();
        }


        public float VrniVrednost()
        {
            if (Otroci.Count == 0)
            {
                return Vrednost * Utez;
            }
            else
            {
                foreach (Node n in Otroci)
                {
                    Vrednost = Vrednost + n.VrniVrednost();
                }
                return Vrednost * Utez;
            }
        }
    }
}
