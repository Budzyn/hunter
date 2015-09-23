namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserIdOnUserProfileId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vacancy", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Vacancy", name: "IX_UserId", newName: "IX_User_Id");
            AddColumn("dbo.Vacancy", "UserProfileId", c => c.Int());
            CreateIndex("dbo.Vacancy", "UserProfileId");
            AddForeignKey("dbo.Vacancy", "UserProfileId", "dbo.UserProfile", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacancy", "UserProfileId", "dbo.UserProfile");
            DropIndex("dbo.Vacancy", new[] { "UserProfileId" });
            DropColumn("dbo.Vacancy", "UserProfileId");
            RenameIndex(table: "dbo.Vacancy", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Vacancy", name: "User_Id", newName: "UserId");
        }
    }
}
