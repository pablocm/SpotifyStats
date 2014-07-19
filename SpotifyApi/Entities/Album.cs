using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Entities
{
    public class Album
    {
        public string Name { get; internal set; }
        public double Popularity { get; internal set; }
        public int Released { get; internal set; }
        public string ArtistUri { get; internal set; }
        public string Uri { get; internal set; }

        public IEnumerable<Track> Tracks { get; set; }
    }
}
