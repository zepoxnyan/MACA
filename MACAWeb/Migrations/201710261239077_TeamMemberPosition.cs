namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMemberPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMembers", "PagePosition", c => c.Int(nullable: false, defaultValueSql: "10"));
        }
        
        public override void Down()
        {
        }
    }
}
