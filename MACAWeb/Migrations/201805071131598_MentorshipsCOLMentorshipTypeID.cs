namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MentorshipsCOLMentorshipTypeID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mentorships", "MentorshipTypeID", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Mentorships", "MentorshipTypeID", "dbo.MentorshipTypes", "MentorshipTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
