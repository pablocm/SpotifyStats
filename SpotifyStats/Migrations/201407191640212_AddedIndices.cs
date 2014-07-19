namespace SpotifyStats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIndices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "Released", c => c.Int(nullable: false));
            AlterColumn("dbo.Artists", "Name", c => c.String(maxLength: 200));
            CreateIndex("dbo.Albums", "Released");
            CreateIndex("dbo.Artists", "Name");
            CreateIndex("dbo.Tracks", "Length");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", new[] { "Length" });
            DropIndex("dbo.Artists", new[] { "Name" });
            DropIndex("dbo.Albums", new[] { "Released" });
            AlterColumn("dbo.Artists", "Name", c => c.String());
            DropColumn("dbo.Albums", "Released");
        }
    }
}
