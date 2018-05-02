namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorsUpdate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors","CREPCCode", c => c.String(nullable: true));
            AlterColumn("dbo.Authors", "ORCID", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
