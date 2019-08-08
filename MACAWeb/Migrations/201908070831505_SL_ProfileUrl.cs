namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SL_ProfileUrl : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SocialLinks");
            CreateTable(
                "dbo.SocialLinks",
                c => new
                    {
                        SocialLinkID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        SocialLinkTypeID = c.Guid(nullable: false),
                        ProfileUrl = c.String(),
                    })
                .PrimaryKey(t => t.SocialLinkID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SocialLinks");
        }
    }
}
