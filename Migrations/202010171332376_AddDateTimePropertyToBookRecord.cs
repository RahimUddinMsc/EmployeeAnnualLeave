namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimePropertyToBookRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "DateTimeRequested", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "DateTimeRequested");
        }
    }
}
