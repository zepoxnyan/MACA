namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleAisID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "AISID", c => c.Guid(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
