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
using System.IO;

namespace konyvtar
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window
    {
        List<string> tagok = new List<string>();
        List<string> konyvek = new List<string>();
        public MainPanel()
        {
            InitializeComponent();
        }

        private void loadelements() {

            tagok.Clear();
            konyvek.Clear();
            listtagok.Items.Clear();
            listkonyv.Items.Clear();

            string filePathtagok = "tagok.txt";


            foreach (var item in File.ReadAllLines(filePathtagok))
            {
                tagok.Add(item);
            }


            string filePathkonyvek = "konyvek.txt";


            foreach (var item in File.ReadAllLines(filePathkonyvek))
            {
                konyvek.Add(item);
            }


            listtagok.Items.Clear();


            for (int i = 0; i < tagok.Count; i++)
            {
                listtagok.Items.Add(tagok[i]);
            }



            for (int i = 0; i < konyvek.Count; i++)
            {
                listkonyv.Items.Add(konyvek[i]);
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            loadelements();

        }

        private void addtag_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "tagok.txt";

            using (StreamWriter writer = File.AppendText(filePath))
            {
                string ujsor = tbx_nev.Text + ";" + tbx_email.Text + ";" + tagok.Count;
                listtagok.Items.Add(ujsor);
                writer.Write(ujsor);
            }

        }

        private void deletetag_Click(object sender, RoutedEventArgs e)
        {
            string email = listtagok.SelectedItem.ToString().Split(';')[1];
            List<string> adatok = new List<string>();
            for (int i = 0; i < listtagok.Items.Count; i++)
            {
                
                if (email != listtagok.Items[i].ToString().Split(';')[1])
                {
                    adatok.Add($"{listtagok.Items[i].ToString()}");
                }                
            }
                
            File.WriteAllLines("tagok.txt", adatok);
            listtagok.Items.Clear();
            loadelements();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "konyvek.txt";

            using (StreamWriter writer = File.AppendText(filePath))
            {
                string ujsor = szerzo.Text + ";" + cim.Text + ";"+ peldanyszam.Text + ";"+ isbn.Text +";"+kiadas.Text+ ";" + tagok.Count;
                listkonyv.Items.Add(ujsor);
                writer.Write( $"\n{ujsor}");
            }
        }

        private void Torles_delete(object sender, RoutedEventArgs e)
        {
            string isbn = listkonyv.SelectedItem.ToString().Split(';')[3];
            List<string> adatok = new List<string>();
            if (listkonyv.Items.ToString().Split('§').Length < 1)
            {
                for (int i = 0; i < listkonyv.Items.Count; i++)
                {

                    if (isbn != listkonyv.Items[i].ToString().Split(';')[3])
                    {
                        adatok.Add($"{listkonyv.Items[i].ToString()}");
                    }
                }

                File.WriteAllLines("konyvek.txt", adatok);
            }
            else
            {
                string email = listtagok.SelectedItem.ToString().Split(';')[1];
                for (int i = 0; i < listkonyv.Items.Count; i++)
                {
                    if (isbn != listkonyv.Items[i].ToString().Split(';')[3])
                    {
                        adatok.Add($"{listkonyv.Items[i].ToString()}");
                    }
                }
                foreach (var item in File.ReadAllLines("kolcsonzott.txt"))
                {
                    if (!adatok.Contains(item) && item.Split('§')[1] != email)
                    {
                        adatok.Add(item);
                    }
                }
                File.WriteAllLines("kolcsonzott.txt", adatok);
            }

            listkonyv.Items.Clear();
            loadelements();
        }

        private void kolcsonzes_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "kolcsonzott.txt";

            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"{listkonyv.SelectedItem.ToString()}§{listtagok.SelectedItem.ToString().Split(';')[1]}");
            }
        }

        private void kikolcsonzottek_Click(object sender, RoutedEventArgs e)
        {
            listkonyv.Items.Clear();

            if (kikolcsonzottek.Content.ToString() != "Kikölcsönzött könyve")
            {
                loadelements();
                kikolcsonzottek.Content = "Kikölcsönzött könyve";
            }
            else
            {
                foreach (var item in File.ReadAllLines("kolcsonzott.txt"))
                {
                    if (item.Split('§')[1] == listtagok.SelectedItem.ToString().Split(';')[1])
                    {
                        listkonyv.Items.Add(item);
                    }
                }
                kikolcsonzottek.Content = "Vissza";
            }
            
        }
    }
}
