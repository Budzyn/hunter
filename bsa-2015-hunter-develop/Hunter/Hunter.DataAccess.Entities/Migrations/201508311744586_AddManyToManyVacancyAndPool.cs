namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToManyVacancyAndPool : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacancy", "PoolId", "dbo.Pool");
            DropIndex("dbo.Vacancy", new[] { "PoolId" });
            CreateTable(
                "dbo.Vacancy_Pool",
                c => new
                    {
                        VacancyId = c.Int(nullable: false),
                        PoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacancyId, t.PoolId })
                .ForeignKey("dbo.Vacancy", t => t.VacancyId, cascadeDelete: true)
                .ForeignKey("dbo.Pool", t => t.PoolId, cascadeDelete: true)
                .Index(t => t.VacancyId)
                .Index(t => t.PoolId);
            
            DropColumn("dbo.Vacancy", "PoolId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacancy", "PoolId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Vacancy_Pool", "PoolId", "dbo.Pool");
            DropForeignKey("dbo.Vacancy_Pool", "VacancyId", "dbo.Vacancy");
            DropIndex("dbo.Vacancy_Pool", new[] { "PoolId" });
            DropIndex("dbo.Vacancy_Pool", new[] { "VacancyId" });
            DropTable("dbo.Vacancy_Pool");
            CreateIndex("dbo.Vacancy", "PoolId");
            AddForeignKey("dbo.Vacancy", "PoolId", "dbo.Pool", "Id");
        }
    }
}
