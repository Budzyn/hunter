namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removelocationfieldinvacancy : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vacancy", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacancy", "Location", c => c.String(maxLength: 300));
        }
    }
}
