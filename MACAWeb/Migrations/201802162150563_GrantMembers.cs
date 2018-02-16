namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrantMembers : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.GrantMembers", t => new
            {
                GrantMemberID = t.Guid(nullable: false),
                GrantID = t.Guid(nullable: false),
                PersonID = t.Guid(nullable: false),
                GrantMemberTypeID = t.Guid(nullable: false),

                Description = t.String(nullable: true),
                Year = t.Int(nullable: false),
                Hours = t.Double(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.GrantMembers", "GrantMemberID");
            AddForeignKey("dbo.GrantMembers", "GrantID", "dbo.Grants", "GrantID");
            AddForeignKey("dbo.GrantMembers", "GrantMemberTypeID", "dbo.GrantMemberTypes", "GrantMemberTypeID");
            AddForeignKey("dbo.GrantMembers", "PersonID", "dbo.People", "PersonID");
        }
        
        public override void Down()
        {
        }
    }
}
