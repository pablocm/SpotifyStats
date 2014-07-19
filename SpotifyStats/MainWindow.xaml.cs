using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
using SpotifyStats.Db;

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
        /// Connects to database and displays downloaded artists.
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (AppDbContext db = new AppDbContext())
            {
                downloadedListBox.ItemsSource = await db.Artists.OrderBy(a => a.Name).ToListAsync();
            }
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
            // TODO
            MessageBox.Show(btn.Tag.ToString());
        }

        /// <summary>
        /// Load artist summary and stats into view.
        /// </summary>
        private void downloadedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var artist = (Artist)e.AddedItems[0];
            // TODO
            MessageBox.Show(artist.Name);
        }
    }
}
