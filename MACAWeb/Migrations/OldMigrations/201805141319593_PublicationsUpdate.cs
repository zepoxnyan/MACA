namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publications", "TitleEN", c => c.String(nullable: true));
            AddColumn("dbo.Publications", "KeywordsEN", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
