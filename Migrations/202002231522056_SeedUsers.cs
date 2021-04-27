namespace AnnualLeave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0a3d161e-5d32-4e48-9ca4-2cc8bdc353f1', N'test@mail.com', 0, N'APfx6xo+XroL+9Hh9EINsz1t9YocuPhWum2I5fZdkmKgfWunQRl2F1YNofwtwljv+A==', N'690231f2-eede-475a-936e-8add659bbacd', NULL, 0, 0, NULL, 1, 0, N'test@mail.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'931ad8ea-1ce8-437a-b7c3-22d8b6fd859e', N'admin@mail.com', 0, N'AEh9gcXtGe696vKgrYcFjby5St+uwylcUkDrO+68HhMDUnMsL5DmhLAveA6icH0yTw==', N'54d7cd45-06ae-42e5-ab82-124bdb239dcd', NULL, 0, 0, NULL, 1, 0, N'admin@mail.com')

                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'7ee7672c-ffa9-4c7f-8d6c-f68833c3020b', N'CanManageStaff')
                
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'931ad8ea-1ce8-437a-b7c3-22d8b6fd859e', N'7ee7672c-ffa9-4c7f-8d6c-f68833c3020b')

            ");
        }
        
        public override void Down()
        {
        }
    }
}
