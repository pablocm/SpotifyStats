using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStats.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasMany<Album>(a => a.Albums)
                .WithRequired(a => a.Artist).HasForeignKey(a => a.ArtistUri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Artist>().HasMany<Track>(a => a.Tracks)
                .WithRequired(t => t.Artist).HasForeignKey(t => t.ArtistUri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Album>().HasMany<Track>(a => a.Tracks)
                .WithRequired(t => t.Album).HasForeignKey(t => t.AlbumUri)
                .WillCascadeOnDelete(false);
        }
    }
}
