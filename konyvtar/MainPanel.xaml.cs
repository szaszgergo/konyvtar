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
using System.Security.Cryptography;

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
            List<string> adatok = new List<string>();
            if (listkonyv.Items.ToString().Split('§').Length < 1)
            {
                string isbn = listkonyv.SelectedItem.ToString().Split(';')[3];
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
                string isbn = listkonyv.SelectedItem.ToString().Split(';')[0];
                string azonosito = listtagok.SelectedItem.ToString().Split(';')[2];

                List<string> konyvek = new List<string>();

                foreach (var item in File.ReadAllLines("konyvek.txt"))
                {
                    if (item.ToString().Split(';')[3] != isbn.Split('§')[1])
                    {
                        konyvek.Add(item);
                    }
                    else
                    {
                        konyvek.Add($"{item.ToString().Split(';')[0]};{item.ToString().Split(';')[1]};{int.Parse(item.ToString().Split(';')[2]) + 1};{item.ToString().Split(';')[3]};{item.ToString().Split(';')[4]}");
                    }
                }
                File.WriteAllLines("konyvek.txt", konyvek);

                for (int i = 0; i < listkonyv.Items.Count; i++)
                {
                    if (isbn != listkonyv.Items[i].ToString().Split(';')[0])
                    {
                        adatok.Add($"{listkonyv.Items[i].ToString()}");
                    }
                }
                foreach (var item in File.ReadAllLines("kolcsonzott.txt"))
                {
                    if (!adatok.Contains(item) && item.Split('§')[0] != azonosito)
                    {
                        adatok.Add(item);
                    }
                }
                File.WriteAllLines("kolcsonzott.txt", adatok);

                kikolcsonzottek.Content = "Kikölcsönzött könyvek";
            }

            listkonyv.Items.Clear();
            loadelements();
        }

        private void kolcsonzes_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "kolcsonzott.txt";
            List<string> konyvei = new List<string>();
            int darabszam = int.Parse(listkonyv.SelectedItem.ToString().Split(';')[2]);

            foreach (var item in File.ReadAllLines("kolcsonzott.txt"))
            {
                if (item.Contains($"{listtagok.SelectedItem.ToString().Split(';')[2]}§"))
                {
                    konyvei.Add(item);
                }
            }
            if (konyvei.Count <= 3)
            {
                if (darabszam != 0)
                {
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine($"{listtagok.SelectedItem.ToString().Split(';')[2]}§{listkonyv.SelectedItem.ToString().Split(';')[3]};{DateTime.Now};{DateTime.Now.AddDays(7)}");
                    }
                    List<string> konyvek = new List<string>();
                    konyvek.Add($"{listkonyv.SelectedItem.ToString().Split(';')[0]};{listkonyv.SelectedItem.ToString().Split(';')[1]};{int.Parse(listkonyv.SelectedItem.ToString().Split(';')[2]) - 1};{listkonyv.SelectedItem.ToString().Split(';')[3]};{listkonyv.SelectedItem.ToString().Split(';')[4]}");
                    foreach (var item in File.ReadAllLines("konyvek.txt"))
                    {
                        if (item.ToString().Split(';')[3] != listkonyv.SelectedItem.ToString().Split(';')[3])
                        {
                            konyvek.Add(item);
                        }
                    }
                    File.WriteAllLines("konyvek.txt", konyvek);
                }
                
            }
            else
            {
                MessageBox.Show("A felhasználónak már megvan a 4 kikölcsönzött könyve!");
            }
            loadelements();
        }

        private void kikolcsonzottek_Click(object sender, RoutedEventArgs e)
        {
            listkonyv.Items.Clear();

            if (kikolcsonzottek.Content.ToString() != "Kikölcsönzött könyvek")
            {
                loadelements();
                kikolcsonzottek.Content = "Kikölcsönzött könyvek";
            }
            else
            {
                foreach (var item in File.ReadAllLines("kolcsonzott.txt"))
                {
                    if (item.Split('§')[0] == listtagok.SelectedItem.ToString().Split(';')[2])
                    {
                        listkonyv.Items.Add(item);
                    }
                }
                kikolcsonzottek.Content = "Vissza";
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Kereses_Click(object sender, RoutedEventArgs e)
        {
            string keresettNev = tbx_nev.Text;
            string KeresettEmail = tbx_email.Text;
            List<string> megegyezoTagok = new List<string>();

            // Ha üres a keresési mező, írassa ki a teljes listát
            if (string.IsNullOrEmpty(keresettNev) && string.IsNullOrEmpty(KeresettEmail))
            {
                Kereses.Content = "Keresés";

                foreach (var item in File.ReadAllLines("tagok.txt"))
                {
                    megegyezoTagok.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < listtagok.Items.Count; i++)
                {
                    string[] sorElemek = listtagok.Items[i].ToString().Split(';');
                    string elsoElem = sorElemek[0].Trim();
                    string masodikElem = sorElemek[1].Trim();

                    if ((string.IsNullOrEmpty(keresettNev) || elsoElem == keresettNev) &&
                        (string.IsNullOrEmpty(KeresettEmail) || masodikElem == KeresettEmail))
                    {
                        megegyezoTagok.Add(listtagok.Items[i].ToString());
                    }
                }

                Kereses.Content = "Vissza";
            }

            // Törlés és újratöltés
            listtagok.Items.Clear();
            megegyezoTagok.ForEach(tag => listtagok.Items.Add(tag));
            tbx_nev.Text = "";
            tbx_email.Text = "";

        }

        private void Kereses_Click_2(object sender, RoutedEventArgs e)
        {
            string keresettszerzo = szerzo.Text;
            string Keresettcim = cim.Text;
            List<string> megegyezoTagok = new List<string>();

            // Ha üres a keresési mező, írassa ki a teljes listát
            if (string.IsNullOrEmpty(keresettszerzo) && string.IsNullOrEmpty(Keresettcim))
            {
                Kereses.Content = "Keresés";

                foreach (var item in File.ReadAllLines("konyvek.txt"))
                {
                    megegyezoTagok.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < listkonyv.Items.Count; i++)
                {
                    string[] sorElemek = listkonyv.Items[i].ToString().Split(';');
                    string elsoElem = sorElemek[0].Trim();
                    string masodikElem = sorElemek[1].Trim();

                    if ((string.IsNullOrEmpty(keresettszerzo) || elsoElem == keresettszerzo) &&
                        (string.IsNullOrEmpty(Keresettcim) || masodikElem == Keresettcim))
                    {
                        megegyezoTagok.Add(listkonyv.Items[i].ToString());
                    }
                }

                Kereses.Content = "Vissza";
            }

            // Törlés és újratöltés
            listkonyv.Items.Clear();
            megegyezoTagok.ForEach(tag => listkonyv.Items.Add(tag));
            szerzo.Text = "";
            cim.Text = "";
        }
    }
}
