namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeLinkTableAdjustedToPointToUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmployeeBookLinks", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeeBookLinks", new[] { "EmployeeId" });
            AddColumn("dbo.EmployeeBookLinks", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.EmployeeBookLinks", "UserId");
            AddForeignKey("dbo.EmployeeBookLinks", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.EmployeeBookLinks", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeeBookLinks", "EmployeeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.EmployeeBookLinks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.EmployeeBookLinks", new[] { "UserId" });
            DropColumn("dbo.EmployeeBookLinks", "UserId");
            CreateIndex("dbo.EmployeeBookLinks", "EmployeeId");
            AddForeignKey("dbo.EmployeeBookLinks", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
