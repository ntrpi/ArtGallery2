namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102101920 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        imageId = c.Int(nullable: false, identity: true),
                        imagePath = c.String(),
                        pieceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.imageId)
                .ForeignKey("dbo.Pieces", t => t.pieceId, cascadeDelete: true)
                .Index(t => t.pieceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "pieceId", "dbo.Pieces");
            DropIndex("dbo.Images", new[] { "pieceId" });
            DropTable("dbo.Images");
        }
    }
}
