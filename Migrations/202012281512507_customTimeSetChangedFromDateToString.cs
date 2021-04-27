namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customTimeSetChangedFromDateToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomTimes", "StartTime", c => c.String());
            AlterColumn("dbo.CustomTimes", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomTimes", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CustomTimes", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
