namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102191710 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "isMainImage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "isMainImage");
        }
    }
}
