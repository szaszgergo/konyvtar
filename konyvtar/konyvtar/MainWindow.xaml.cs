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
using System.IO;
namespace konyvtar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> felhasznalok = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
                    
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
         
            string filePath = "munkatarsak.txt";
           

            foreach (var item in File.ReadAllLines(filePath))
            {
                felhasznalok.Add(item);
            }
            
        }

        private void kuld_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < felhasznalok.Count; i++)
            {
                if (felhasznalok[i].Split(';')[0].ToString() == felhasznalo.Text)
                {
                    if (felhasznalok[i].Split(';')[2].ToString() == jelszo.Text)
                    {
                        MessageBox.Show("Bentvagy");
                    }
                }
            }
        }

        private void jelszo_KeyUp(object sender, KeyEventArgs e)
        {
            if (jelszo.Text != "jelszo")
            {
                jelszo.Foreground = Brushes.Black;
                jelszo.Foreground.Opacity = 0.9;
                
            }

        }
    }
}
