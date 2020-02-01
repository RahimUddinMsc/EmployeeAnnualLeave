namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleNamesToTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Roles (RoleName) VALUES ('Employee')");
            Sql("INSERT INTO Roles (RoleName) VALUES ('Manager')");
        }
        
        public override void Down()
        {
        }
    }
}
