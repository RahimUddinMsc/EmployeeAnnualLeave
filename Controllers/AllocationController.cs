using AnnualLeave.Controllers.Api;
using AnnualLeave.Models;
using AnnualLeave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnualLeave.Controllers
{
    public class AllocationController : Controller
    {

        private ApplicationDbContext _context;

        public AllocationController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Allocation
        public ActionResult Index()
        {
            var now = DateTime.Now;
            var CalHandler = new CalendarAllocation();            
            CalHandler.CalendarDays = getCalendarDataForMonth(now.Month, now.Year);
            return View(CalHandler);
        }

        public IEnumerable<Calendar> getCalendarDataForMonth(int monthSelected, int yearSelected)
        {
            var calData = _context.Calendars.Where(r => r.Month == monthSelected && r.Year == yearSelected).ToList();
            return calData;
        }

        [Route("GetAllocationDaysForMonth")]
        public ActionResult GetAllocationDaysForMonth(int monthSelected, int yearSelected)
        {
            var CalHandler = new CalendarAllocation();
            CalHandler.CalendarDays = getCalendarDataForMonth(monthSelected, yearSelected);
            return PartialView("_AllocationContainer", CalHandler);

        }     

    }
}