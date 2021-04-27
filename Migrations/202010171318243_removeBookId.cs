namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeBookId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Queues", "BookId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Queues", "BookId", c => c.Int(nullable: false));
        }
    }
}
