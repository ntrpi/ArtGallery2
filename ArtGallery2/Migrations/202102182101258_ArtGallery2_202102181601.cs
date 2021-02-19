namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102181601 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "imageName", c => c.String());
            AddColumn("dbo.Images", "imagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "imagePath");
            DropColumn("dbo.Images", "imageName");
        }
    }
}
