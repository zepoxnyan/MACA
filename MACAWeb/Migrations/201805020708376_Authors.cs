namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Authors : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Authors", t => new
            {
                AuthorID = t.Guid(nullable: false),
                AuthorTypeID = t.Guid(nullable: false),

                Surname = t.String(nullable: false),
                FirstName = t.String(nullable: false),
                CREPCCode = t.String(nullable: false),
                ORCID = t.Int(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.Authors", "AuthorID");
            AddForeignKey("dbo.Authors", "AuthorTypeID", "dbo.AuthorTypes", "AuthorTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
