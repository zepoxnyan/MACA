namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionsUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PositionTypes", "PositionTypeID", "dbo.PositionTypes");
            AddForeignKey("dbo.Positions", "PositionTypeID", "dbo.PositionTypes", "PositionTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
