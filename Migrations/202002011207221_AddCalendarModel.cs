namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendarModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calendars",
                c => new
                    {
                        CalendarId = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        AvailableMinutes = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CalendarId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Calendars");
        }
    }
}
