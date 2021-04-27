namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDuplicateKeyFromEmployees : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "EmployeeBookLinkId", "dbo.EmployeeBookLinks");
            DropIndex("dbo.Employees", new[] { "EmployeeBookLinkId" });
            DropColumn("dbo.Employees", "EmployeeBookLinkId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeBookLinkId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "EmployeeBookLinkId");
            AddForeignKey("dbo.Employees", "EmployeeBookLinkId", "dbo.EmployeeBookLinks", "EmployeeBookLinkId", cascadeDelete: true);
        }
    }
}
