namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetAllRequestedLinkedTables : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM EmployeeBookLinks");
            Sql("DBCC CHECKIDENT ('EmployeeBookLinks', RESEED, 0)");
            Sql("DELETE FROM Employees");
            Sql("DBCC CHECKIDENT ('Employees', RESEED, 0)");

        }
        
        public override void Down()
        {
        }
    }
}
