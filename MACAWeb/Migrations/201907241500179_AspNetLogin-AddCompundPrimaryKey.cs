namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetLoginAddCompundPrimaryKey : DbMigration
    {
        public override void Up()
        {
            //AddPrimaryKey("dbo.AspNetUserLogins", "ProviderKey","UserId");
        }
        
        public override void Down()
        {
        }
    }
}
