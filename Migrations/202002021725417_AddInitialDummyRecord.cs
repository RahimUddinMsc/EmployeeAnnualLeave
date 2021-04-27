namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialDummyRecord : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Calendars (Day,Month,Year,AvailableMinutes) Values (-1,-1,-1,-1)");
        }
        
        public override void Down()
        {
        }
    }
}
