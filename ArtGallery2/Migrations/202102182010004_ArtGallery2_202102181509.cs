namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102181509 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "imageExt", c => c.String());
            DropColumn("dbo.Images", "imagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "imagePath", c => c.String());
            DropColumn("dbo.Images", "imageExt");
        }
    }
}
