namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeBookLinkIdModelCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeBookLinks",
                c => new
                    {
                        EmployeeBookLinkId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeBookLinkId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeBookLinks", "BookId", "dbo.Books");
            DropIndex("dbo.EmployeeBookLinks", new[] { "BookId" });
            DropTable("dbo.EmployeeBookLinks");
        }
    }
}
