namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCardAndAddCandidateVacancyInSpecialNote : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpecialNote", "CardId", "dbo.Card");
            DropIndex("dbo.SpecialNote", new[] { "CardId" });
            AddColumn("dbo.SpecialNote", "CandidateId", c => c.Int(nullable: false));
            AddColumn("dbo.SpecialNote", "VacancyId", c => c.Int());
            CreateIndex("dbo.SpecialNote", "CandidateId");
            CreateIndex("dbo.SpecialNote", "VacancyId");
            AddForeignKey("dbo.SpecialNote", "VacancyId", "dbo.Vacancy", "Id");
            AddForeignKey("dbo.SpecialNote", "CandidateId", "dbo.Candidate", "Id");
            DropColumn("dbo.SpecialNote", "CardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpecialNote", "CardId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SpecialNote", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.SpecialNote", "VacancyId", "dbo.Vacancy");
            DropIndex("dbo.SpecialNote", new[] { "VacancyId" });
            DropIndex("dbo.SpecialNote", new[] { "CandidateId" });
            DropColumn("dbo.SpecialNote", "VacancyId");
            DropColumn("dbo.SpecialNote", "CandidateId");
            CreateIndex("dbo.SpecialNote", "CardId");
            AddForeignKey("dbo.SpecialNote", "CardId", "dbo.Card", "Id");
        }
    }
}
