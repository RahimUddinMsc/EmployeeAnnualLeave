namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPendingForeignKeyTOBookObj : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "PendingId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "PendingId");
            AddForeignKey("dbo.Books", "PendingId", "dbo.Pendings", "PendingId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "PendingId", "dbo.Pendings");
            DropIndex("dbo.Books", new[] { "PendingId" });
            DropColumn("dbo.Books", "PendingId");
        }
    }
}
