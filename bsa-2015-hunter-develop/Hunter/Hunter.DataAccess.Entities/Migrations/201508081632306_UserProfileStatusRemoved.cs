namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileStatusRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserProfile", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "Status", c => c.Int(nullable: false));
        }
    }
}
