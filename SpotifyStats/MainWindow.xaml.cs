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
        int workingTasks = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Keeps track of how many works are in progress and shows/hides the progress bar accordingly.
        /// </summary>
        private void WorkStarted()
        {
            workingTasks++;
            if (workingTasks > 0)
                progressBar.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Keeps track of how many works are in progress and shows/hides the progress bar accordingly.
        /// </summary>
        private void WorkFinished()
        {
            workingTasks--;
            if (workingTasks == 0)
                progressBar.Visibility = System.Windows.Visibility.Hidden;
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
            WorkStarted();
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
                WorkFinished();
                searchInProgress = false;
            }
        }

        /// <summary>
        /// Downloads the currently selected artist's data.
        /// </summary>
        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var artistUri = btn.Tag.ToString();

            WorkStarted();
            btn.IsEnabled = false;
            using (var db = new AppDbContext())
            {
                // Check if its already in DB.
                if ((await db.Artists.FindAsync(artistUri)) == null)
                {
                    // Download data
                    var api = new Spotify();
                    var artist = await api.LookupArtistAsync(btn.Tag.ToString());

                    // Save the data
                    ArtistRepository ar = new ArtistRepository(db);
                    await ar.SaveSpotifyArtistAsync(artist);

                    // Refresh the viewlist
                    downloadedListBox.ItemsSource = await db.Artists.OrderBy(a => a.Name).ToListAsync();
                }
            }
            WorkFinished();
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
