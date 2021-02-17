namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102101946 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        mediaId = c.Int(nullable: false, identity: true),
                        mediaName = c.String(),
                    })
                .PrimaryKey(t => t.mediaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Media");
        }
    }
}
