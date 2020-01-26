using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnualLeave.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var test = GetEmployeeData();
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
            var test = GetEmployeeData().SingleOrDefault(c => c.EmployeeId == id);
            return Content("Name: " + test.FirstName  +" Minutes available:" + test.MinutesUsed);
        }


        private IEnumerable<Employee> GetEmployeeData()
        {
            return new List<Employee>
            {
                new Employee {EmployeeId = 1, RoleID = 1, FirstName = "mark", LastName = "tame", MinutesAvailable = 23.3, MinutesUsed = 10},
                new Employee {EmployeeId = 2, RoleID = 2, FirstName = "sally", LastName = "jenkins", MinutesAvailable = 13.3, MinutesUsed = 5}
            };
            
            
           
        }

    }
}