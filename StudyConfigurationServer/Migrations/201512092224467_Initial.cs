namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        CurrentTaskType = c.Int(nullable: false),
                        DistributionRule = c.Int(nullable: false),
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
                        CurrentStageID = c.Int(nullable: false),
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
                "dbo.StudyTasks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsEditable = c.Boolean(nullable: false),
                        TaskType = c.Int(nullable: false),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Id)
                .Index(t => t.Stage_Id);
            
            CreateTable(
                "dbo.DataFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FieldType = c.Int(nullable: false),
                        StudyTask_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_Id)
                .Index(t => t.StudyTask_Id);
            
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                        DataField_Id = c.Int(),
                        DataField_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.DataFields", t => t.DataField_Id)
                .ForeignKey("dbo.DataFields", t => t.DataField_Id1)
                .Index(t => t.User_Id)
                .Index(t => t.DataField_Id)
                .Index(t => t.DataField_Id1);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                        StudyTask_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_Id)
                .Index(t => t.StudyTask_Id);
            
            CreateTable(
                "dbo.UserStudies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudyRole = c.Int(nullable: false),
                        Stage_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Stage_Id)
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
            DropForeignKey("dbo.Stages", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.Items", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.Users", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.StudyTasks", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StudyTasks", "Id", "dbo.Items");
            DropForeignKey("dbo.DataFields", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.UserDatas", "DataField_Id1", "dbo.DataFields");
            DropForeignKey("dbo.UserDatas", "DataField_Id", "dbo.DataFields");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Studies", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.UserDatas", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.Criteria", "Stage_Id", "dbo.Stages");
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.UserStudies", new[] { "User_Id" });
            DropIndex("dbo.UserStudies", new[] { "Stage_Id" });
            DropIndex("dbo.Users", new[] { "StudyTask_Id" });
            DropIndex("dbo.UserDatas", new[] { "DataField_Id1" });
            DropIndex("dbo.UserDatas", new[] { "DataField_Id" });
            DropIndex("dbo.UserDatas", new[] { "User_Id" });
            DropIndex("dbo.DataFields", new[] { "StudyTask_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Stage_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Id" });
            DropIndex("dbo.Items", new[] { "Study_Id" });
            DropIndex("dbo.Studies", new[] { "Team_Id" });
            DropIndex("dbo.Stages", new[] { "Study_Id" });
            DropIndex("dbo.Criteria", new[] { "Stage_Id" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.Teams");
            DropTable("dbo.UserStudies");
            DropTable("dbo.Users");
            DropTable("dbo.UserDatas");
            DropTable("dbo.DataFields");
            DropTable("dbo.StudyTasks");
            DropTable("dbo.Items");
            DropTable("dbo.Studies");
            DropTable("dbo.Stages");
            DropTable("dbo.Criteria");
        }
    }
}
