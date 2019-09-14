namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleAisIDUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "AISID", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
