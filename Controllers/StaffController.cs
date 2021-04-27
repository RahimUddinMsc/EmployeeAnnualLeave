using AnnualLeave.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using AnnualLeave.ViewModel;


namespace AnnualLeave.Controllers
{
    [Authorize(Roles = RoleName.CanManageStaff)]
    public class StaffController : Controller
    {

        private ApplicationDbContext _context;

        public StaffController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {

            var test = _context.Employees.Include(e => e.Role).ToList();
            var obj = new ViewModel.BookingProcess
            {
                Employees = test
            };

            return View(obj);
        }
    }
}