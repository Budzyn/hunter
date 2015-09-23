namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_userProfile_field_to_special_not : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialNote", "UserProfileId", c => c.Int());
            CreateIndex("dbo.SpecialNote", "UserProfileId");
            AddForeignKey("dbo.SpecialNote", "UserProfileId", "dbo.UserProfile", "Id");
            DropColumn("dbo.SpecialNote", "UserLogin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpecialNote", "UserLogin", c => c.String(nullable: false, maxLength: 200));
            DropForeignKey("dbo.SpecialNote", "UserProfileId", "dbo.UserProfile");
            DropIndex("dbo.SpecialNote", new[] { "UserProfileId" });
            DropColumn("dbo.SpecialNote", "UserProfileId");
        }
    }
}
