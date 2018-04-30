namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationTypesLocalCoefs : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.PublicationTypeLocalCoefs", t => new
            {
                PublicationTypeLocalCoefID = t.Guid(nullable: false),
                PublicationTypeLocalID = t.Guid(nullable: false),

                Year = t.Int(nullable: false),
                Coefficient = t.Double(nullable: false),
                Description = t.String(nullable: true),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.PublicationTypeLocalCoefs", "PublicationTypeLocalCoefID");
            AddForeignKey("dbo.PublicationTypeLocalCoefs", "PublicationTypeLocalID", "dbo.PublicationTypeLocals", "PublicationTypeLocalID");
        }
        
        public override void Down()
        {
        }
    }
}
