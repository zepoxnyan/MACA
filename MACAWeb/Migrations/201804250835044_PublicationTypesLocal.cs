namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationTypesLocal : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.PublicationTypeLocals", t => new
            {
                PublicationTypeLocalID = t.Guid(nullable: false),
                PublicationTypeGroupID = t.Guid(nullable: false),

                Name = t.String(nullable: false),
                Description = t.String(nullable: true),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.PublicationTypeLocals", "PublicationTypeLocalID");
            AddForeignKey("dbo.PublicationTypeLocals", "PublicationTypeGroupID", "dbo.PublicationTypeGroups", "PublicationTypeGroupID");
        }
        
        public override void Down()
        {
        }
    }
}
