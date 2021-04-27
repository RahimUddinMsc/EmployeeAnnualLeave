namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomTimeBookChangedToCustomTime : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomTimeBooks", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.CustomTimeBooks", new[] { "CalendarId" });
            CreateTable(
                "dbo.CustomTimes",
                c => new
                    {
                        CustomTimeId = c.Int(nullable: false, identity: true),
                        CalendarId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomTimeId)
                .ForeignKey("dbo.Calendars", t => t.CalendarId, cascadeDelete: true)
                .Index(t => t.CalendarId);
            
            DropTable("dbo.CustomTimeBooks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomTimeBooks",
                c => new
                    {
                        CustomTimeBookId = c.Int(nullable: false, identity: true),
                        CalendarId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomTimeBookId);
            
            DropForeignKey("dbo.CustomTimes", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.CustomTimes", new[] { "CalendarId" });
            DropTable("dbo.CustomTimes");
            CreateIndex("dbo.CustomTimeBooks", "CalendarId");
            AddForeignKey("dbo.CustomTimeBooks", "CalendarId", "dbo.Calendars", "CalendarId", cascadeDelete: true);
        }
    }
}
