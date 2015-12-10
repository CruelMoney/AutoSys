namespace StudyConfigurationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class api : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDatas", "User_Id", "dbo.Users");
            DropIndex("dbo.UserDatas", new[] { "User_Id" });
            RenameColumn(table: "dbo.UserDatas", name: "User_Id", newName: "UserID");
            AlterColumn("dbo.UserDatas", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.UserDatas", "UserID");
            AddForeignKey("dbo.UserDatas", "UserID", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDatas", "UserID", "dbo.Users");
            DropIndex("dbo.UserDatas", new[] { "UserID" });
            AlterColumn("dbo.UserDatas", "UserID", c => c.Int());
            RenameColumn(table: "dbo.UserDatas", name: "UserID", newName: "User_Id");
            CreateIndex("dbo.UserDatas", "User_Id");
            AddForeignKey("dbo.UserDatas", "User_Id", "dbo.Users", "Id");
        }
    }
}
