namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Teachings : DbMigration
    {
        public override void Up()
        {
            /*CreateTable("dbo.Activities", t => new
            {
                ActivityID = t.Guid(nullable: false),
                ActivityTypeID = t.Guid(nullable: false),
                PersonID = t.Guid(nullable: false),

                Remark = t.String(nullable: true),
                Weight = t.Double(nullable: true, defaultValueSql: "1.0"),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Teachings", "TeachingID");
            AddForeignKey("dbo.Teachings", "TeachingTypeID", "dbo.TeachingTypes", "TeachingTypeID");
            AddForeignKey("dbo.Teachings", "SubjectID", "dbo.Subjects", "SubjectID");
            AddForeignKey("dbo.Teachings", "PersonID", "dbo.People", "PersonID");*/
        }
        
        public override void Down()
        {
        }
    }
}
