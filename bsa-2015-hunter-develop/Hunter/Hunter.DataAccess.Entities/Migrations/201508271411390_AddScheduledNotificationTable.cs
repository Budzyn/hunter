namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScheduledNotificationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScheduledNotification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        Pending = c.DateTime(nullable: false),
                        Message = c.String(maxLength: 2000),
                        IsSent = c.Boolean(nullable: false),
                        IsShown = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidate", t => t.CandidateId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduledNotification", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.ScheduledNotification", "CandidateId", "dbo.Candidate");
            DropIndex("dbo.ScheduledNotification", new[] { "UserProfileId" });
            DropIndex("dbo.ScheduledNotification", new[] { "CandidateId" });
            DropTable("dbo.ScheduledNotification");
        }
    }
}
