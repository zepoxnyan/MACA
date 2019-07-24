namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropPersonUserdroptable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.PersonUser");
        }
        
        public override void Down()
        {
        }
    }
}
