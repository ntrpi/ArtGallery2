namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery_202102231845 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PieceTechniques",
                c => new
                    {
                        pieceTechniqueId = c.Int(nullable: false, identity: true),
                        pieceId = c.Int(nullable: false),
                        techniqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.pieceTechniqueId)
                .ForeignKey("dbo.Pieces", t => t.pieceId, cascadeDelete: true)
                .ForeignKey("dbo.Techniques", t => t.techniqueId, cascadeDelete: true)
                .Index(t => t.pieceId)
                .Index(t => t.techniqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PieceTechniques", "techniqueId", "dbo.Techniques");
            DropForeignKey("dbo.PieceTechniques", "pieceId", "dbo.Pieces");
            DropIndex("dbo.PieceTechniques", new[] { "techniqueId" });
            DropIndex("dbo.PieceTechniques", new[] { "pieceId" });
            DropTable("dbo.PieceTechniques");
        }
    }
}
