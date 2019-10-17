namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Publications", "BibtexFile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Publications", "BibtexFile", c => c.Binary());
        }
    }
}
