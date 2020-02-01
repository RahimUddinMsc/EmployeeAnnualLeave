namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDuplicateKey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "ApprovalsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ApprovalsId", c => c.Int(nullable: false));
        }
    }
}
