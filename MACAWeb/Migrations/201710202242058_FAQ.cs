namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAQ : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FAQs", "UserCreatedID", c => c.Guid(nullable: false));
            AlterColumn("dbo.FAQs", "UserModifiedID", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
