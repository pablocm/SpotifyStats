using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SpotifyApi.Entities;

namespace SpotifyApi
{
    public class Spotify
    {
        private const string apiUrl = "https://ws.spotify.com/";
        private const string artistSearchFormat = "search/1/artist.xml?q={0}";
        private const string albumSearchFormat = "search/1/album.xml?q={0}";
        private const string trackSearchFormat = "search/1/track.xml?q={0}";
        private const string lookupExtrasFormat = "lookup/1/?uri={0}&extras={1}";
        private const string ns = "{http://www.spotify.com/ns/music/1}";

        public Spotify()
        {
        }

        /// <summary>
        /// Requests data through the Spotify Metadata API and returns the XML response.
        /// </summary>
        /// <param name="requestString">The request URL string.</param>
        private async Task<XDocument> DoApiRequestAsync(string requestUrl)
        {
            WebRequest request = WebRequest.Create(apiUrl + requestUrl);
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("API not available. Status code: " + response.StatusCode);

                return XDocument.Load(response.GetResponseStream());
            }
        }

        /// <summary>
        /// Search for artists using the query string.
        /// </summary>
        /// <param name="query">The search keyword(s).</param>
        public async Task<IEnumerable<Artist>> FindArtistAsync(string query)
        {
            XDocument document = await DoApiRequestAsync(String.Format(artistSearchFormat, query));
            var artists = from artist in document.Descendants(ns + "artist")
                          select new Artist
                          {
                              Name = artist.Element(ns + "name").Value,
                              Popularity = Double.Parse(artist.Element(ns + "popularity").Value),
                              Uri = artist.Attribute("href") != null ? artist.Attribute("href").Value : String.Empty
                          };

            return artists;
        }

        /// <summary>
        /// Search for albums using the query string.
        /// </summary>
        /// <param name="query">The search keyword(s).</param>
        public async Task<IEnumerable<Album>> FindAlbumAsync(string query)
        {
            XDocument document = await DoApiRequestAsync(String.Format(albumSearchFormat, query));
            var albums = from album in document.Descendants(ns + "album")
                         select new Album
                         {
                             Name = album.Element(ns + "name").Value,
                             Popularity = Double.Parse(album.Element(ns + "popularity").Value),
                             ArtistUri = album.Descendants(ns + "artist").First().Attribute("href").Value,
                             Uri = album.Attribute("href") != null ? album.Attribute("href").Value : String.Empty
                         };

            return albums;
        }

        /// <summary>
        /// Search for tracks using the query string.
        /// </summary>
        /// <param name="query">The search keyword(s).</param>
        public async Task<IEnumerable<Track>> FindTrackAsync(string query)
        {
            XDocument document = await DoApiRequestAsync(String.Format(trackSearchFormat, query));
            var tracks = from track in document.Descendants(ns + "track")
                         select new Track
                         {
                             Name = track.Element(ns + "name").Value,
                             Popularity = Double.Parse(track.Element(ns + "popularity").Value),
                             ArtistUri = track.Descendants(ns + "artist").First().Attribute("href").Value,
                             AlbumUri = track.Descendants(ns + "album").First().Attribute("href").Value,
                             TrackNumber = Int32.Parse(track.Element(ns + "track-number").Value),
                             Length = Double.Parse(track.Element(ns + "length").Value),
                             Uri = track.Attribute("href") != null ? track.Attribute("href").Value : String.Empty
                         };

            return tracks;
        }

        /// <summary>
        /// Look up detailed artist info, including albums and tracks, from the URI.
        /// </summary>
        /// <param name="artistUri">Spotify artist URI</param>
        public async Task<Artist> LookupArtistAsync(string artistUri)
        {
            XDocument document = await DoApiRequestAsync(String.Format(lookupExtrasFormat, artistUri, "album"));
            var detailedArtist = (from artist in document.Descendants(ns + "artist")
                                 select new Artist
                                 {
                                     Name = artist.Element(ns + "name").Value,
                                     //Popularity = Double.Parse(artist.Element(ns + "popularity").Value),
                                     Uri = artistUri,
                                     //Albums = from album in artist.Element(ns + "albums").Descendants(ns + "album")
                                     //         select LookupAlbumAsync(album.Attribute("href").Value).Result
                                 }).First();

            var albumUris = from album in document.Descendants(ns + "artist").Elements(ns + "albums").Descendants(ns + "album")
                            select album.Attribute("href").Value;
            
            var albums = new List<Album>();
            foreach (var albumUri in albumUris)
                albums.Add(await LookupAlbumAsync(albumUri));
            detailedArtist.Albums = albums;

            return detailedArtist;
        }

        /// <summary>
        /// Look up detailed album info from the URI.
        /// </summary>
        /// <param name="albumUri">Spotify album URI</param>
        public async Task<Album> LookupAlbumAsync(string albumUri)
        {
            XDocument document = await DoApiRequestAsync(String.Format(lookupExtrasFormat, albumUri, "trackdetail"));
            var albums = from album in document.Descendants(ns + "album")
                         select new Album
                         {
                             Name = album.Element(ns + "name").Value,
                             //Popularity = Double.Parse(album.Element(ns + "popularity").Value),
                             Released = Int32.Parse(album.Element(ns + "released").Value),
                             ArtistUri = album.Elements(ns + "artist").First().Attribute("href") != null ? album.Elements(ns + "artist").First().Attribute("href").Value : String.Empty,
                             Uri = albumUri,
                             Tracks = from track in album.Element(ns + "tracks").Elements()
                                      select new Track
                                      {
                                          Name = track.Element(ns + "name").Value,
                                          Popularity = Double.Parse(track.Element(ns + "popularity").Value),
                                          ArtistUri = track.Elements(ns + "artist").First().Attribute("href") != null ? track.Elements(ns + "artist").First().Attribute("href").Value : String.Empty,
                                          AlbumUri = albumUri,
                                          TrackNumber = Int32.Parse(track.Element(ns + "track-number").Value),
                                          Length = Double.Parse(track.Element(ns + "length").Value),
                                          Uri = track.Attribute("href").Value
                                      }
                         };

            return albums.FirstOrDefault();
        }
    }
}
