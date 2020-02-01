namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoundTwoRolesTableReset : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Roles");
            Sql("DBCC CHECKIDENT ('Roles', RESEED, 0)");
            Sql("INSERT INTO Roles (RoleName) VALUES ('Employee')");
            Sql("INSERT INTO Roles (RoleName) VALUES ('Manager')");
        }
        
        public override void Down()
        {
        }
    }
}
