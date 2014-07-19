using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Entities
{
    public class Artist
    {
        public string Name { get; internal set; }
        public double Popularity { get; internal set; }
        public string Uri { get; internal set; }

        public IEnumerable<Album> Albums { get; set; }
    }
}
