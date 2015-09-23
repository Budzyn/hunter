namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddfieldtoVacancy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacancy", "UserId", c => c.Int());
            CreateIndex("dbo.Vacancy", "UserId");
            AddForeignKey("dbo.Vacancy", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacancy", "UserId", "dbo.User");
            DropIndex("dbo.Vacancy", new[] { "UserId" });
            DropColumn("dbo.Vacancy", "UserId");
        }
    }
}
