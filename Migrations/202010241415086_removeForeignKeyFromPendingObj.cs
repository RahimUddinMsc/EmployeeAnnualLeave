namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeForeignKeyFromPendingObj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pendings", "BookId", "dbo.Books");
            DropIndex("dbo.Pendings", new[] { "BookId" });
            DropColumn("dbo.Pendings", "BookId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pendings", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pendings", "BookId");
            AddForeignKey("dbo.Pendings", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
    }
}
