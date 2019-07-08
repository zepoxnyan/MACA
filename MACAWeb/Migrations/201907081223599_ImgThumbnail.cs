namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImgThumbnail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "ImageThumb", c => c.Binary());
        }

        public override void Down()
        {
            DropColumn("dbo.People", "ImageThumb");
        }
    }
}
