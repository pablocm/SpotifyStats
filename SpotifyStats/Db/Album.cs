using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class Album
    {
        [Key]
        public string Uri { get; set; }
        public string Name { get; set; }
        public double Popularity { get; set; }
        public string ArtistUri { get; set; }

        public Artist Artist { get; set; }
        public virtual List<Track> Tracks { get; set; }
    }
}
