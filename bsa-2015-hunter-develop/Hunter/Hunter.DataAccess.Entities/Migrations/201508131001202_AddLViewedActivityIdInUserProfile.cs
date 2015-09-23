namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLViewedActivityIdInUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "LViewedActivity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "LViewedActivity");
        }
    }
}
