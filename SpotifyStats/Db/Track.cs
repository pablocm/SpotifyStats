using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class Track
    {
        [Key]
        public string Uri { get; set; }
        public string Name { get; set; }
        public double Popularity { get; set; }
        public string ArtistUri { get; set; } // TODO: Tracks with several artists.
        public string AlbumUri { get; set; }
        public int TrackNumber { get; set; }
        [Index]
        public double Length { get; set; }

        public Artist Artist { get; set; }
        public Album Album { get; set; }
    }
}
