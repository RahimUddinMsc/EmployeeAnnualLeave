namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApprovalTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Approvals (Description) VALUES ('Approved')");
            Sql("INSERT INTO Approvals (Description) VALUES ('Declined')");
            Sql("INSERT INTO Approvals (Description) VALUES ('Pending')");            
        }
        
        public override void Down()
        {
        }
    }
}
