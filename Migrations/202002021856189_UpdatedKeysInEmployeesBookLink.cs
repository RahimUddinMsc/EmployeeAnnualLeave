namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedKeysInEmployeesBookLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeBookLinks", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.EmployeeBookLinks", "EmployeeId");
            AddForeignKey("dbo.EmployeeBookLinks", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeBookLinks", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeeBookLinks", new[] { "EmployeeId" });
            DropColumn("dbo.EmployeeBookLinks", "EmployeeId");
        }
    }
}
