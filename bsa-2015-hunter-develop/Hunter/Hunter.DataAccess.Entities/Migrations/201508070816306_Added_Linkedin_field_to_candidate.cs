namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Linkedin_field_to_candidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "Linkedin", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "Linkedin");
        }
    }
}
