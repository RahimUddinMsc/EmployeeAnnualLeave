namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarModelUpdateWithCustomTimeSet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calendars", "CustomTimeSet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calendars", "CustomTimeSet");
        }
    }
}
