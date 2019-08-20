namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConferenceTalks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConferenceTalks",
                c => new
                    {
                        ConferenceTalkID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        ConferenceName = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Year = c.String(nullable: false),
                        InvitedTalk = c.Boolean(nullable: false),
                        PdfLink = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ConferenceTalkID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConferenceTalks", "PersonID", "dbo.People");
            DropIndex("dbo.ConferenceTalks", new[] { "PersonID" });
            DropTable("dbo.ConferenceTalks");
        }
    }
}
