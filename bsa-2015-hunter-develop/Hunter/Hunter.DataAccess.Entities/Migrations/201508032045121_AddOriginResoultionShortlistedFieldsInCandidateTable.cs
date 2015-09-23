using System.Data.Entity.Migrations;

namespace Hunter.DataAccess.Entities.Migrations
{
    public partial class AddOriginResoultionShortlistedFieldsInCandidateTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "Origin", c => c.Int(nullable: false));
            AddColumn("dbo.Candidate", "Resoultion", c => c.Int(nullable: false));
            AddColumn("dbo.Candidate", "Shortlisted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "Shortlisted");
            DropColumn("dbo.Candidate", "Resoultion");
            DropColumn("dbo.Candidate", "Origin");
        }
    }
}
