namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonAuthorIdFK : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.People", "AuthorID", "Authors", "AuthorID");
        }
        
        public override void Down()
        {
        }
    }
}
