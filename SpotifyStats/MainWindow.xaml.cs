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
            {
                progressBar.Visibility = System.Windows.Visibility.Hidden;
                statusText.Text = "Ready.";
            }
        }

        /// <summary>
        /// Connects to database and displays downloaded artists.
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the listbox with the currently downloaded artists.
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    downloadedListBox.ItemsSource = await db.Artists.OrderBy(a => a.Name).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occurred while connecting to the Database: " + ex.Message);
                this.Close();
            }
        }

        /// <summary>
        /// Updates the search results.
        /// </summary>
        private async void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            await UpdateSearchResults();
        }

        /// <summary>
        /// Updates the search results when the user presses Enter.
        /// </summary>
        private async void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                await UpdateSearchResults();
        }

        /// <summary>
        /// Updates the search results with the query.
        /// </summary>
        private async Task UpdateSearchResults()
        {
            string query = searchTextBox.Text.Trim();
            if (String.IsNullOrEmpty(query))
                return;

            WorkStarted();
            try
            {
                var api = new Spotify();
                var artists = await api.FindArtistAsync(query);

                resultsListBox.ItemsSource = artists;
            }
            catch (WebException ex)
            {
                MessageBox.Show("Web error while retrieving data: " + ex.Message);
            }
            finally
            {
                WorkFinished();
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
                    try
                    {
                        statusText.Text = String.Format("{0} downloads pending...", workingTasks);
                        var api = new Spotify();
                        var artist = await api.LookupArtistAsync(btn.Tag.ToString());

                        // Save the data
                        statusText.Text = String.Format("Saving {0} into database...", artist.Name);
                        ArtistRepository ar = new ArtistRepository(db);
                        await ar.SaveSpotifyArtistAsync(artist);

                        // Refresh the viewlist
                        downloadedListBox.ItemsSource = await db.Artists.OrderBy(a => a.Name).ToListAsync();
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show("Web error while retrieving data: " + ex.Message);
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Error while saving to DB: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            WorkFinished();
        }

        /// <summary>
        /// Load artist summary and stats into view.
        /// </summary>
        private async void downloadedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var artist = (Artist)e.AddedItems[0];

                artistNameTextBlock.Text = artist.Name;
                albumsListBox.ItemsSource = null;
                using (var db = new AppDbContext())
                {
                    var repository = new ArtistRepository(db);

                    var albumSummary = await repository.GetArtistAlbumsSummary(artist.Uri);
                    albumsListBox.ItemsSource = albumSummary;
                }
            }
        }

        /// <summary>
        /// Load album's tracks.
        /// </summary>
        private async void albumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var albumSummary = (AlbumSummary)e.AddedItems[0];

                tracksListBox.ItemsSource = null;
                using (var db = new AppDbContext())
                {
                    var tracks = await db.Tracks.Where(t => t.AlbumUri == albumSummary.Album.Uri).OrderBy(t => t.TrackNumber).ToListAsync();
                    tracksListBox.ItemsSource = tracks;
                }
            }
        }
    }
}
