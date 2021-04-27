namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomTimeBookModelAddittion : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.CustomTimeBookId)
                .ForeignKey("dbo.Calendars", t => t.CalendarId, cascadeDelete: true)
                .Index(t => t.CalendarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomTimeBooks", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.CustomTimeBooks", new[] { "CalendarId" });
            DropTable("dbo.CustomTimeBooks");
        }
    }
}
