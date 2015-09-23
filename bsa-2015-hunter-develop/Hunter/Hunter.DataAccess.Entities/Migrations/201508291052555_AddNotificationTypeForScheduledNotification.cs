namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTypeForScheduledNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduledNotification", "NotificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ScheduledNotification", "NotificationType", c => c.Int(nullable: false));
            DropColumn("dbo.ScheduledNotification", "Pending");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScheduledNotification", "Pending", c => c.DateTime(nullable: false));
            DropColumn("dbo.ScheduledNotification", "NotificationType");
            DropColumn("dbo.ScheduledNotification", "NotificationDate");
        }
    }
}
