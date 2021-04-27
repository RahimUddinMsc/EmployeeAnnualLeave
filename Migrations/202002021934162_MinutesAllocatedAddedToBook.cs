namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinutesAllocatedAddedToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "MinutesAllocated", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "MinutesAllocated");
        }
    }
}
