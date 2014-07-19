using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class Artist
    {
        [Key]
        public string Uri { get; set; }
        [Index, StringLength(200)]
        public string Name { get; set; }
        public double Popularity { get; set; }

        public virtual List<Album> Albums { get; set; }
        public virtual List<Track> Tracks { get; set; }
    }
}
