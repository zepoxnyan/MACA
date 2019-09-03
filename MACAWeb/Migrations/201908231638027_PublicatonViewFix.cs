namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicatonViewFix : DbMigration
    {
        public override void Up()
        {
            AddPrimaryKey("dbo.Publications", "PublicationID");
            AddPrimaryKey("dbo.Authors", "AuthorID");
            AddPrimaryKey("dbo.PublicationAuthors", "PublicationAuthorID");
            AddForeignKey("dbo.PublicationAuthors", "PublicationID", "dbo.Publications", "PublicationID");
            AddForeignKey("dbo.PublicationAuthors", "AuthorID", "dbo.Authors", "AuthorID");
        }

        public override void Down()
        {

        }
    }
}
