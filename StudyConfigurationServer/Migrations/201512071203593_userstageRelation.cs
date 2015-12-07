namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userstageRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserStudies", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.TaskRequestedDatas", "StudyTask_Id", "dbo.StudyTasks");
            DropIndex("dbo.UserStudies", new[] { "Study_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Paper_Id" });
            DropColumn("dbo.StudyTasks", "Id");
            RenameColumn(table: "dbo.StudyTasks", name: "Paper_Id", newName: "Id");
            RenameColumn(table: "dbo.DataFields", name: "Task_Id", newName: "TaskRequestedData_Id");
            RenameIndex(table: "dbo.DataFields", name: "IX_Task_Id", newName: "IX_TaskRequestedData_Id");
            DropPrimaryKey("dbo.StudyTasks");
            AddColumn("dbo.Studies", "Team_Id", c => c.Int());
            AddColumn("dbo.UserStudies", "Stage_Id", c => c.Int());
            AddColumn("dbo.StudyTasks", "IsFinished", c => c.Boolean(nullable: false));
            AlterColumn("dbo.StudyTasks", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.StudyTasks", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudyTasks", "Id");
            CreateIndex("dbo.Studies", "Team_Id");
            CreateIndex("dbo.StudyTasks", "Id");
            CreateIndex("dbo.UserStudies", "Stage_Id");
            AddForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages", "Id");
            AddForeignKey("dbo.Studies", "Team_Id", "dbo.Teams", "Id");
            AddForeignKey("dbo.TaskRequestedDatas", "StudyTask_Id", "dbo.StudyTasks", "Id");
            DropColumn("dbo.UserStudies", "Study_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserStudies", "Study_Id", c => c.Int());
            DropForeignKey("dbo.TaskRequestedDatas", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.Studies", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages");
            DropIndex("dbo.UserStudies", new[] { "Stage_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Id" });
            DropIndex("dbo.Studies", new[] { "Team_Id" });
            DropPrimaryKey("dbo.StudyTasks");
            AlterColumn("dbo.StudyTasks", "Id", c => c.Int());
            AlterColumn("dbo.StudyTasks", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.StudyTasks", "IsFinished");
            DropColumn("dbo.UserStudies", "Stage_Id");
            DropColumn("dbo.Studies", "Team_Id");
            AddPrimaryKey("dbo.StudyTasks", "Id");
            RenameIndex(table: "dbo.DataFields", name: "IX_TaskRequestedData_Id", newName: "IX_Task_Id");
            RenameColumn(table: "dbo.DataFields", name: "TaskRequestedData_Id", newName: "Task_Id");
            RenameColumn(table: "dbo.StudyTasks", name: "Id", newName: "Paper_Id");
            AddColumn("dbo.StudyTasks", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.StudyTasks", "Paper_Id");
            CreateIndex("dbo.UserStudies", "Study_Id");
            AddForeignKey("dbo.TaskRequestedDatas", "StudyTask_Id", "dbo.StudyTasks", "Id");
            AddForeignKey("dbo.UserStudies", "Study_Id", "dbo.Studies", "Id");
        }
    }
}
