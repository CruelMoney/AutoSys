namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Criteria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DataType = c.Int(nullable: false),
                        Rule = c.Int(nullable: false),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Stage_Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Study_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Studies", t => t.Study_Id)
                .Index(t => t.Study_Id);
            
            CreateTable(
                "dbo.Studies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CurrentStage = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Study_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Studies", t => t.Study_Id)
                .Index(t => t.Study_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                        Study_Id = c.Int(),
                        Study_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Studies", t => t.Study_Id)
                .ForeignKey("dbo.Studies", t => t.Study_Id1)
                .Index(t => t.Study_Id)
                .Index(t => t.Study_Id1);
            
            CreateTable(
                "dbo.StudyTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskType = c.Int(nullable: false),
                        IsDeliverable = c.Boolean(nullable: false),
                        Paper_Id = c.Int(),
                        Stage_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Paper_Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Paper_Id)
                .Index(t => t.Stage_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TaskRequestedDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudyTask_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.StudyTask_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FieldType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.User_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Study_Id1", "dbo.Studies");
            DropForeignKey("dbo.Stages", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.Users", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Studies", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.StudyTasks", "User_Id", "dbo.Users");
            DropForeignKey("dbo.StudyTasks", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.TaskRequestedDatas", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TaskRequestedDatas", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.StudyTasks", "Paper_Id", "dbo.Items");
            DropForeignKey("dbo.Items", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.Criteria", "Stage_Id", "dbo.Stages");
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.TaskRequestedDatas", new[] { "User_Id" });
            DropIndex("dbo.TaskRequestedDatas", new[] { "StudyTask_Id" });
            DropIndex("dbo.StudyTasks", new[] { "User_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Stage_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Paper_Id" });
            DropIndex("dbo.Users", new[] { "Study_Id1" });
            DropIndex("dbo.Users", new[] { "Study_Id" });
            DropIndex("dbo.Items", new[] { "Study_Id" });
            DropIndex("dbo.Studies", new[] { "Team_Id" });
            DropIndex("dbo.Stages", new[] { "Study_Id" });
            DropIndex("dbo.Criteria", new[] { "Stage_Id" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.DataFields");
            DropTable("dbo.Teams");
            DropTable("dbo.TaskRequestedDatas");
            DropTable("dbo.StudyTasks");
            DropTable("dbo.Users");
            DropTable("dbo.Items");
            DropTable("dbo.Studies");
            DropTable("dbo.Stages");
            DropTable("dbo.Criteria");
        }
    }
}
