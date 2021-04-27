using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnualLeave.Controllers
{
    public class EmployeeController : Controller
    {

        private ApplicationDbContext _context;

        public EmployeeController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(Employee emp)
        {
            //setup default role as employee
            var defRole = _context.RolesCustom.Find(1);

            //Make the object for the employee
            var newEmp = new Employee
            {
                Role = defRole,
                RoleID = defRole.RoleId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MinutesAvailable = emp.MinutesAvailable,
                MinutesUsed = 0,
            };

            _context.Employees.Add(newEmp);
            _context.SaveChanges();

            return Content("saved");
        }
        public ActionResult Display()
        {            
            var allLinks = _context.EmployeeBookLinks.ToList();
            foreach (var link in allLinks)
            {
                var emp = _context.Users.SingleOrDefault(e => e.Id == link.UserId);
                var book = _context.Books.SingleOrDefault(e => e.BookId == link.BookId);

                link.UserId = emp.Id;
                link.Book = book;

            }
            //var employeeSelect = allLinks.Join(employees, x => x.EmployeeId, y => y.EmployeeId, (x, y) => new { x, y }).FirstOrDefault();

            return View(allLinks);
        }

    }
}