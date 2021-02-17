namespace ArtGallery2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtGallery2_202102162053 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pieces", "pieceName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pieces", "pieceName", c => c.String());
        }
    }
}
