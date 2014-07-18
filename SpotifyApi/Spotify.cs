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
        private const string ns = "{http://www.spotify.com/ns/music/1}";

        public Spotify()
        {
        }

        /// <summary>
        /// Requests data through the Spotify Metadata API and returns the XML response.
        /// </summary>
        /// <param name="requestString">The request URL string.</param>
        private XDocument DoApiRequest(string requestUrl)
        {
            WebRequest request = WebRequest.Create(apiUrl + requestUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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
        public IEnumerable<Artist> FindArtist(string query)
        {
            XDocument document = DoApiRequest(String.Format(artistSearchFormat, query));
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
        public IEnumerable<Album> FindAlbum(string query)
        {
            XDocument document = DoApiRequest(String.Format(albumSearchFormat, query));
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
        public IEnumerable<Track> FindTrack(string query)
        {
            XDocument document = DoApiRequest(String.Format(trackSearchFormat, query));
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
    }
}
