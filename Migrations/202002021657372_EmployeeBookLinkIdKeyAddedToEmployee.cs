namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeBookLinkIdKeyAddedToEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "EmployeeBookLinkId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "EmployeeBookLinkId");
            AddForeignKey("dbo.Employees", "EmployeeBookLinkId", "dbo.EmployeeBookLinks", "EmployeeBookLinkId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "EmployeeBookLinkId", "dbo.EmployeeBookLinks");
            DropIndex("dbo.Employees", new[] { "EmployeeBookLinkId" });
            DropColumn("dbo.Employees", "EmployeeBookLinkId");
        }
    }
}
