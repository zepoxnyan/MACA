namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskStatus : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.TaskStatus", "StatusId", "TaskStatusId");

            CreateTable("dbo.TaskMessages", t => new
            {
                TaskMessageId = t.Guid(nullable: false),
                TaskId = t.Guid(nullable: false),
                TaskStatusId = t.Guid(nullable: false),
                DateCreated = t.DateTime(nullable: false),
                UserId = t.Guid(nullable: false)
            });
            AddPrimaryKey("dbo.TaskMessages", "TaskMessageId");
            AddForeignKey("dbo.TaskMessages", "TaskId", "dbo.UserTasks", "TaskId");
            AddForeignKey("dbo.TaskMessages", "TaskStatusId", "dbo.TaskStatus", "TaskStatusId");
        }
        
        public override void Down()
        {
        }
    }
}
