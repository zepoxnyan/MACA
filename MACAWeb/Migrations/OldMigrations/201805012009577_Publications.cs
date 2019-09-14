namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Publications : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Publications", t => new
            {
                PublicationID = t.Guid(nullable: false),
                PublicationTypeID = t.Guid(nullable: false),
                PublicationTypeLocalID = t.Guid(nullable: false),
                PublicationClassificationID = t.Guid(nullable: false),
                PublicationStatusID = t.Guid(nullable: false),

                Title = t.String(nullable: false),
                Year = t.Int(nullable: false),
                Journal = t.String(nullable: true),
                Volume = t.Int(nullable: true),
                Issue = t.Int(nullable: true),
                Pages = t.String(nullable: true),
                DOI = t.String(nullable: true),
                Link = t.String(nullable: true),
                PreprintLink = t.String(nullable: true),
                Note = t.String(nullable: true),
                Editors = t.String(nullable: true),
                Publisher = t.String(nullable: true),
                Series = t.String(nullable: true),
                Address = t.String(nullable: true),
                Edition = t.String(nullable: true),
                BookTitle = t.String(nullable: true),
                Organization = t.String(nullable: true),
                Chapter = t.String(nullable: true),
                Keywords = t.String(nullable: true),
                Abstract = t.String(nullable: true),
                
                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Publications", "PublicationID");
            AddForeignKey("dbo.Publications", "PublicationTypeID", "dbo.PublicationTypes", "PublicationTypeID");
            AddForeignKey("dbo.Publications", "PublicationTypeLocalID", "dbo.PublicationTypeLocals", "PublicationTypeLocalID");
            AddForeignKey("dbo.Publications", "PublicationClassificationID", "dbo.PublicationClassifications", "PublicationClassificationID");
            AddForeignKey("dbo.Publications", "PublicationStatusID", "dbo.PublicationStatus", "PublicationStatusID");
        }
        
        public override void Down()
        {
        }
    }
}
