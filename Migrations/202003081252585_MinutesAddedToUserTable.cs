namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinutesAddedToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MinutesAvailable", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "MinutesUsed", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MinutesUsed");
            DropColumn("dbo.AspNetUsers", "MinutesAvailable");
        }
    }
}
