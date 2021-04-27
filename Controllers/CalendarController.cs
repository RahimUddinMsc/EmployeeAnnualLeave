using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnnualLeave.Models;
using AnnualLeave.ViewModel;
using System.Data.Entity;


namespace AnnualLeave.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        private ApplicationDbContext _context;

        public CalendarController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public ActionResult Index()
        {     


            //var employees = _context.Employees;
            //var books = _context.Employees;
            //var categorizedProducts =
            //    from e in employees
            //    join b in books on e.EmployeeId equals b.EmployeeId                
            //    select new
            //    {
            //        e = e.EmployeeId, // or pc.ProdId
            //        b = b.EmployeeId
            //        // other assignments
            //    };

            //var test = categorizedProducts.FirstOrDefault();
            //var employeeSelect = employees.Join(books, x => x.EmployeeId, y => y.EmployeeId, (x, y) => new { x, y }).ToList();
            
            
            return View();
        }




    }
}