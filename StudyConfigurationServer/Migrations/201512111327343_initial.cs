namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.UserStudies", "User_Id", "dbo.Users");
            DropIndex("dbo.UserStudies", new[] { "Stage_Id" });
            DropIndex("dbo.UserStudies", new[] { "User_Id" });
            AlterColumn("dbo.UserStudies", "Stage_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.UserStudies", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.UserStudies", "Stage_Id");
            CreateIndex("dbo.UserStudies", "User_Id");
            AddForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserStudies", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserStudies", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages");
            DropIndex("dbo.UserStudies", new[] { "User_Id" });
            DropIndex("dbo.UserStudies", new[] { "Stage_Id" });
            AlterColumn("dbo.UserStudies", "User_Id", c => c.Int());
            AlterColumn("dbo.UserStudies", "Stage_Id", c => c.Int());
            CreateIndex("dbo.UserStudies", "User_Id");
            CreateIndex("dbo.UserStudies", "Stage_Id");
            AddForeignKey("dbo.UserStudies", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserStudies", "Stage_Id", "dbo.Stages", "Id");
        }
    }
}
