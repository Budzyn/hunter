namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_userprofile_field_to_activity_entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "UserProfileId", c => c.Int());
            CreateIndex("dbo.Activity", "UserProfileId");
            AddForeignKey("dbo.Activity", "UserProfileId", "dbo.UserProfile", "Id");
            DropColumn("dbo.Activity", "UserLogin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activity", "UserLogin", c => c.String(maxLength: 100));
            DropForeignKey("dbo.Activity", "UserProfileId", "dbo.UserProfile");
            DropIndex("dbo.Activity", new[] { "UserProfileId" });
            DropColumn("dbo.Activity", "UserProfileId");
        }
    }
}
