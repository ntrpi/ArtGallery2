namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102102028 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pieces", "mediaId", c => c.Int(nullable: false));
            AddColumn("dbo.Pieces", "statusId", c => c.Int(nullable: false));
            AddColumn("dbo.Pieces", "techniqueId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pieces", "mediaId");
            CreateIndex("dbo.Pieces", "statusId");
            CreateIndex("dbo.Pieces", "techniqueId");
            AddForeignKey("dbo.Pieces", "mediaId", "dbo.Media", "mediaId", cascadeDelete: true);
            AddForeignKey("dbo.Pieces", "statusId", "dbo.Status", "statusId", cascadeDelete: true);
            AddForeignKey("dbo.Pieces", "techniqueId", "dbo.Techniques", "techniqueId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pieces", "techniqueId", "dbo.Techniques");
            DropForeignKey("dbo.Pieces", "statusId", "dbo.Status");
            DropForeignKey("dbo.Pieces", "mediaId", "dbo.Media");
            DropIndex("dbo.Pieces", new[] { "techniqueId" });
            DropIndex("dbo.Pieces", new[] { "statusId" });
            DropIndex("dbo.Pieces", new[] { "mediaId" });
            DropColumn("dbo.Pieces", "techniqueId");
            DropColumn("dbo.Pieces", "statusId");
            DropColumn("dbo.Pieces", "mediaId");
        }
    }
}
