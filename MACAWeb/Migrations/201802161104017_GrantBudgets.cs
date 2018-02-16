namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrantBudgets : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.GrantBudgets", t => new
            {
                GrantBudgetID = t.Guid(nullable: false),
                GrantID = t.Guid(nullable: false),
                GrantBudgetsTypeID = t.Guid(nullable: false),

                Description = t.String(nullable: true),
                Year = t.Int(nullable: false),
                Amount = t.Double(nullable: false),

                DateCreated = t.DateTime(nullable: false),
                DateModified = t.DateTime(nullable: false),
                UserCreatedID = t.Guid(nullable: false),
                UserModifiedID = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.GrantBudgets", "GrantBudgetID");
            AddForeignKey("dbo.GrantBudgets", "GrantID", "dbo.Grants", "GrantID");
            AddForeignKey("dbo.GrantBudgets", "GrantBudgetsTypeID", "dbo.GrantBudgetsTypes", "GrantBudgetsTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
