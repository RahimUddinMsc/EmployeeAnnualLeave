namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyAddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ApprovalsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "ApprovalId");
            AddForeignKey("dbo.Books", "ApprovalId", "dbo.Approvals", "ApprovalId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "ApprovalId", "dbo.Approvals");
            DropIndex("dbo.Books", new[] { "ApprovalId" });
            DropColumn("dbo.Books", "ApprovalsId");
        }
    }
}
