namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuccessStatusInFeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedback", "SuccessStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedback", "SuccessStatus");
        }
    }
}
