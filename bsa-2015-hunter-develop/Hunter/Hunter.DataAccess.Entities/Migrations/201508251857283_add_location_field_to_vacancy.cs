namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_location_field_to_vacancy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacancy", "Location", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacancy", "Location");
        }
    }
}
