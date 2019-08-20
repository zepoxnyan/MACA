namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInterests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        InterestID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InterestID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interests", "PersonID", "dbo.People");
            DropIndex("dbo.Interests", new[] { "PersonID" });
            DropTable("dbo.Interests");
        }
    }
}
