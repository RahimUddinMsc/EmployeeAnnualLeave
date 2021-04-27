namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueueTableAddedRD2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Queues",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        CalendarId = c.Int(nullable: false),
                        position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QueueId)
                .ForeignKey("dbo.Calendars", t => t.CalendarId, cascadeDelete: true)
                .Index(t => t.CalendarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queues", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.Queues", new[] { "CalendarId" });
            DropTable("dbo.Queues");
        }
    }
}
