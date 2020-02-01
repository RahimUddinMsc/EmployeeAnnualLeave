namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetRoleTable : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Roles");
            Sql("DBCC CHECKIDENT ('Roles', RESEED, 1)");
            Sql("INSERT INTO Roles (RoleName) VALUES ('Employee')");
            Sql("INSERT INTO Roles (RoleName) VALUES ('Manager')");

        }
        
        public override void Down()
        {
        }
    }
}
