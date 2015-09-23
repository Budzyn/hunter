namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPoolBackgroundColorFieldToPoolTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pool", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pool", "Color");
        }
    }
}
