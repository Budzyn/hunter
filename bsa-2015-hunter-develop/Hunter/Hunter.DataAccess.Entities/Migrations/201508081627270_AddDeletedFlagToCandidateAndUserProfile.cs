namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeletedFlagToCandidateAndUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "IsDeleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.UserProfile", "IsDeleted", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "IsDeleted");
            DropColumn("dbo.Candidate", "IsDeleted");
        }
    }
}
