namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102102006 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Techniques",
                c => new
                    {
                        techniqueId = c.Int(nullable: false, identity: true),
                        techniqueName = c.String(),
                    })
                .PrimaryKey(t => t.techniqueId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Techniques");
        }
    }
}
