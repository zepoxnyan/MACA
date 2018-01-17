namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Grants : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grants", "GrantStatusID", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Grants", "GrantStatusID", "dbo.GrantStatus", "GrantStatusID");
        }
        
        public override void Down()
        {
        }
    }
}
