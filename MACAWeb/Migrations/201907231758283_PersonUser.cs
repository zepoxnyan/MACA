namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUser : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.PersonUser", t => new
            {
                PersonUserID = t.Guid(nullable: false),
                PersonID = t.Guid(nullable: false),
                UserID = t.String(nullable: false, maxLength: 128)
            });
            AddPrimaryKey("dbo.PersonUser", "PersonUserID");
            AddForeignKey("dbo.PersonUser", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PersonUser", "PersonID", "dbo.People", "PersonID");
        }

        public override void Down()
        {
        }
    }
}
