namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeachingUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Teachings", "Hours", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
