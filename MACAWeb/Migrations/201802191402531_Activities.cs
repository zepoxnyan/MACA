namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activities : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Activities", t => new
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
            AddPrimaryKey("dbo.Activities", "ActivityID");
            AddForeignKey("dbo.Activities", "ActivityTypeID", "dbo.ActivityTypes", "ActivityTypeID");
            AddForeignKey("dbo.Activities", "PersonID", "dbo.People", "PersonID");
        }
        
        public override void Down()
        {
        }
    }
}
