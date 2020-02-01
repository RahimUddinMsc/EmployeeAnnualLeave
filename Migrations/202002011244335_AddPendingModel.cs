namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPendingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pendings",
                c => new
                    {
                        PendingId = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        Response = c.String(),
                        approved = c.Int(nullable: false),
                        Book_BookId = c.Int(),
                    })
                .PrimaryKey(t => t.PendingId)
                .ForeignKey("dbo.Books", t => t.Book_BookId)
                .Index(t => t.Book_BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pendings", "Book_BookId", "dbo.Books");
            DropIndex("dbo.Pendings", new[] { "Book_BookId" });
            DropTable("dbo.Pendings");
        }
    }
}
