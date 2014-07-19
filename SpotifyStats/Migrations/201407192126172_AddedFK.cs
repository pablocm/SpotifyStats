namespace SpotifyStats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFK : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Albums", new[] { "ArtistUri" });
            DropIndex("dbo.Tracks", new[] { "ArtistUri" });
            DropIndex("dbo.Tracks", new[] { "AlbumUri" });
            AlterColumn("dbo.Albums", "ArtistUri", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tracks", "ArtistUri", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tracks", "AlbumUri", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Albums", "ArtistUri");
            CreateIndex("dbo.Tracks", "ArtistUri");
            CreateIndex("dbo.Tracks", "AlbumUri");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", new[] { "AlbumUri" });
            DropIndex("dbo.Tracks", new[] { "ArtistUri" });
            DropIndex("dbo.Albums", new[] { "ArtistUri" });
            AlterColumn("dbo.Tracks", "AlbumUri", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tracks", "ArtistUri", c => c.String(maxLength: 128));
            AlterColumn("dbo.Albums", "ArtistUri", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tracks", "AlbumUri");
            CreateIndex("dbo.Tracks", "ArtistUri");
            CreateIndex("dbo.Albums", "ArtistUri");
        }
    }
}
