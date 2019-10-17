namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationAddBibtex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publications", "BibtexFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publications", "BibtexFile");
        }
    }
}
