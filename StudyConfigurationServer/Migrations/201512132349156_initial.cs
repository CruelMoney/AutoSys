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
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DataType = c.Int(nullable: false),
                        Rule = c.Int(nullable: false),
                        Stage_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Stages", t => t.Stage_ID)
                .Index(t => t.Stage_ID);
            
            CreateTable(
                "dbo.StoredStrings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Criteria_ID = c.Int(),
                        Criteria_ID1 = c.Int(),
                        UserData_ID = c.Int(),
                        DataField_ID = c.Int(),
                        Item_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Criteria", t => t.Criteria_ID)
                .ForeignKey("dbo.Criteria", t => t.Criteria_ID1)
                .ForeignKey("dbo.UserDatas", t => t.UserData_ID)
                .ForeignKey("dbo.DataFields", t => t.DataField_ID)
                .ForeignKey("dbo.Items", t => t.Item_ID)
                .Index(t => t.Criteria_ID)
                .Index(t => t.Criteria_ID1)
                .Index(t => t.UserData_ID)
                .Index(t => t.DataField_ID)
                .Index(t => t.Item_ID);
            
            CreateTable(
                "dbo.DataFields",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FieldType = c.Int(nullable: false),
                        StudyTask_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_ID)
                .Index(t => t.StudyTask_ID);
            
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        DataField_ID = c.Int(),
                        DataField_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DataFields", t => t.DataField_ID)
                .ForeignKey("dbo.DataFields", t => t.DataField_ID1)
                .Index(t => t.DataField_ID)
                .Index(t => t.DataField_ID1);
            
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Item_ID = c.Int(),
                        Stage_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Items", t => t.Item_ID)
                .ForeignKey("dbo.Stages", t => t.Stage_ID)
                .Index(t => t.Item_ID)
                .Index(t => t.Stage_ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Study_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Studies", t => t.Study_ID)
                .Index(t => t.Study_ID);
            
            CreateTable(
                "dbo.StudyTasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsEditable = c.Boolean(nullable: false),
                        TaskType = c.Int(nullable: false),
                        Paper_ID = c.Int(),
                        Stage_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Items", t => t.Paper_ID)
                .ForeignKey("dbo.Stages", t => t.Stage_ID)
                .Index(t => t.Paper_ID)
                .Index(t => t.Stage_ID);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CurrentTaskType = c.Int(nullable: false),
                        DistributionRule = c.Int(nullable: false),
                        IsCurrentStage = c.Boolean(nullable: false),
                        Study_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Studies", t => t.Study_ID)
                .Index(t => t.Study_ID);
            
            CreateTable(
                "dbo.Studies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CurrentStageID = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        Team_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Teams", t => t.Team_ID)
                .Index(t => t.Team_ID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Metadata = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserStudies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudyRole = c.Int(nullable: false),
                        Stage_ID = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Stages", t => t.Stage_ID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.Stage_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.UserStudyTasks",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        StudyTask_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.StudyTask_ID })
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.StudyTasks", t => t.StudyTask_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.StudyTask_ID);
            
            CreateTable(
                "dbo.UserTeams",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Team_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Team_ID })
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Team_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FieldTypes", "Stage_ID", "dbo.Stages");
            DropForeignKey("dbo.StudyTasks", "Stage_ID", "dbo.Stages");
            DropForeignKey("dbo.Studies", "Team_ID", "dbo.Teams");
            DropForeignKey("dbo.UserTeams", "Team_ID", "dbo.Teams");
            DropForeignKey("dbo.UserTeams", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserStudyTasks", "StudyTask_ID", "dbo.StudyTasks");
            DropForeignKey("dbo.UserStudyTasks", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "Stage_ID", "dbo.Stages");
            DropForeignKey("dbo.Stages", "Study_ID", "dbo.Studies");
            DropForeignKey("dbo.Items", "Study_ID", "dbo.Studies");
            DropForeignKey("dbo.Criteria", "Stage_ID", "dbo.Stages");
            DropForeignKey("dbo.StudyTasks", "Paper_ID", "dbo.Items");
            DropForeignKey("dbo.DataFields", "StudyTask_ID", "dbo.StudyTasks");
            DropForeignKey("dbo.StoredStrings", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.FieldTypes", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.UserDatas", "DataField_ID1", "dbo.DataFields");
            DropForeignKey("dbo.StoredStrings", "DataField_ID", "dbo.DataFields");
            DropForeignKey("dbo.UserDatas", "DataField_ID", "dbo.DataFields");
            DropForeignKey("dbo.StoredStrings", "UserData_ID", "dbo.UserDatas");
            DropForeignKey("dbo.StoredStrings", "Criteria_ID1", "dbo.Criteria");
            DropForeignKey("dbo.StoredStrings", "Criteria_ID", "dbo.Criteria");
            DropIndex("dbo.UserTeams", new[] { "Team_ID" });
            DropIndex("dbo.UserTeams", new[] { "User_ID" });
            DropIndex("dbo.UserStudyTasks", new[] { "StudyTask_ID" });
            DropIndex("dbo.UserStudyTasks", new[] { "User_ID" });
            DropIndex("dbo.UserStudies", new[] { "User_ID" });
            DropIndex("dbo.UserStudies", new[] { "Stage_ID" });
            DropIndex("dbo.Studies", new[] { "Team_ID" });
            DropIndex("dbo.Stages", new[] { "Study_ID" });
            DropIndex("dbo.StudyTasks", new[] { "Stage_ID" });
            DropIndex("dbo.StudyTasks", new[] { "Paper_ID" });
            DropIndex("dbo.Items", new[] { "Study_ID" });
            DropIndex("dbo.FieldTypes", new[] { "Stage_ID" });
            DropIndex("dbo.FieldTypes", new[] { "Item_ID" });
            DropIndex("dbo.UserDatas", new[] { "DataField_ID1" });
            DropIndex("dbo.UserDatas", new[] { "DataField_ID" });
            DropIndex("dbo.DataFields", new[] { "StudyTask_ID" });
            DropIndex("dbo.StoredStrings", new[] { "Item_ID" });
            DropIndex("dbo.StoredStrings", new[] { "DataField_ID" });
            DropIndex("dbo.StoredStrings", new[] { "UserData_ID" });
            DropIndex("dbo.StoredStrings", new[] { "Criteria_ID1" });
            DropIndex("dbo.StoredStrings", new[] { "Criteria_ID" });
            DropIndex("dbo.Criteria", new[] { "Stage_ID" });
            DropTable("dbo.UserTeams");
            DropTable("dbo.UserStudyTasks");
            DropTable("dbo.UserStudies");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.Studies");
            DropTable("dbo.Stages");
            DropTable("dbo.StudyTasks");
            DropTable("dbo.Items");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.UserDatas");
            DropTable("dbo.DataFields");
            DropTable("dbo.StoredStrings");
            DropTable("dbo.Criteria");
        }
    }
}
