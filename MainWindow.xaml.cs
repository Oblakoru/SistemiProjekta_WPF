﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        //Začetni rook na katerega gradimo hierarhijo, vrednost tega bo končen izračun alternative.
        public static Node rootNode = new Node("Ocenjevanje", 0, 1, 0, 0, false);


        //Nova stvar!
        public static ObservableCollection<Node> rootNodeList = new ObservableCollection<Node>();

        //Začasni ObservableCollection, ki vsebuje Liste drevesa. Njegove podatke nato prenesemo na List property objekta.
        public static ObservableCollection<Node> Listi = new ObservableCollection<Node>();

        //Imena ter končne ocene posameznih alternativ
        public static ObservableCollection<Alternativa> IzracunaneAlternative = new ObservableCollection<Alternativa>();

        public MainWindow()
        {
            InitializeComponent();

            //rootNode.Otroci.Add(new Node("Cena", 0, 0.5f, 0, 100, MyEnum.Linearna));
            //rootNode.Otroci.Add(new Node("Izgled", 0, 0.5f, 0, 100, MyEnum.Linearna));
            //rootNode.Otroci[1].Otroci.Add(new Node("Grandchild0", 0, 0.5f, 0, 10, MyEnum.Linearna));
            //rootNode.Otroci[1].Otroci.Add(new Node("Grandchild1", 0, 0.6f, 0, 10, MyEnum.Linearna));


            rootNodeList.Add(rootNode);

            //Old
            //Drevo.DataContext = rootNode;

            //new
            Drevo.DataContext = rootNodeList;

        }

        //Dodajanja otroka v hierarhijo preko novega okna.
        private void AddChildButton_Click(object sender, RoutedEventArgs e)
        {

            Node selectedNode = Drevo.SelectedItem as Node;
            try
            {
                if (selectedNode == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("Izbrati je potrebno NODE!");
            }
           
            
           //var x = Drevo.SelectedItem as TreeViewItem;

           // try
           // {
           //     selectedNode = x.DataContext as Node;
           //     if (selectedNode == null)
           //     {
           //         throw new InvalidOperationException("Izberi Node!");
           //     }
           // }
           // catch
           // {
           //     MessageBox.Show("Izberi Node!");
           // }
            

            //try
            //{
            //    if( selectedNode == null)
            //    {
            //        throw new Exception();
            //    }
            //    selectedNode = x.DataContext as Node;
            //}
            //catch
            //{
            //    MessageBox.Show("Izbrati je potrebno node!");
            //}

            try
            {
                if (selectedNode != null)
                {
                    //Odpiranje novega okna, ki izpiše vse podatke iz textboxov
                    AddChildDialog dialog = new AddChildDialog();

                    //Preverjanje, če je dialog bil pravilno potrjen - OK gumb
                    if (dialog.ShowDialog() == true)
                    {
                        //float value = float.Parse(dialog.ValueTextBox.Text);
                        //float weight = float.Parse(dialog.WeightTextBox.Text);
                        float weight = float.Parse(dialog.valueText.Text);
                        string name = dialog.NameTextBox.Text;
                        int min = int.Parse(dialog.MinTextBox.Text);
                        int max = int.Parse(dialog.MaxTextBox.Text);

                        bool list;

                        MyEnum vrsta;

                        if(dialog.checkBox.IsChecked == true)
                        {
                            
                            vrsta = (MyEnum)dialog.Izbira_Combobox.SelectedItem;
                            if (vrsta == MyEnum.Tabelarična)
                            {
                                MessageBox.Show("Ker je bila izbrana tabelarična funkcija, je domena med 0 in 1");
                                min = 0;
                                max = 1;
                            }
                            list = true;
                            
                        }
                        else
                        {
                            vrsta = MyEnum.Tabelarična;
                            list = false;
                        }
                        

                        //Izbranemu nodu dodamo otroka
                        //selectedNode.Otroci.Add(new Node(name, value, weight, min, max, vrsta));
                        selectedNode.Otroci.Add(new Node(name, 0, weight, min, max, list, vrsta));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ojoj!");
            }

        }

        //Vsakemu nodu naredimo novi ObservableCollection, v katerem se nahajajo reference to njegovih listov z namenom spreminjanja vrednosti
        private void DodeliListe(Node node)
        {
            node.GetAllChildNodes();
            node.Listi = new ObservableCollection<Node>(Listi);
            Listi.Clear();
        }


        private void Prikazi_List(object sender, RoutedEventArgs e)
        {
            try
            {
                //PReverjanje če so uteži pravilna
                rootNode.CheckSumInLevelRecursive();

                rootNode.GetAllChildNodes();
                rootNode.Listi = new ObservableCollection<Node>(Listi);
                Listi.Clear();

                foreach (Node node in rootNode.Listi)
                {
                    if (node.List == false)
                        throw new Exception("Nepravilen list");
                }

                tab_altermatove.IsEnabled = true;
                tab_root.SelectedIndex = 1;
                tab_hierarhija.IsEnabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ojoj!");
            }
                
        }

        //Tukaj se dodajajo alternative
        private void PrikazPolj(object sender, RoutedEventArgs e)
        {
            var zacasna = NarediKopijo(rootNode);
            DodeliListe(zacasna);
            //Odpiranje novega okna, ki izpiše vse podatke iz textboxov
            DodajParameter dialog = new DodajParameter(zacasna);
            //Preverjanje, če je dialog bil pravilno potrjen - OK gumb
            if (dialog.ShowDialog() == true)
            {
                IzracunaneAlternative.Add(dialog.alternativa);

                vrednost_Izracunane.Text = "Trenutno ovrednotene alternative: ";
                foreach (var alternative in IzracunaneAlternative)
                {
                    vrednost_Izracunane.Text += "\n" + alternative.Ime;
                }
            }
        }

        private void IzracunajVrednost(object sender, RoutedEventArgs e)
        {
            vrednost_Izracunane.Text = "Ocene trenutnih alternativ: ";
            foreach (var alternative in IzracunaneAlternative)
            {
                vrednost_Izracunane.Text += "\n" + alternative.Ime + " Vrednost je: " + alternative.Vrednost;
            }

            var zacasno = IzracunaneAlternative.OrderByDescending(obj => obj.Vrednost).FirstOrDefault();
            vrednost_Izracunane.Text += "\n Zmagovalna alternativa je: " + zacasno.Ime + " z vrednostjo: " + zacasno.Vrednost.ToString();
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

        // Funkcija za kloniranje hierarhije za vsak parameter posebej. Reference se ustvarijo ponovno za vsak parameter.
        public Node NarediKopijo(Node node)
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
            //kopije.Add(copiedNode);

            //Dodana funkcionalnost za windows
            return copiedNode;
        }

        private void UrediChildButton_Click(object sender, RoutedEventArgs e)
        {
            Node selectedNode = Drevo.SelectedItem as Node;

            try
            {
                //if (selectedNode == null)
                //{
                //    var x = Drevo.SelectedItem as TreeViewItem;
                //    selectedNode = x.DataContext as Node;
                //}

                if (selectedNode == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Izbrati je poterbno NODE!");
            }

            try
            {
                if (selectedNode != null)
                {

                    AddChildDialog dialog = new AddChildDialog();
                    dialog.valueText.Text = selectedNode.Utez.ToString();
                    dialog.NameTextBox.Text = selectedNode.Ime;
                    dialog.MinTextBox.Text = selectedNode.Min.ToString();
                    dialog.MaxTextBox.Text = selectedNode.Max.ToString();

                    //Preverjanje, če je dialog bil pravilno potrjen - OK gumb
                    if (dialog.ShowDialog() == true)
                    {
                        float weight = float.Parse(dialog.valueText.Text);
                        string name = dialog.NameTextBox.Text;
                        int min = int.Parse(dialog.MinTextBox.Text);
                        int max = int.Parse(dialog.MaxTextBox.Text);
                        //MyEnum vrsta = (MyEnum)dialog.Izbira_Combobox.SelectedItem;

                        bool list;

                        MyEnum vrsta;

                        if (dialog.checkBox.IsChecked == true)
                        {
                            vrsta = (MyEnum)dialog.Izbira_Combobox.SelectedItem;
                            if (vrsta == MyEnum.Tabelarična)
                            {
                                MessageBox.Show("Ker je bila izbrana tabelarična funkcija, je domena med 0 in 1");
                                min = 0;
                                max = 1;
                            }
                            list = true;

                        }
                        else
                        {
                            vrsta = MyEnum.Tabelarična;
                            list = false;
                        }

                        //Izbranemu nodu dodamo otroka
                        //selectedNode.Otroci.Add(new Node(name, value, weight, min, max, vrsta));
                        selectedNode.Utez = weight;
                        selectedNode.Min = min; 
                        selectedNode.Max = max;
                        selectedNode.Ime = name;
                        selectedNode.Vrsta = vrsta;
                        selectedNode.List = list;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ojoj!");
            }
        }

        private void Uvoz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                DefaultExt = ".json"
            };

            // Show the OpenFileDialog
            bool? result = openFileDialog.ShowDialog();

            // Check if a file was selected
            if (result == true)
            {
                try
                {
                    // Read JSON data from the file
                    string json = File.ReadAllText(openFileDialog.FileName);

                    // Convert JSON string to object
                    
                    //Drevo.Items.Clear();
                    var objekt = JsonConvert.DeserializeObject<Node>(json);
                    rootNode = objekt;

                    rootNodeList.Clear();
                    rootNodeList.Add(rootNode);
                    Drevo.DataContext = rootNodeList;
                    //Drevo.DataContext = rootNodeList;


                    // Drevo.Items.Refresh();
                    //
                    // Drevo.DataContext = rootNode;

                    //OnPropertyChanged();

                    MessageBox.Show("JSON file opened successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred while opening JSON file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Izvoz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                DefaultExt = ".json"
            };

            // Show the SaveFileDialog
            bool? result = saveFileDialog.ShowDialog();

            // Check if a file was selected
            if (result == true)
            {
                try
                {
                    // Convert object to JSON string
                    string json = JsonConvert.SerializeObject(rootNode, Formatting.Indented);

                    // Save JSON data to the file
                    File.WriteAllText(saveFileDialog.FileName, json);

                    MessageBox.Show("JSON file saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred while saving JSON file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PrikaziGraf(object sender, RoutedEventArgs e)
        {
            GrafOkno graf = new GrafOkno(IzracunaneAlternative);
            graf.Show();
        }
    }

    public class Node : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float vrednost;
        public float Vrednost
        {
            get { return vrednost; }
            set
            {
                if (vrednost != value)
                {
                    vrednost = value;
                    OnPropertyChanged(nameof(Vrednost));
                }
            }
        }

        private bool list;
        public bool List
        {
            get { return list; }
            set
            {
                if (list != value)
                {
                    list = value;
                    OnPropertyChanged(nameof(List));
                }
            }
        }

        private float utez;
        public float Utez
        {
            get { return utez; }
            set
            {
                if (utez != value)
                {
                    utez = value;
                    OnPropertyChanged(nameof(Utez));
                }
            }
        }

        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    OnPropertyChanged(nameof(Ime));
                }
            }
        }

        private int min;
        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    OnPropertyChanged(nameof(Min));
                }
            }
        }

        private int max;
        public int Max
        {
            get { return max; }
            set
            {
                if (max != value)
                {
                    max = value;
                    OnPropertyChanged(nameof(Max));
                }
            }
        }

        private MyEnum vrsta;
        public MyEnum Vrsta
        {
            get { return vrsta; }
            set
            {
                if (vrsta != value)
                {
                    vrsta = value;
                    OnPropertyChanged(nameof(Vrsta));
                }
            }
        }

        //public MyEnum Vrsta = MyEnum.Tabelarična;

        public ObservableCollection<Node> Listi = new ObservableCollection<Node>();

        public ObservableCollection<Node> Otroci { get; set; }

        public Node(string ime, float value, float utez, int min, int max, bool list, MyEnum vrsta = MyEnum.Tabelarična)
        {
            Ime = ime;
            Vrednost = value;
            Utez = utez;
            Otroci = new ObservableCollection<Node>();
            Min = min;
            Max = max;
            Vrsta = vrsta;
            List = list;
        }

        public Node()
        {
            Otroci = new ObservableCollection<Node>();
        }

        //ToDo
        public void CheckSumInLevelRecursive()
        {

            if (Otroci.Count == 0)
            {
                return;
            }

            foreach (var siblingNode in Otroci)
            {
                siblingNode.CheckSumInLevelRecursive();
            }

            float sum = 0;
            foreach (var siblingNode in Otroci)
            {
                sum += siblingNode.Utez;
            }

            if (sum == 1)
            {
                return ;
            }

            throw new InvalidOperationException("Sum of sibling nodes' Utez properties is not equal to 1.");
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Izračunavanje vrednosti rekurzivno od listov navzgor - rezultat je ocena nekega parametra
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
                    return (float)Math.Log(Vrednost, Max) * Utez;
                }
                else if (Vrsta == MyEnum.Eksponentna)
                {
                    double k = 1 / Math.Pow(Max, 2);
                    float x = (float)(k * (Math.Pow(Vrednost, 2)));
                    return x * Utez;
                }
                else if (Vrsta == MyEnum.Exponentna_Padajoca)
                {
                    double k = 1 / Math.Pow(Max, 2);
                    float x = (float)(k * (Math.Pow(Vrednost, 2)));
                    var vrednost = 1 - x;
                    return vrednost * Utez;
                }
                else if (Vrsta == MyEnum.Linearna_Padajoca)
                {
                    float m = (float)(1.0 / (Max - Min));
                    float b = -m * Min;
                    float y = m * Vrednost + b;
                    var rezultat = 1 - y;
                    return rezultat * Utez;
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

        //Pridobivanje vseh Listov nekega drevesa
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
        Eksponentna,
        Linearna_Padajoca,
        Exponentna_Padajoca,
        Logaritemska_Padajoca
    }

    public class Alternativa
    {
        public float Vrednost { get; set; }
        public string Ime { get; set; }
    }

}
