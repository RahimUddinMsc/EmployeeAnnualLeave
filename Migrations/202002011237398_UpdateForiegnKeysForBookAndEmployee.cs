namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateForiegnKeysForBookAndEmployee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "BookId", "dbo.Books");
            DropIndex("dbo.Employees", new[] { "BookId" });
            AddColumn("dbo.Books", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "CalendarId");
            CreateIndex("dbo.Books", "EmployeeId");
            AddForeignKey("dbo.Books", "CalendarId", "dbo.Calendars", "CalendarId", cascadeDelete: true);
            AddForeignKey("dbo.Books", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
            DropColumn("dbo.Employees", "BookId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "BookId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Books", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Books", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.Books", new[] { "EmployeeId" });
            DropIndex("dbo.Books", new[] { "CalendarId" });
            DropColumn("dbo.Books", "EmployeeId");
            CreateIndex("dbo.Employees", "BookId");
            AddForeignKey("dbo.Employees", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
    }
}
