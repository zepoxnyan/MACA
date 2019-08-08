namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SL_v2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SocialLinks", "SocialLinkTypeID");
            AddForeignKey("dbo.SocialLinks", "SocialLinkTypeID", "dbo.SocialLinkTypes", "SocialLinkTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialLinks", "SocialLinkTypeID", "dbo.SocialLinkTypes");
            DropIndex("dbo.SocialLinks", new[] { "SocialLinkTypeID" });
        }
    }
}
