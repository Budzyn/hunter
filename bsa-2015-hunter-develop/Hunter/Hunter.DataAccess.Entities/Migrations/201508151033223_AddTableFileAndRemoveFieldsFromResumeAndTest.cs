namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableFileAndRemoveFieldsFromResumeAndTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 2000),
                        Added = c.DateTime(nullable: false),
                        Path = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Test", "FileId", c => c.Int());
            AddColumn("dbo.Interview", "Comment", c => c.String(maxLength: 3000, fixedLength: true));
            AddColumn("dbo.Resume", "FileId", c => c.Int());
            AlterColumn("dbo.Interview", "FeedbackId", c => c.Int());
            AlterColumn("dbo.Candidate", "ResumeId", c => c.Int(nullable: true));
            CreateIndex("dbo.Test", "FileId");
            CreateIndex("dbo.Resume", "FileId");
            AddForeignKey("dbo.Test", "FileId", "dbo.Files", "Id");
            AddForeignKey("dbo.Resume", "FileId", "dbo.Files", "Id");
            DropColumn("dbo.Test", "FileName");
            DropColumn("dbo.Test", "FileStream");
            DropColumn("dbo.Interview", "Comments");
            DropColumn("dbo.Resume", "FileStream");
            DropColumn("dbo.Resume", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Resume", "FileName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Resume", "FileStream", c => c.Binary(nullable: false));
            AddColumn("dbo.Interview", "Comments", c => c.String(maxLength: 10, fixedLength: true));
            AddColumn("dbo.Test", "FileStream", c => c.Binary());
            AddColumn("dbo.Test", "FileName", c => c.String(maxLength: 255));
            DropForeignKey("dbo.Resume", "FileId", "dbo.Files");
            DropForeignKey("dbo.Test", "FileId", "dbo.Files");
            DropIndex("dbo.Resume", new[] { "FileId" });
            DropIndex("dbo.Test", new[] { "FileId" });
            AlterColumn("dbo.Interview", "FeedbackId", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Candidate", "ResumeId", c => c.Int(nullable: false));
            DropColumn("dbo.Resume", "FileId");
            DropColumn("dbo.Interview", "Comment");
            DropColumn("dbo.Test", "FileId");
            DropTable("dbo.Files");
        }
    }
}
