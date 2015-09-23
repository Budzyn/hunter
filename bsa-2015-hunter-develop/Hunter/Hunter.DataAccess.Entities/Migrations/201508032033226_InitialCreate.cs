using System.Data.Entity.Migrations;

namespace Hunter.DataAccess.Entities.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserLogin = c.String(maxLength: 100),
                        Tag = c.String(maxLength: 150),
                        Message = c.String(nullable: false, maxLength: 2000),
                        Url = c.String(nullable: false, maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Candidate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.Binary(storeType: "image"),
                        FirstName = c.String(nullable: false, maxLength: 300),
                        LastName = c.String(maxLength: 300),
                        Email = c.String(maxLength: 300),
                        CurrentPosition = c.String(maxLength: 300),
                        Company = c.String(maxLength: 300),
                        Location = c.String(maxLength: 300),
                        Skype = c.String(maxLength: 300),
                        Phone = c.String(maxLength: 300),
                        Salary = c.String(maxLength: 300),
                        YearsOfExperience = c.Double(),
                        ResumeId = c.Int(nullable: false),
                        AddedByProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.AddedByProfileId)
                .ForeignKey("dbo.Resume", t => t.ResumeId)
                .Index(t => t.ResumeId)
                .Index(t => t.AddedByProfileId);
            
            CreateTable(
                "dbo.Card",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        VacancyId = c.Int(nullable: false),
                        Added = c.DateTime(nullable: false),
                        Stage = c.Int(nullable: false),
                        AddedByProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.AddedByProfileId)
                .ForeignKey("dbo.Vacancy", t => t.VacancyId)
                .ForeignKey("dbo.Candidate", t => t.CandidateId)
                .Index(t => t.CandidateId)
                .Index(t => t.VacancyId)
                .Index(t => t.AddedByProfileId);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardId = c.Int(nullable: false),
                        ProfileId = c.Int(),
                        Text = c.String(nullable: false, maxLength: 3000),
                        Added = c.DateTime(nullable: false),
                        Edited = c.DateTime(),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.ProfileId)
                .ForeignKey("dbo.Card", t => t.CardId)
                .Index(t => t.CardId)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 2000),
                        FileName = c.String(maxLength: 255),
                        FileStream = c.Binary(),
                        Comment = c.String(maxLength: 3000),
                        CardId = c.Int(nullable: false),
                        FeedbackId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feedback", t => t.FeedbackId)
                .ForeignKey("dbo.Card", t => t.CardId)
                .Index(t => t.CardId)
                .Index(t => t.FeedbackId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(maxLength: 15),
                        UserLogin = c.String(maxLength: 100),
                        Added = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Interview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        Comments = c.String(maxLength: 10, fixedLength: true),
                        FeedbackId = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Card", t => t.CardId)
                .Index(t => t.CardId);
            
            CreateTable(
                "dbo.SpecialNote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserLogin = c.String(nullable: false, maxLength: 200),
                        Text = c.String(nullable: false, maxLength: 3000),
                        LastEdited = c.DateTime(nullable: false),
                        CardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Card", t => t.CardId)
                .Index(t => t.CardId);
            
            CreateTable(
                "dbo.Vacancy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        Status = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Description = c.String(maxLength: 4000),
                        PoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pool", t => t.PoolId)
                .Index(t => t.PoolId);
            
            CreateTable(
                "dbo.Pool",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resume",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileStream = c.Binary(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 200),
                        PasswordHash = c.String(nullable: false, maxLength: 300),
                        State = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRole", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Candidate_Pool",
                c => new
                    {
                        CandidateId = c.Int(nullable: false),
                        PoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.PoolId })
                .ForeignKey("dbo.Candidate", t => t.CandidateId, cascadeDelete: true)
                .ForeignKey("dbo.Pool", t => t.PoolId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.PoolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleId", "dbo.UserRole");
            DropForeignKey("dbo.Candidate", "ResumeId", "dbo.Resume");
            DropForeignKey("dbo.Candidate_Pool", "PoolId", "dbo.Pool");
            DropForeignKey("dbo.Candidate_Pool", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Card", "CandidateId", "dbo.Candidate");
            DropForeignKey("dbo.Vacancy", "PoolId", "dbo.Pool");
            DropForeignKey("dbo.Card", "VacancyId", "dbo.Vacancy");
            DropForeignKey("dbo.Test", "CardId", "dbo.Card");
            DropForeignKey("dbo.SpecialNote", "CardId", "dbo.Card");
            DropForeignKey("dbo.Interview", "CardId", "dbo.Card");
            DropForeignKey("dbo.Feedback", "CardId", "dbo.Card");
            DropForeignKey("dbo.Feedback", "ProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.Card", "AddedByProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.Candidate", "AddedByProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.Test", "FeedbackId", "dbo.Feedback");
            DropIndex("dbo.Candidate_Pool", new[] { "PoolId" });
            DropIndex("dbo.Candidate_Pool", new[] { "CandidateId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Vacancy", new[] { "PoolId" });
            DropIndex("dbo.SpecialNote", new[] { "CardId" });
            DropIndex("dbo.Interview", new[] { "CardId" });
            DropIndex("dbo.Test", new[] { "FeedbackId" });
            DropIndex("dbo.Test", new[] { "CardId" });
            DropIndex("dbo.Feedback", new[] { "ProfileId" });
            DropIndex("dbo.Feedback", new[] { "CardId" });
            DropIndex("dbo.Card", new[] { "AddedByProfileId" });
            DropIndex("dbo.Card", new[] { "VacancyId" });
            DropIndex("dbo.Card", new[] { "CandidateId" });
            DropIndex("dbo.Candidate", new[] { "AddedByProfileId" });
            DropIndex("dbo.Candidate", new[] { "ResumeId" });
            DropTable("dbo.Candidate_Pool");
            DropTable("dbo.UserRole");
            DropTable("dbo.User");
            DropTable("dbo.Resume");
            DropTable("dbo.Pool");
            DropTable("dbo.Vacancy");
            DropTable("dbo.SpecialNote");
            DropTable("dbo.Interview");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Test");
            DropTable("dbo.Feedback");
            DropTable("dbo.Card");
            DropTable("dbo.Candidate");
            DropTable("dbo.Activity");
        }
    }
}
