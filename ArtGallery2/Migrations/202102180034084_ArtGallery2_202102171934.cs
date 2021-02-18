namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102171934 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pieces", "mediaId");
            DropColumn("dbo.Pieces", "statusId");
            DropColumn("dbo.Pieces", "techniqueId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pieces", "techniqueId", c => c.Int(nullable: false));
            AddColumn("dbo.Pieces", "statusId", c => c.Int(nullable: false));
            AddColumn("dbo.Pieces", "mediaId", c => c.Int(nullable: false));
        }
    }
}
