namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialDummyRecord1 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Calendars (Day,Month,Year,AvailableMinutes) Values (-1,-1,-1,-1)");
            Sql("DELETE FROM Books");
            Sql("DBCC CHECKIDENT ('Books', RESEED, 0)");
            Sql("INSERT INTO Books (CalendarId,ApprovalId,CustomTimeSet) Values (1,1,'false')");            
        }
        
        public override void Down()
        {
        }
    }
}
