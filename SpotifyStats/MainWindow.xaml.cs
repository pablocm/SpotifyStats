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
using System.Net;

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
        }

        /// <summary>
        /// Updates the search results while user enters text.
        /// </summary>
        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = searchTextBox.Text.Trim();
            if (String.IsNullOrEmpty(query) || searchInProgress)
                return;

            searchInProgress = true;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            try
            {
                var api = new Spotify();
                var artists = await api.FindArtistAsync(query);

                resultsListBox.ItemsSource = artists;
            }
            catch(WebException ex)
            {
                MessageBox.Show("Error while retrieving data: " + ex.Message);
            }
            finally
            {
                progressBar.Visibility = System.Windows.Visibility.Hidden;
                searchInProgress = false;
            }
        }

        /// <summary>
        /// Downloads the currently selected artist's data.
        /// </summary>
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            MessageBox.Show(btn.Tag.ToString());
        }
    }
}
