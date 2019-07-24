namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUserupgrade2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonUser", "UserID", "dbo.AspNetUsers", "Id");
            DropForeignKey("dbo.PersonUser", "PersonID", "dbo.People", "PersonID");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.PersonUser", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PersonUser", "PersonID", "dbo.People", "PersonID");
        }
    }
}
