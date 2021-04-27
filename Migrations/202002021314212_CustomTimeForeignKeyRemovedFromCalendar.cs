namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomTimeForeignKeyRemovedFromCalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CustomTimeSet", c => c.Boolean(nullable: false));
            DropColumn("dbo.Calendars", "CustomTimeSet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Calendars", "CustomTimeSet", c => c.Boolean(nullable: false));
            DropColumn("dbo.Books", "CustomTimeSet");
        }
    }
}
