namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_resumeSummari_candidate_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "ResumeSummary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "ResumeSummary");
        }
    }
}
