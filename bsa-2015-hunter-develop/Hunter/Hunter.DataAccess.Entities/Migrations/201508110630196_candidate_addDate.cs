namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candidate_addDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "AddDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "AddDate");
        }
    }
}
