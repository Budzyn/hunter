namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activity_type_enum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activity", "Tag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activity", "Tag", c => c.String(maxLength: 150));
        }
    }
}
