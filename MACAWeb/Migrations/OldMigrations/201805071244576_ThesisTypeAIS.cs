namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ThesisTypeAIS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThesisTypes", "AISCode", c => c.Int());
        }

        public override void Down()
        {
        }
    }
}
