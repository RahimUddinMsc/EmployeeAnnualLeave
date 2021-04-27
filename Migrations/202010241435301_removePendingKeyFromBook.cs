namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePendingKeyFromBook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "PendingId", "dbo.Pendings");
            DropIndex("dbo.Books", new[] { "PendingId" });
            DropColumn("dbo.Books", "PendingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "PendingId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "PendingId");
            AddForeignKey("dbo.Books", "PendingId", "dbo.Pendings", "PendingId", cascadeDelete: true);
        }
    }
}
