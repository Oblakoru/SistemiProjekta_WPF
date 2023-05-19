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
        public static Node rootNode = new Node("Ocenjevanje", 0, 1, 0, 0);
        public static ObservableCollection<Node> Listi = new ObservableCollection<Node>();
        public static ObservableCollection<Node> kopije = new ObservableCollection<Node>();
        public static int steviloAlternativ = 0;

        public static ObservableCollection<Alternativa> IzracunaneAlternative = new ObservableCollection<Alternativa>();


        public MainWindow()
        {
            InitializeComponent();

            rootNode.Otroci.Add(new Node("Cena", 0, 0.5f, 0, 100, MyEnum.Linearna));
            rootNode.Otroci.Add(new Node("Izgled", 0, 0.5f, 0, 100, MyEnum.Linearna));
            //rootNode.Otroci[1].Otroci.Add(new Node("Grandchild0", 0, 0.5f, 0, 10, MyEnum.Linearna));
            //rootNode.Otroci[1].Otroci.Add(new Node("Grandchild1", 0, 0.5f, 0, 10, MyEnum.Linearna));

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
        // 

        private void AddChildButton_Click(object sender, RoutedEventArgs e)
        {
            Node selectedNode = Drevo.SelectedItem as Node;

            try
            {
                if (selectedNode == null)
                {
                    var x = Drevo.SelectedItem as TreeViewItem;
                    selectedNode = x.DataContext as Node;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ojoj!");
            }

            try
            {
                if (selectedNode != null)
                {
                    AddChildDialog dialog = new AddChildDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        float value = float.Parse(dialog.ValueTextBox.Text);
                        float weight = float.Parse(dialog.WeightTextBox.Text);
                        string name = dialog.NameTextBox.Text;
                        int min = int.Parse(dialog.MinTextBox.Text);
                        int max = int.Parse(dialog.MaxTextBox.Text);

                        MyEnum vrsta = (MyEnum)dialog.Izbira_Combobox.SelectedItem;

                        selectedNode.Otroci.Add(new Node(name, value, weight, min, max, vrsta));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ojoj!");
            }

        }

        private void Izracunaj_Button_Click(object sender, RoutedEventArgs e)
        {
            vrednost.Text = rootNode.VrniVrednost().ToString();
        }
        private void DodeliListe(Node node)
        {
            node.GetAllChildNodes();
            node.Listi = new ObservableCollection<Node>(Listi);
            Listi.Clear();
        }
        private void Prikazi_List(object sender, RoutedEventArgs e)
        {
            rootNode.GetAllChildNodes();
            rootNode.Listi = new ObservableCollection<Node>(Listi);
            Listi.Clear();

            string besedilo = "";
            foreach (Node x in rootNode.Listi)
            {
                besedilo = besedilo + " " +  x.Ime;

            }
            vrednost.Text = besedilo;
        }

        private void PrikazPolj(object sender, RoutedEventArgs e)
        {
            steviloAlternativ++;

            StackPanel stackPanel = new StackPanel();
            //stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;



            foreach (Node node in rootNode.Listi)
            {
                // create a label with the "Ime" attribute of the node
                Label label = new Label();
                label.Content = node.Ime;
                label.Width = 100;

                // create a textbox for the "Vrednost" attribute of the node
                TextBox textBox = new TextBox();
                textBox.Text = node.Vrednost.ToString();
                textBox.Width = 400;

                //StackPanel stackPanel = new StackPanel();
                //stackPanel.Orientation = Orientation.Horizontal;
                //stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

                stackPanel.Children.Add(label);
                stackPanel.Children.Add(textBox);

                //alternative.Children.Add(stackPanel);
            }
            alternative.Children.Add(stackPanel);
        }

        ///Dodat treba za stackpanel parenta
        private void IzracunajVrednost(object sender, RoutedEventArgs e)
        {
            //Added
            for (int z = 0; z < alternative.Children.Count; z++)
            {
                NarediKopijo(rootNode);
            }

            // gre skozi vsak stackpanel - ena alternativa
            for (int x = 0; x < alternative.Children.Count; x++)
            {
                if (alternative.Children[x] is StackPanel)
                {
                    StackPanel stackPanel = alternative.Children[x] as StackPanel;

                    int steviloListov = 0;
                    //novo
                    DodeliListe(kopije[x]);
                    //Gre skozi vsak objekt v stackpanelu - če je ta Textbox - se vrednost prenese na node!
                    for (int y = 0; y < stackPanel.Children.Count; y++)
                    {
                        if (stackPanel.Children[y] is TextBox)
                        {   
                           
                            TextBox textBox = stackPanel.Children[y] as TextBox;
                            /// TUle bo treba fixat da bo y sam med 1-2
                            /// //Problem je ker se posodi sam Listi, ne pa Otroci!!!
                            kopije[x].Listi[steviloListov].Vrednost = float.Parse(textBox.Text);
                            //Dodano
                            steviloListov++;

                            //Alternativa nova = new Alternativa();
                            //nova.Vrednost = rootNode.VrniVrednost();
                            //nova.Ime = x.ToString();
                            //IzracunaneAlternative.Add(nova);
                            //vrednost_Izracunane.Text = nova.Ime + " " + nova.Vrednost.ToString();
                           
                        }
                    }

                    Alternativa nova = new Alternativa();
                    nova.Vrednost = kopije[x].VrniVrednost();
                    nova.Ime = x.ToString();
                    IzracunaneAlternative.Add(nova);
                    //vrednost_Izracunane.Text = nova.Ime + " " + nova.Vrednost.ToString();
                }
            }

            //vrednost_Izracunane.Text = rootNode.VrniVrednost().ToString();

            foreach (var alternative in IzracunaneAlternative)
            {
                vrednost_Izracunane.Text += "\n" + alternative.Ime + " Vrednost je: " + alternative.Vrednost;
            }
        }

        Node DeepCopy(Node original)
        {
            Node copiedNode = new Node
            {
                Vrednost = original.Vrednost,
                Utez = original.Utez,
                Ime = original.Ime,
                Min = original.Min,
                Max = original.Max,
                Vrsta = original.Vrsta,
                Listi = new ObservableCollection<Node>(original.Listi.Select(n => DeepCopy(n))),
                Otroci = new ObservableCollection<Node>(original.Otroci.Select(n => DeepCopy(n)))
            };
            return copiedNode;
        }


        public void NarediKopijo(Node node)
        {
            Node originalNode = node; 

            Node copiedNode = new Node
            {
                Vrednost = originalNode.Vrednost,
                Utez = originalNode.Utez,
                Ime = originalNode.Ime,
                Min = originalNode.Min,
                Max = originalNode.Max,
                Vrsta = originalNode.Vrsta,
                Listi = new ObservableCollection<Node>(originalNode.Listi.Select(n => DeepCopy(n))),
                Otroci = new ObservableCollection<Node>(originalNode.Otroci.Select(n => DeepCopy(n)))
            };
            kopije.Add(copiedNode);
        }



        /// TO DO !
        //private void Izbrisi(object sender, RoutedEventArgs e)
        //{
        //    Node selectedNode = Drevo.SelectedItem as Node;

        //    try
        //    {
        //        if (selectedNode == null)
        //        {
        //            var x = Drevo.SelectedItem as TreeViewItem;
        //            selectedNode = x.DataContext as Node;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ojoj!");
        //    }


        //    if (selectedNode != null)
        //    {
        //        AddChildDialog dialog = new AddChildDialog();
        //        if (dialog.ShowDialog() == true)
        //        {
        //            float value = float.Parse(dialog.ValueTextBox.Text);
        //            float weight = float.Parse(dialog.WeightTextBox.Text);
        //            string name = dialog.NameTextBox.Text;
        //            int min = int.Parse(dialog.MinTextBox.Text);
        //            int max = int.Parse(dialog.MaxTextBox.Text);

        //            MyEnum vrsta = (MyEnum)dialog.Izbira_Combobox.SelectedItem;

        //            selectedNode.Otroci.Add(new Node(name, value, weight, min, max, vrsta));
        //        }
        //    }
        //}
    }

    public class Node
    {
        public float Vrednost { get; set; }
        public float Utez { get; set; }
        public string Ime { get; set; }
        public int Min { get; set; }

        public int Max { get; set; }

        public MyEnum Vrsta = MyEnum.Tabelarična;

        public ObservableCollection<Node> Listi = new ObservableCollection<Node>();

        public ObservableCollection<Node> Otroci { get; set; }

        public Node(string ime, float value, float utez, int min, int max, MyEnum vrsta = MyEnum.Tabelarična)
        {
            Ime = ime;

            Vrednost = value;

            Utez = utez;

            Otroci = new ObservableCollection<Node>();

            Min = min;

            Max = max;

            Vrsta = vrsta;
        }

        public Node()
        {
            Otroci = new ObservableCollection<Node>();
        }


        public float VrniVrednost()
        {
            if (Otroci.Count == 0)
            {
                if (Vrsta == MyEnum.Linearna)
                {
                    float m = (float)(1.0 / (Max - Min)); 
                    float b = -m * Min; 
                    float y = m * Vrednost + b; 
                    return y * Utez;
                }
                else if (Vrsta == MyEnum.Logaritemska)
                {
                    //double a = 1.0 / (1.0 + Math.Exp(-10)); // the maximum output value
                    //double k = 10 / Math.Log(1 / a - 1); // the steepness of the logarithmic function
                    //double y = a / (1 + Math.Exp(-k * x)); // calculate the output value using the logarithmic function
                    return Vrednost;
                }
                else if (Vrsta == MyEnum.Eksponentna)
                {
                    double k = 1 / Math.Pow(Max, 2);
                    float x = (float)(k * (Math.Pow(Vrednost, 2)));
                    return x * Utez;
                }
                else
                {
                    return Vrednost * Utez;
                }

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

        public void GetAllChildNodes()
        {
            if (Otroci.Count == 0)
            {
                MainWindow.Listi.Add(this);
            }
            else
            {
                foreach (Node n in Otroci)
                {
                    n.GetAllChildNodes();
                }
            }
        }
    }

    public enum MyEnum
    {
        Linearna,
        Logaritemska,
        Tabelarična,
        Eksponentna
    }

    public class Alternativa
    {
        public float Vrednost { get; set; }
        public string Ime { get; set; }
    }

}
