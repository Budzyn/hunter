namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCandidateSalaryDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidate", "Salary", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidate", "Salary", c => c.String(maxLength: 300));
        }
    }
}
