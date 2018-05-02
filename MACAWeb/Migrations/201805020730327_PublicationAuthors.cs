namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationAuthors : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.PublicationAuthors", t => new
            {
                PublicationAuthorID = t.Guid(nullable: false),
                AuthorID = t.Guid(nullable: false),
                AuthorTypeID = t.Guid(nullable: false),
                PublicationID = t.Guid(nullable: false),

                Percent = t.Double(nullable: false, defaultValueSql: "1.0"),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.PublicationAuthors", "PublicationAuthorID");
            AddForeignKey("dbo.PublicationAuthors", "AuthorID", "dbo.Authors", "AuthorID");
            AddForeignKey("dbo.PublicationAuthors", "AuthorTypeID", "dbo.AuthorTypes", "AuthorTypeID");
            AddForeignKey("dbo.PublicationAuthors", "PublicationID", "dbo.Publications", "PublicationID");
        }
        
        public override void Down()
        {
        }
    }
}
