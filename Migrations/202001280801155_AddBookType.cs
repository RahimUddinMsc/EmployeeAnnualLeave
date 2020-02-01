namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        CalendarId = c.Int(nullable: false),
                        ApprovalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId);
            
            AddColumn("dbo.Employees", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "BookId");
            AddForeignKey("dbo.Employees", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "BookId", "dbo.Books");
            DropIndex("dbo.Employees", new[] { "BookId" });
            DropColumn("dbo.Employees", "BookId");
            DropTable("dbo.Books");
        }
    }
}
