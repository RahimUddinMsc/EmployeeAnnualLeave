namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedEmployeeKeyFromBook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Books", new[] { "EmployeeId" });
            DropColumn("dbo.Books", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "EmployeeId");
            AddForeignKey("dbo.Books", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
