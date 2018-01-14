namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("UserTasks", "Name", c => c.String(nullable: false));
            AddColumn("UserTasks", "Remark", c => c.String(nullable: true));
            RenameColumn("UserTasks", "StatusId", "TaskStatusId");
            AddForeignKey("UserTasks", "TaskStatusId", "TaskStatus", "TaskStatusId");
            DropColumn("UserTasks", "UserId");
        }
        
        public override void Down()
        {
        }
    }
}
