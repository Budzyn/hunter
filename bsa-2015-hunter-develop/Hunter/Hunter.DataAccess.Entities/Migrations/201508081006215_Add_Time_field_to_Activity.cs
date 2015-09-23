namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Time_field_to_Activity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activity", "Time");
        }
    }
}
