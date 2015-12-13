namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        UserID = c.Int(nullable: false),
                        DataField_Id = c.Int(),
                        DataField_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataFields", t => t.DataField_Id)
                .ForeignKey("dbo.DataFields", t => t.DataField_Id1)
                .Index(t => t.DataField_Id)
                .Index(t => t.DataField_Id1);
            
            CreateTable(
                "dbo.StoredStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        UserData_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDatas", t => t.UserData_Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.UserData_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Item_Id = c.Int(),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Stage_Id);
            
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
                        Id = c.Int(nullable: false, identity: true),
                        IsEditable = c.Boolean(nullable: false),
                        TaskType = c.Int(nullable: false),
                        Paper_Id = c.Int(),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Paper_Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Paper_Id)
                .Index(t => t.Stage_Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StudyID = c.Int(nullable: false),
                        CurrentTaskType = c.Int(nullable: false),
                        DistributionRule = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Studies", t => t.StudyID, cascadeDelete: true)
                .Index(t => t.StudyID);
            
            CreateTable(
                "dbo.UserStudies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudyRole = c.Int(nullable: false),
                        Stage_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Stage_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.UserStudyTasks",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        StudyTask_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.StudyTask_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.StudyTask_Id);
            
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
            DropForeignKey("dbo.Studies", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Stages", "StudyID", "dbo.Studies");
            DropForeignKey("dbo.Items", "Study_Id", "dbo.Studies");
            DropForeignKey("dbo.FieldTypes", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.UserStudies", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.UserStudyTasks", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.UserStudyTasks", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StudyTasks", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.Criteria", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StudyTasks", "Paper_Id", "dbo.Items");
            DropForeignKey("dbo.DataFields", "StudyTask_Id", "dbo.StudyTasks");
            DropForeignKey("dbo.StoredStrings", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.FieldTypes", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.UserDatas", "DataField_Id1", "dbo.DataFields");
            DropForeignKey("dbo.UserDatas", "DataField_Id", "dbo.DataFields");
            DropForeignKey("dbo.StoredStrings", "UserData_Id", "dbo.UserDatas");
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.UserStudyTasks", new[] { "StudyTask_Id" });
            DropIndex("dbo.UserStudyTasks", new[] { "User_Id" });
            DropIndex("dbo.Studies", new[] { "Team_Id" });
            DropIndex("dbo.UserStudies", new[] { "User_Id" });
            DropIndex("dbo.UserStudies", new[] { "Stage_Id" });
            DropIndex("dbo.Stages", new[] { "StudyID" });
            DropIndex("dbo.StudyTasks", new[] { "Stage_Id" });
            DropIndex("dbo.StudyTasks", new[] { "Paper_Id" });
            DropIndex("dbo.Items", new[] { "Study_Id" });
            DropIndex("dbo.FieldTypes", new[] { "Stage_Id" });
            DropIndex("dbo.FieldTypes", new[] { "Item_Id" });
            DropIndex("dbo.StoredStrings", new[] { "Item_Id" });
            DropIndex("dbo.StoredStrings", new[] { "UserData_Id" });
            DropIndex("dbo.UserDatas", new[] { "DataField_Id1" });
            DropIndex("dbo.UserDatas", new[] { "DataField_Id" });
            DropIndex("dbo.DataFields", new[] { "StudyTask_Id" });
            DropIndex("dbo.Criteria", new[] { "Stage_Id" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.UserStudyTasks");
            DropTable("dbo.Studies");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.UserStudies");
            DropTable("dbo.Stages");
            DropTable("dbo.StudyTasks");
            DropTable("dbo.Items");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.StoredStrings");
            DropTable("dbo.UserDatas");
            DropTable("dbo.DataFields");
            DropTable("dbo.Criteria");
        }
    }
}