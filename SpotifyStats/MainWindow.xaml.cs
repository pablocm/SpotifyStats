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
        bool searchInProgress = false;

        public MainWindow()
        {
            InitializeComponent();
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = searchTextBox.Text.Trim();
            if (String.IsNullOrEmpty(query) || searchInProgress)
                return;

            searchInProgress = true;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            var api = new Spotify();
            var artists = await api.FindArtistAsync(query);

            resultsListBox.ItemsSource = artists;
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            searchInProgress = false;
        }
    }
}
