namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeminarTalks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeminarTalks",
                c => new
                    {
                        SeminarTalkID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        SeminarName = c.String(nullable: false),
                        University = c.String(),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Year = c.String(nullable: false),
                        PdfLink = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SeminarTalkID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeminarTalks", "PersonID", "dbo.People");
            DropIndex("dbo.SeminarTalks", new[] { "PersonID" });
            DropTable("dbo.SeminarTalks");
        }
    }
}
