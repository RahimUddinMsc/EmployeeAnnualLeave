namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleForiegnKeyToEmployee : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Employees", "RoleID");
            AddForeignKey("dbo.Employees", "RoleID", "dbo.Roles", "RoleId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "RoleID", "dbo.Roles");
            DropIndex("dbo.Employees", new[] { "RoleID" });
        }
    }
}
