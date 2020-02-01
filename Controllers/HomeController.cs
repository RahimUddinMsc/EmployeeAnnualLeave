using AnnualLeave.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace AnnualLeave.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {

            var test = _context.Employees.Include(e => e.Role).ToList();
            return View(test);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int id)
        {
            var test = _context.Employees.SingleOrDefault(c => c.EmployeeId == id);
            return Content("Name: " + test.FirstName  +" Minutes available:" + test.MinutesUsed);
        }


   
    }
}