namespace Hunter.DataAccess.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddedFieldInTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Test", "Added", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Test", "Added");
        }
    }
}
