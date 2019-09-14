namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeachingTypesAIS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeachingTypes", "AISCode", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
