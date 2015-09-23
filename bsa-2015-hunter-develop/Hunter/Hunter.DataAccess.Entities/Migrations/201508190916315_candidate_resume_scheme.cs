namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candidate_resume_scheme : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidate", "ResumeId", "dbo.Resume");
            DropForeignKey("dbo.Resume", "FileId", "dbo.Files");
            DropIndex("dbo.Candidate", new[] { "ResumeId" });
            DropIndex("dbo.Resume", new[] { "FileId" });
            AddColumn("dbo.Resume", "CandidateId", c => c.Int(nullable:false));
            AlterColumn("dbo.Resume", "FileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Resume", "FileId");
            CreateIndex("dbo.Resume", "CandidateId");
            AddForeignKey("dbo.Resume", "CandidateId", "dbo.Candidate", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Resume", "FileId", "dbo.Files", "Id", cascadeDelete: true);
            DropColumn("dbo.Candidate", "ResumeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidate", "ResumeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Resume", "FileId", "dbo.Files");
            DropForeignKey("dbo.Resume", "CandidateId", "dbo.Candidate");
            DropIndex("dbo.Resume", new[] { "CandidateId" });
            DropIndex("dbo.Resume", new[] { "FileId" });
            AlterColumn("dbo.Resume", "FileId", c => c.Int());
            RenameColumn(table: "dbo.Resume", name: "CandidateId", newName: "ResumeId");
            CreateIndex("dbo.Resume", "FileId");
            CreateIndex("dbo.Candidate", "ResumeId");
            AddForeignKey("dbo.Resume", "FileId", "dbo.Files", "Id");
            AddForeignKey("dbo.Candidate", "ResumeId", "dbo.Resume", "Id");
        }
    }
}
