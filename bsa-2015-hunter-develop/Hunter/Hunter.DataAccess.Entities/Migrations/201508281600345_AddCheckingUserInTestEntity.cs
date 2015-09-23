namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckingUserInTestEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Test", "UserProfileId", c => c.Int());
            AddColumn("dbo.Test", "IsChecked", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Test", "UserProfileId");
            AddForeignKey("dbo.Test", "UserProfileId", "dbo.UserProfile", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Test", "UserProfileId", "dbo.UserProfile");
            DropIndex("dbo.Test", new[] { "UserProfileId" });
            DropColumn("dbo.Test", "IsChecked");
            DropColumn("dbo.Test", "UserProfileId");
        }
    }
}
