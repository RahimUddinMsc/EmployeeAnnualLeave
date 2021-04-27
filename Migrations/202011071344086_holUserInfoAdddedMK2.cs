namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holUserInfoAdddedMK2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserHolidayInfoes",
                c => new
                    {
                        UserHolidayInfoID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FullDay = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UserHolidayInfoID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserHolidayInfoes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserHolidayInfoes", new[] { "UserId" });
            DropTable("dbo.UserHolidayInfoes");
        }
    }
}
