namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102102023 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        statusId = c.Int(nullable: false, identity: true),
                        statusName = c.String(),
                    })
                .PrimaryKey(t => t.statusId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Status");
        }
    }
}
