namespace SpotifyStats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Uri = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Popularity = c.Double(nullable: false),
                        ArtistUri = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Uri)
                .ForeignKey("dbo.Artists", t => t.ArtistUri)
                .Index(t => t.ArtistUri);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Uri = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Popularity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Uri);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Uri = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Popularity = c.Double(nullable: false),
                        ArtistUri = c.String(maxLength: 128),
                        AlbumUri = c.String(maxLength: 128),
                        TrackNumber = c.Int(nullable: false),
                        Length = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Uri)
                .ForeignKey("dbo.Albums", t => t.AlbumUri)
                .ForeignKey("dbo.Artists", t => t.ArtistUri)
                .Index(t => t.ArtistUri)
                .Index(t => t.AlbumUri);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tracks", "ArtistUri", "dbo.Artists");
            DropForeignKey("dbo.Tracks", "AlbumUri", "dbo.Albums");
            DropForeignKey("dbo.Albums", "ArtistUri", "dbo.Artists");
            DropIndex("dbo.Tracks", new[] { "AlbumUri" });
            DropIndex("dbo.Tracks", new[] { "ArtistUri" });
            DropIndex("dbo.Albums", new[] { "ArtistUri" });
            DropTable("dbo.Tracks");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
