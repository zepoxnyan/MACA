namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SL_addLogo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SocialLinkTypes", "Logo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SocialLinkTypes", "Logo");
        }
    }
}
