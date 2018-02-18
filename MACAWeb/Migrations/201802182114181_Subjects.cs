namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subjects : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Subjects", t => new
            {
                SubjectID = t.Guid(nullable: false),
                StudyLevelID = t.Guid(nullable: false),

                Name = t.String(nullable: false),
                Description = t.String(nullable: true),
                Year = t.Int(nullable: false),
                Semester = t.Int(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Subjects", "SubjectID");
            AddForeignKey("dbo.Subjects", "StudyLevelID", "dbo.StudyLevels", "StudyLevelID");            
        }
        
        public override void Down()
        {
        }
    }
}
