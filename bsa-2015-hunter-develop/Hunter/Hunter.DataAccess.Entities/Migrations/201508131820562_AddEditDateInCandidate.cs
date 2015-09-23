namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEditDateInCandidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "EditDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "EditDate");
        }
    }
}
