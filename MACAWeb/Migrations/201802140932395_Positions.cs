namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Positions : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Positions", t => new
            {
                PositionID = t.Guid(nullable: false),
                PersonID = t.Guid(nullable: false),
                PositionTypeID = t.Guid(nullable: false),

                Description = t.String(nullable: true),
                Year = t.Int(nullable: false),
                Semester = t.Int(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Positions", "PositionID");
            AddForeignKey("dbo.Positions", "PersonID", "dbo.People", "PersonID");            
        }
        
        public override void Down()
        {
        }
    }
}
