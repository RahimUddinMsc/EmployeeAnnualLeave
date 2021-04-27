namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeBookKeyToPending : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pendings", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pendings", "BookId");
            AddForeignKey("dbo.Pendings", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pendings", "BookId", "dbo.Books");
            DropIndex("dbo.Pendings", new[] { "BookId" });
            DropColumn("dbo.Pendings", "BookId");
        }
    }
}
