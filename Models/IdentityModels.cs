using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnnualLeave.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public double MinutesAvailable { get; set; }
        [Required]
        public double MinutesUsed { get; set; }
        [Required]
        public double ManagerID { get; set; }
        public string ProfileImage { get; set; }


    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<Pending> Pendings { get; set; }
        public DbSet<Role> RolesCustom { get; set; }
        public DbSet<CustomTime> CustomTimes { get; set; }
        public DbSet<EmployeeBookLink> EmployeeBookLinks { get; set; }
        public DbSet<Queue> Queue { get; set; }
        public DbSet<UserHolidayInfo> UserHolidayInfos { get; set; }
        public DbSet<Team> Teams { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}