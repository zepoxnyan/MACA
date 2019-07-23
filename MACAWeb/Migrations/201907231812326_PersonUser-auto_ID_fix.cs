namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonUserauto_ID_fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonUser", "PersonUserID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
        }
        
        public override void Down()
        {
        }
    }
}
