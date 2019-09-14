namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MentorshipTypesAIS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MentorshipTypes", "AISCode", c => c.Int());
        }
        
        public override void Down()
        {
        }
    }
}
