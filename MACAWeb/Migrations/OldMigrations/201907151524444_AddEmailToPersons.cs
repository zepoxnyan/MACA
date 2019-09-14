namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailToPersons : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Email", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "ImageThumb");
        }
    }
}
