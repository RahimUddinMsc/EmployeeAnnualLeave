namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBookForeignKeyToQueueModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queues", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Queues", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Queues", "UserId");
            AddForeignKey("dbo.Queues", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queues", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Queues", new[] { "UserId" });
            DropColumn("dbo.Queues", "BookId");
            DropColumn("dbo.Queues", "UserId");
        }
    }
}
