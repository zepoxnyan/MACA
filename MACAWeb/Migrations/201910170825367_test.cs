namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Publications", "BibtexFile");
            AddColumn("dbo.Publications", "BibtexFile", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publications", "BibtexFile");
            AddColumn("dbo.Publications", "BibtexFile", c => c.String());
        }
    }
}
