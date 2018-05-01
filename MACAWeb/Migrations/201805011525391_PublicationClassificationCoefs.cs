namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicationClassificationCoefs : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.PublicationClassificationCoefficients", t => new
            {
                PublicationClassificationCoefficientID = t.Guid(nullable: false),
                PublicationClassificationID = t.Guid(nullable: false),
                PublicationTypeGroupID = t.Guid(nullable: false),

                Year = t.Int(nullable: false),
                Coefficient = t.Double(nullable: false),
                Description = t.String(nullable: true),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.PublicationClassificationCoefficients", "PublicationClassificationCoefficientID");
            AddForeignKey("dbo.PublicationClassificationCoefficients", "PublicationClassificationID", "dbo.PublicationClassifications", "PublicationClassificationID");
            AddForeignKey("dbo.PublicationClassificationCoefficients", "PublicationTypeGroupID", "dbo.PublicationTypeGroups", "PublicationTypeGroupID");
        }
        
        public override void Down()
        {
        }
    }
}
