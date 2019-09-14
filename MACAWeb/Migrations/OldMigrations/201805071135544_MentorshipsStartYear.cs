namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MentorshipsStartYear : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Mentorships", "Year", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
