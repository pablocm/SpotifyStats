using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class ArtistRepository
    {
        private AppDbContext context;

        public ArtistRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Saves a Spotify API's artist into the database.
        /// </summary>
        /// <param name="artist">The artist data.</param>
        public async Task SaveSpotifyArtistAsync(SpotifyApi.Entities.Artist spotifyArtist)
        {
            Artist artist = new Artist
            {
                Uri = spotifyArtist.Uri,
                Name = spotifyArtist.Name,
                Popularity = spotifyArtist.Popularity,
                Albums = new List<Album>(),
                Tracks = new List<Track>()
            };
            context.Artists.Add(artist);

            // Create the albums
            foreach (var spotifyAlbum in spotifyArtist.Albums)
            {
                Album album = new Album
                {
                    Uri = spotifyAlbum.Uri,
                    Name = spotifyAlbum.Name,
                    Released = spotifyAlbum.Released,
                    Tracks = new List<Track>()
                };
                context.Albums.Add(album);

                // Create the tracks
                foreach (var spotifyTrack in spotifyAlbum.Tracks)
                {
                    Track track = new Track
                    {
                        Uri = spotifyTrack.Uri,
                        Name = spotifyTrack.Name,
                        Popularity = spotifyTrack.Popularity,
                        TrackNumber = spotifyTrack.TrackNumber,
                        Length = spotifyTrack.Length
                    };
                    context.Tracks.Add(track);

                    album.Tracks.Add(track);
                    artist.Tracks.Add(track);
                }
                artist.Albums.Add(album);
            }

            // Commit
            await context.SaveChangesAsync();
        }
    }
}
