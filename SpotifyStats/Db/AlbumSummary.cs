using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class AlbumSummary
    {
        public Album Album { get; set; }
        public double AveragePopularity { get; set; }
        public Track LongestTrack { get; set; }

        public string Name
        {
            get
            {
                return Album.LongName;
            }
        }

        public string LongestTrackName
        {
            get
            {
                return LongestTrack.LongName;
            }
        }

    }
}
