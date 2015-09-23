namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_DateOfBirth_field_to_Candidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate", "DateOfBirth", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate", "DateOfBirth");
        }
    }
}
