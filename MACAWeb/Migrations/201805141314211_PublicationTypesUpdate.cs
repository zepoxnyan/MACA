namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationTypesUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicationTypes", "Code", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
