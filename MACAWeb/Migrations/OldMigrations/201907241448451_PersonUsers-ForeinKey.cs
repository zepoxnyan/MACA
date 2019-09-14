namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUsersForeinKey : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.PersonUsers", "PersonID", "dbo.People", "PersonID");
        }
        
        public override void Down()
        {
        }
    }
}
