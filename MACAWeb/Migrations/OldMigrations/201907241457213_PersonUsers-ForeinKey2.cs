namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUsersForeinKey2 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.PersonUsers", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
