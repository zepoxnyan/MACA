namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMembers : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.TeamMembers", "Image", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
