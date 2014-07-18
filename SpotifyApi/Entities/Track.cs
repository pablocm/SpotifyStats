using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Entities
{
    public class Track
    {
        public string Name { get; internal set; }
        public double Popularity { get; internal set; }
        public string ArtistUri { get; internal set; } // TODO: Tracks with several artists.
        public string AlbumUri { get; internal set; }
        public int TrackNumber { get; internal set; }
        public double Length { get; internal set; }
        public string Uri { get; internal set; }
    }
}
