namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SubjectsUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "FK_dbo.Subjects_dbo.StudyLevels_StudyLevelID");
            AlterColumn("dbo.Subjects", "StudyLevelID", c => c.Guid(nullable: true));
            AlterColumn("dbo.Subjects", "Year", c => c.String(nullable: false));

            AddColumn("dbo.Subjects", "AISCode", c=>c.String(nullable:false));
            AddColumn("dbo.Subjects", "ShortName", c => c.String(nullable: false));
            AddColumn("dbo.Subjects", "Department", c => c.String(nullable: false));
        }

        public override void Down()
        {
        }
    }
}
