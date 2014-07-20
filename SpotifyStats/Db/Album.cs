using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index]
        public int Released { get; set; }

        public Artist Artist { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }

        public string LongName
        {
            get
            {
                return String.Format("{0} ({1})", Name, Released);
            }
        }
    }
}
