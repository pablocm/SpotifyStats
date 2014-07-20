using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /// Returns a summary of the artists albums, containing their average popularity
        /// and its longest track.
        /// </summary>
        /// <param name="artistUri">The spotify artist URI</param>
        public async Task<List<AlbumSummary>> GetArtistAlbumsSummary(string artistUri)
        {
            // This Linq expression compiles to a single Sql query.
            var summary = from al in context.Albums
                          where al.ArtistUri == artistUri
                          orderby al.Released descending
                          select new AlbumSummary
                          {
                              Album = al,
                              AveragePopularity = (int)(al.Tracks.Average(t => t.Popularity) * 100),
                              LongestTrack = al.Tracks.OrderByDescending(t => t.Length).FirstOrDefault()
                          };
            return await summary.ToListAsync();
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
            };
            context.Artists.Add(artist);

            // Create the albums
            foreach (var spotifyAlbum in spotifyArtist.Albums)
            {
                if (context.Albums.Where(a => a.Uri == spotifyAlbum.Uri).Count() > 0)
                    continue;

                Album album = new Album
                {
                    Uri = spotifyAlbum.Uri,
                    Name = spotifyAlbum.Name,
                    Released = spotifyAlbum.Released,
                    ArtistUri = artist.Uri
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
                        Length = spotifyTrack.Length,
                        AlbumUri = album.Uri,
                        ArtistUri = artist.Uri
                    };
                    context.Tracks.Add(track);
                }
                // Commit album
                await context.SaveChangesAsync();
            }
        }
    }
}
