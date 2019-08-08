namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialLinks : DbMigration
    {
        public override void Up()
        {            
            CreateTable(
                "dbo.SocialLinks",
                c => new
                {
                    SocialLinkID = c.Guid(nullable: false),
                    PersonID = c.Guid(nullable: false),
                    SocialLinkTypeID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.SocialLinkID);
        }
        
        public override void Down()
        {
            DropTable("dbo.SocialLinks");
        }
    }
}
