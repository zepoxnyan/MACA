namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonAuthorId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "AuthorID", c => c.Guid(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
