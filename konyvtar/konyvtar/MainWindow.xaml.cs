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
        public MainWindow()
        {
            InitializeComponent();
                    
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string filePath = "munkatarsak.txt";

            // Open the file for reading using a StreamReader
            using (StreamReader sr = new StreamReader(filePath))
            {
                // Read the entire contents of the file
                string fileContents = sr.ReadToEnd();
                List<string> beolvas = new List<string>();
                foreach (var item in fileContents)
                {
                    beolvas.Add(item.ToString());
                }
                // Output the contents to the console

             
               
            }
        }

        private void kuld_Click(object sender, RoutedEventArgs e)
        {
          
        }

    
    }
}
