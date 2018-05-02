namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorsUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Authors", "FK_dbo.Authors_dbo.AuthorTypes_AuthorTypeID");
            DropColumn("dbo.Authors", "AuthorTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
