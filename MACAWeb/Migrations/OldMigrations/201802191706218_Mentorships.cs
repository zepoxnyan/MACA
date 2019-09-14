namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mentorships : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Mentorships", t => new
            {
                MentorshipID = t.Guid(nullable: false),
                ThesisTypeID = t.Guid(nullable: false),
                PersonID = t.Guid(nullable: false),

                Remarks = t.String(nullable: true),
                Student = t.String(nullable: false),
                ThesisTitle = t.String(nullable: false),
                Year = t.Int(nullable: false),
                Semester = t.Int(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Mentorships", "MentorshipID");
            AddForeignKey("dbo.Mentorships", "ThesisTypeID", "dbo.ThesisTypes", "ThesisTypeID");
            AddForeignKey("dbo.Mentorships", "PersonID", "dbo.People", "PersonID");
        }
        
        public override void Down()
        {
        }
    }
}
