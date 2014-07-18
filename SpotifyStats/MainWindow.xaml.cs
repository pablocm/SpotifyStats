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
using SpotifyApi;
using SpotifyApi.Entities;

namespace SpotifyStats
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

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = searchTextBox.Text;

            var api = new Spotify();
            var artists = api.FindArtist(query);

            resultsListBox.ItemsSource = artists;
        }
    }
}
