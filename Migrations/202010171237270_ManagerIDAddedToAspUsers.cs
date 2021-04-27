namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerIDAddedToAspUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ManagerID", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ManagerID");
        }
    }
}
