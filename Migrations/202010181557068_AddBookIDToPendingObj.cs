namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookIDToPendingObj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pendings", "Book_BookId", "dbo.Books");
            DropIndex("dbo.Pendings", new[] { "Book_BookId" });
            RenameColumn(table: "dbo.Pendings", name: "Book_BookId", newName: "BookId");
            AlterColumn("dbo.Pendings", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pendings", "BookId");
            AddForeignKey("dbo.Pendings", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pendings", "BookId", "dbo.Books");
            DropIndex("dbo.Pendings", new[] { "BookId" });
            AlterColumn("dbo.Pendings", "BookId", c => c.Int());
            RenameColumn(table: "dbo.Pendings", name: "BookId", newName: "Book_BookId");
            CreateIndex("dbo.Pendings", "Book_BookId");
            AddForeignKey("dbo.Pendings", "Book_BookId", "dbo.Books", "BookId");
        }
    }
}
