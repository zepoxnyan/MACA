namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SL_v3 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SocialLinks", "PersonID");
            AddForeignKey("dbo.SocialLinks", "PersonID", "dbo.People", "PersonID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialLinks", "PersonID", "dbo.People");
            DropIndex("dbo.SocialLinks", new[] { "PersonID" });
        }
    }
}
