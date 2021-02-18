namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102171106 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pieces", "mediaId", "dbo.Media");
            DropForeignKey("dbo.Pieces", "statusId", "dbo.Status");
            DropForeignKey("dbo.Pieces", "techniqueId", "dbo.Techniques");
            DropIndex("dbo.Pieces", new[] { "mediaId" });
            DropIndex("dbo.Pieces", new[] { "statusId" });
            DropIndex("dbo.Pieces", new[] { "techniqueId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Pieces", "techniqueId");
            CreateIndex("dbo.Pieces", "statusId");
            CreateIndex("dbo.Pieces", "mediaId");
            AddForeignKey("dbo.Pieces", "techniqueId", "dbo.Techniques", "techniqueId", cascadeDelete: true);
            AddForeignKey("dbo.Pieces", "statusId", "dbo.Status", "statusId", cascadeDelete: true);
            AddForeignKey("dbo.Pieces", "mediaId", "dbo.Media", "mediaId", cascadeDelete: true);
        }
    }
}
